using Reign.API.Saving;
using Reign.Configuration;
using Reign.Core.Audio;
using Reign.Core.Discord;
using Reign.Core.Input;
using Reign.Core.Saving;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Reign.Core.InstanceRequired
{
    public class Bootstrapper : MonoBehaviour, ISaveDataKnower
    {
        [Header("Audio")]
        [SerializeField]
        private AudioPool audioPool;

        [Header("Input")]
        [SerializeField]
        private InputActionAsset inputAsset;
        
        [SerializeField]        
        private string actionMapName;

        [Header("Discord")]
        [SerializeField] private DiscordStatusContainer firstDiscordStatus;

        private void Awake()
        {
            AudioManager.Refresh(audioPool);
            InputManager.Refresh(inputAsset, actionMapName);
            
            if (Config.Project.DISCORD_ENABLED)
            {
                DiscordManager.SetupAsync(firstDiscordStatus);
            }
        }

        private void OnEnable()
        {
            SceneManager.activeSceneChanged += LoadSaveData;
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= LoadSaveData;
        }

        /// <summary>
        /// Load save data when new scene is loaded
        /// </summary>
        private async void LoadSaveData(Scene _, Scene __)
        {
            await SaveManager.Load();
        }

        public void OnSave(ref SaveData data)
        {
            data.fullScreenMode = Screen.fullScreenMode;
            data.screenResolution = Screen.currentResolution;
        }

        public void OnLoad(SaveData data)
        {
            Screen.SetResolution(data.screenResolution.width, data.screenResolution.height, data.fullScreenMode, data.screenResolution.refreshRateRatio);
        }
    }
}
