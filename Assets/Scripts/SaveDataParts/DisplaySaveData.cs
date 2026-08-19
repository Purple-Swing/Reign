using UnityEngine;

namespace Reign.API.Saving
{
    // Screen Settings
    public partial class SaveData
    {
        public Resolution screenResolution = new()
        {
            // Create resolution with width, height and refresh rate of current monitor

            width = Screen.currentResolution.width,
            height = Screen.currentResolution.height,
            refreshRateRatio = Screen.currentResolution.refreshRateRatio
        };

        public FullScreenMode fullScreenMode = FullScreenMode.MaximizedWindow;
    }
}
