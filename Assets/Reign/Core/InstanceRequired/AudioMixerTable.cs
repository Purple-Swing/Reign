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
        
        public AudioMixer GetMixerByName(string name, out AudioMixer found)
        {
            foreach (var mixer in mixerList)
            {
                if (mixer.name == name)
                {
                    found = mixer;
                    return mixer;
                }
            }

            found = null;
            return null;
        }

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

        public void GetMixerSetFloat(string mixerName, string param, float value)
        {
            if (GetMixerByName(mixerName, out var mixer))
            {
                mixer.SetFloat(param, value);
            }
        }
        public void OnSave(ref SaveData data)
        {
        }

        public void OnLoad(SaveData data)
        {
            foreach (var pair in data.audioMixerGroupValues)
            {
                var name = pair.Key.Split(".");

                var mixerNamePart = name[0];
                var parameterNamePart = name[1];

                GetMixerSetFloat(mixerNamePart, parameterNamePart, pair.Value);
            }
        }
    }
}
