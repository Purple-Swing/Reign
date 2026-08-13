using Reign.API.Saving;
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
            // Find all scripts that implement ISaveDataKnower
            return GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include).OfType<ISaveDataKnower>().ToList();
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
            SaveAllKnowers();

            await saveFileManager.SaveAsync(saveData);

            Debug.Log("SaveData saved successfully.");

            return saveData;
        }

        public async static Task<SaveData> Load()
        {
            saveData = await saveFileManager.LoadAsync();

            saveData ??= new();

            LoadAllKnowers();

            Debug.Log("SaveData loaded successfully.");

            return saveData;
        }
    }
}
