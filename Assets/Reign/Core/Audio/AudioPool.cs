using UnityEngine;
using System.Collections.Generic;

namespace Reign.Core.Audio
{
    [CreateAssetMenu(fileName = "Audio Pool", menuName = "Reign/New Audio Pool")]
    public class AudioPool : ScriptableObject
    {
        public List<AudioEntryGroup> groups = new List<AudioEntryGroup>();
    }
}
