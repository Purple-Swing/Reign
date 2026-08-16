using Reign.API.Saving;
using Reign.Configuration;
using Reign.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Reign.Core.Saving
{
    public static class SaveManager
    {
        public static SaveData SaveData => saveData;

        private static SaveData saveData;
        private static readonly SaveFileManager saveFileManager = new();
        private static readonly List<ISaveDataKnower> saveDataKnowers = new();

        public static List<ISaveDataKnower> RefreshKnowers()
        {
            saveDataKnowers.Clear();

            saveDataKnowers.AddRange(GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include).OfType<ISaveDataKnower>());

            return saveDataKnowers;
        }

        private static void LoadAllKnowers()
        {
            foreach (var item in saveDataKnowers)
            {
                item.OnLoad(saveData);
            }
        }

        private static void SaveAllKnowers()
        {
            foreach (var item in saveDataKnowers)
            {
                item.OnSave(ref saveData);
            }
        }

        public async static Task<SaveData> Save()
        {
            RefreshKnowers();
            SaveAllKnowers();

            await saveFileManager.SaveAsync(saveData);

            Debug.Log("SaveData saved successfully.");
            return saveData;
        }

        public async static Task<SaveData> Load()
        {
            saveData = await saveFileManager.LoadAsync();

            saveData ??= new();

            RefreshKnowers();
            LoadAllKnowers();

            Debug.Log("SaveData loaded successfully.");

            if (Config.Project.DEBUG_SPIT_SAVE_VALUES)
            {
                var text = $"Save Data Spat:\nMixer Group Values: {Utility.DictionaryPairsToString(saveData.audioMixerGroupValues)}" + 
                $"Fullscreen: {saveData.fullScreenMode}\nScreen Resolution: {saveData.screenResolution}";

                Debug.Log(text);
            }

            return saveData;
        }
    }
}
