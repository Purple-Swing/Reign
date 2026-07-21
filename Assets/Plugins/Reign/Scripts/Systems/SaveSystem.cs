#pragma warning disable CS0162 // Unreachable code detected

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reign.Events;
using Reign.Generic;
using Reign.Generic.Saving;
using Reign.Generic.Visuals;
using Reign.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Reign.Systems
{
    public struct OnDataLoadedEvent : IEvent 
    {
        public GameData loadedData;
        public List<IDataHandler> presentHandlers;
    }

    public struct OnDataSavedEvent : IEvent
    {
        public GameData savedData;
        public List<IDataHandler> presentHandlers;
    }

    [DefaultExecutionOrder(-1)]
    public sealed class SaveSystem : System<SaveSystem>
    {
        private GameData gameData;
        private SaveFileHandler saveFileHandler = new();
        private List<IDataHandler> dataHandlers;

        /// <summary>
        /// Return a list of the present data handlers in the scene
        /// </summary>
        public static List<IDataHandler> GetDataHandlers()
        {
            // Start from MonoBehaviour and not ReignMonoBehaviour because that inherits MonoBehaviour
            return FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include).OfType<IDataHandler>().ToList();
        }

        /// <summary>
        /// Refresh list of active IDataHandlers
        /// </summary>
        public void RefreshHandlers()
        {
            dataHandlers = GetDataHandlers();
        }

        private List<IDataHandler> LoadHandlers()
        {
            foreach (var handler in dataHandlers)
            {
                handler.LoadData(gameData);
            }

            return dataHandlers;
        }

        private List<IDataHandler> SaveHandlers()
        {
            foreach (var handler in dataHandlers)
            {
                handler.SaveData(ref gameData);
            }

            return dataHandlers;
        }

        /// <summary>
        /// Load game data (asynchronously) in every present data handler
        /// </summary>
        public async Task LoadGameData()
        {
            if (!Reign.CurrentGameCertificates.SAVE_SYSTEM_ENABLED)
            {
                Debug.Log("Data tried to load, but SAVE_SYSTEM_ENABLED flag is false");
                return;
            }

            gameData = await saveFileHandler.LoadAsync();

            // Fallback
            gameData ??= new GameData();

            var present = LoadHandlers();

            _ = EventBus.Publish(new OnDataLoadedEvent { loadedData = gameData, presentHandlers = present });

            Debug.Log("Loaded data successfully");
        }

        /// <summary>
        /// Save game data (asynchronously) to every present data handler
        /// </summary>
        public async Task SaveGameData()
        {
            if (!Reign.CurrentGameCertificates.SAVE_SYSTEM_ENABLED)
            {
                Debug.Log("Data tried to save, but SAVE_SYSTEM_ENABLED flag is false");
                return;
            }

            // Save to all handlers
            var present = SaveHandlers();

            _ = EventBus.Publish(new OnDataSavedEvent { savedData = gameData, presentHandlers = present });

            await saveFileHandler.SaveAsync(gameData);

            Debug.Log("Saved data successfully");
        }

        // Runtime
        private async Task SetupAsync()
        {
            while (Reign.CurrentGameCertificates == null)
            {
                await Task.Yield();
            }

            if (!Reign.CurrentGameCertificates.SAVE_SYSTEM_ENABLED) return;

            RefreshHandlers();

            await LoadGameData();
        }

        private void OnApplicationQuit()
        {
            if (Reign.CurrentGameCertificates.SAVE_ON_QUIT)
            {
                _ = SaveGameData();
            }
        }

        private void OnEnable()
        {
            // For current scene changed, setup.
            SceneManager.activeSceneChanged += SceneChanged;
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= SceneChanged;
        }

        private void SceneChanged(Scene scene, Scene scene2)
        {
            // Scene change guarantees that we are in a new scene and can refresh data.

            RunSetup();
        }

        private async void RunSetup()
        {
            try
            {
                await SetupAsync();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}