using Reign.API.Saving;
using Reign.Interfaces;
using UnityEngine;

namespace Reign.Internal
{
    public class Internal_GameDataTest : MonoBehaviour, IDataHandler
    {
        // Check if the Save System is working correctly using this script: if it has errors when trying to print values
        // or they don't appear the same in all scenes, that is the issue!

        public void LoadData(GameData data)
        {
            Debug.Log($"Save Data: {data.screenResolution.fullscreen}, {data.screenResolution.width}x{data.screenResolution.height}, {data.mixerParameters[0].exposedParameter}: {data.mixerParameters[0].value}");
        }

        public void SaveData(ref GameData data)
        {
        }
    }
}
