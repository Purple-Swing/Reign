using System.Collections.Generic;
using UnityEngine;

namespace Reign.API.Saving
{
    // Audio Settings
    public partial class SaveData
    {
        public Dictionary<string, float> audioMixerGroupValues = new Dictionary<string, float>
        {
            // MixerName.ParameterName, Value
            {"AudioMixer.MasterVolume", 0.0f}
        };
    }
}
