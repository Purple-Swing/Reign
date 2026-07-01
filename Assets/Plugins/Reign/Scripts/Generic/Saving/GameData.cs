using System.Collections.Generic;
using Reign.Generic.Audio;
using Reign.Generic.Visuals;
using UnityEngine;

namespace Reign.Generic.Saving
{
    [System.Serializable]
    public sealed class GameData
    {
        public ScreenSettings screenResolution;

        public List<MixerParameter> mixerParameters;

        // The constructor allows default values that act as a fallback when the data doesn't exist.
        public GameData()
        {
            screenResolution = new ScreenSettings(1280, 720, false, true, true);

            mixerParameters = new()
            {
                new() { value = -1.94f, exposedParameter = "Master Volume"}
            };
        }
    }
}