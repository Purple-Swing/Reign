using Reign.Generic.Saving;
using Reign.Interfaces;
using UnityEngine;
using UnityEngine.Rendering;

namespace Reign.Systems
{
    public sealed class DisplaySystem : System<DisplaySystem>, IDataHandler
    {
        [SerializeField] Volume globalVolume;

        public void LoadData(GameData DATA)
        {
            globalVolume.enabled = DATA.screenResolution.postProcess;
            QualitySettings.vSyncCount = DATA.screenResolution.vsync ? 1 : 0;
            Screen.SetResolution(DATA.screenResolution.width, DATA.screenResolution.height, DATA.screenResolution.fullscreen);
        }

        public void SaveData(ref GameData DATA)
        {
            // Save current options
            DATA.screenResolution = new(Screen.width, Screen.height, Screen.fullScreen, QualitySettings.vSyncCount == 1,
            DATA.screenResolution.postProcess);
        }
    }
}