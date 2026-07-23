using Reign.API.Saving;
using Reign.Interfaces;
using UnityEngine;
using UnityEngine.Rendering;

namespace Reign.Systems
{
    public sealed class DisplaySystem : System<DisplaySystem>, IDataHandler
    {
        [SerializeField] Volume globalVolume;

        public void LoadData(GameData data)
        {
            globalVolume.enabled = data.screenResolution.postProcess;
            QualitySettings.vSyncCount = data.screenResolution.vsync ? 1 : 0;
            Screen.SetResolution(data.screenResolution.width, data.screenResolution.height, data.screenResolution.fullscreen);
        }

        public void SaveData(ref GameData data)
        {
            // Save current options
            data.screenResolution = new(Screen.width, Screen.height, Screen.fullScreen, QualitySettings.vSyncCount == 1,
            data.screenResolution.postProcess);
        }
    }
}