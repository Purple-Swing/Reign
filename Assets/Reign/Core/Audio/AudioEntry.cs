using UnityEngine;
using System.Collections.Generic;
using System;

namespace Reign.Core.Audio
{
    [Serializable]
    public class AudioEntry
    {
        public string name;

        // Ignored when parameters are given in Audio Manager functions
        public float volume;
        public float pitch;
        
        public List<AudioClip> clips = new List<AudioClip>();
    }
}
