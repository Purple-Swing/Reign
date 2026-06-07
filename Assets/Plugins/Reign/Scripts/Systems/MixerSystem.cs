using System;
using System.Collections.Generic;
using Reign.Generic.Audio;
using Reign.Generic.Saving;
using Reign.Interfaces;
using UnityEngine;
using UnityEngine.Audio;

namespace Reign.Systems
{
    public sealed class MixerSystem : System<MixerSystem>, IDataHandler
    {
        [SerializeField] private List<AudioMixer> mixers;

        public AudioMixer GetMixerByName(string name)
        {
            foreach (var mixer in mixers)
            {
                if (mixer != null && mixer.name == name)
                {
                    return mixer;
                }
            }
            return null;
        }

        public void SetParameter(string name, float value)
        {
            foreach (var mixer in mixers)
            {
                if (mixer == null) continue;

                mixer.SetFloat(name, value);
            }
        }

        public void LoadData(GameData data)
        {
            foreach (var param in data.mixerParameters)
            {
                SetParameter(param.exposedParameter, param.value);
            }
        }

        public void SaveData(ref GameData data)
        {
        }
    }
}