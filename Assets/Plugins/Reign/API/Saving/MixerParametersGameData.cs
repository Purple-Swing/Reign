using Reign.Generic.Audio;
using System.Collections.Generic;
using UnityEngine;

namespace Reign.API.Saving
{
    // Mixer parameters
    public sealed partial class GameData
    {
        public List<MixerParameter> mixerParameters = new()
        {
            new() { value = -1.94f, exposedParameter = "Master Volume"}
        };
    }
}
