using UnityEngine;
using System.Collections.Generic;
using System;

namespace Reign.Core.Audio
{
    [Serializable]
    public class AudioEntryGroup
    {
        public string name;
        public List<AudioEntry> entries = new List<AudioEntry>();
    }
}
