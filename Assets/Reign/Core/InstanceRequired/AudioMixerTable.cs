using System.Collections.Generic;
using Reign.API.Saving;
using Reign.Core.Extensions;
using Reign.Core.Generic;
using Reign.Core.Saving;
using UnityEngine;
using UnityEngine.Audio;

namespace Reign.Core.InstanceRequired
{
    public class AudioMixerTable : Singleton<AudioMixerTable>, ISaveDataKnower
    {
        [SerializeField]
        private List<AudioMixer> mixerList = new();
        
        /// <summary>
        /// Get a mixer by name of the Object.
        /// </summary>
        public bool GetMixerByName(string name, out AudioMixer found)
        {
            foreach (var mixer in mixerList)
            {
                if (mixer.name == name)
                {
                    found = mixer;
                    return true;
                }
            }

            Debug.LogWarning($"Mixer by name '{name}' was not found.");
            found = null;
            return false;
        }

        /// <summary>
        /// Return the output mixer group of a mixer with the name of mixerName.
        /// </summary>
        public AudioMixerGroup GetOutputMixerGroupByName(string mixerName)
        {
            if (GetMixerByName(mixerName, out var mixer))
            {
                return mixer.outputAudioMixerGroup;
            }
            else
            {
                return null;
            }
        }

        public void OnSave(ref SaveData data)
        {
        }

        public void OnLoad(SaveData data)
        {
            // Set all registered audio mixer values to saved values.

            foreach (var pair in data.audioMixerGroupValues)
            {
                var name = pair.Key.Split(".");

                var mixerNamePart = name[0];
                var parameterNamePart = name[1];

                if (GetMixerByName(mixerNamePart, out var mixer))
                {
                    mixer.SetFloat(parameterNamePart, pair.Value);
                }
            }
        }
    }
}
