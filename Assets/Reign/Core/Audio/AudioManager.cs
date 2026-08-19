using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Reign.Core.Extensions;
using System.Threading.Tasks;
using UnityEngine.Audio;
using Reign.Core.InstanceRequired;

namespace Reign.Core.Audio
{
    public static class AudioManager
    {
        private static AudioPool pool;
        private static Dictionary<string, AudioEntry> entries;

        public static void Refresh(AudioPool givenPool)
        {
            if (givenPool == null) return;
            
            // Set pool to the new pool.
            pool = givenPool;

            foreach (AudioEntryGroup group in givenPool.groups)
            {
                if (group == null || string.IsNullOrEmpty(group.name)) continue;

                // For each group.
                foreach (AudioEntry entry in group.entries)
                {
                    if (entry == null || string.IsNullOrEmpty(entry.name)) continue;

                    // For each entry in group: Try add entry as (group).(name)

                    // 'TryAdd' refuses duplicates.
                    entries.TryAdd($"{group.name}.{entry.name}", entry);
                }
            }
        }

        public static AudioEntry FindAudioEntryByName(string name)
        {
            if (entries.TryGetValue(name, out var entry))
            {
                // If entries contains name, get the entry named name.
                return entry;
            }
            
            return null;
        }

        /// <summary>
        /// Setup the given Audio Source and return its Audio Clip
        /// </summary>
        public static AudioClip SetupSource(AudioSource source, AudioEntry entry, int? clipIndex = null, string clipName = "", Vector3? pos = null, float? volume = null, float? pitch = null)
        {
            if (pos.HasValue)
            {
                // Audio falloff settings have to be predefined by the source.
                
                // If we give position a value:

                // Turn spatial audio on.
                source.spatialBlend = 1.0f;

                // Since every Audio Source should be contained in its own game object...
                // Set the transform position of the object the source is added on to the position value.
                source.transform.position = pos.Value;
            }
            else
            {
                source.spatialBlend = 0.0f;

                // Unless we want to make this system meaninglessly complex, you have to reset the source position manually.
            }

            if (!volume.HasValue)
            {
                // If the volume we have given doesn't have a value:
                // Set the source volume to the entry volume.

                // If the entry is null default the volume to 1.0f.

                source.volume = entry != null ? 1.0f : entry.volume;
            }
            else
            {
                // If we give the volume a value: use it instead.
                source.volume = volume.Value;
            }

            if (!pitch.HasValue)
            {
                // Same logic applies.
                source.pitch = entry != null ? 1.0f : entry.pitch;
            }
            else
            {
                source.pitch = pitch.Value;
            }

            if (entry != null)
            {
                // If there is a relevant entry:

                if (!clipIndex.HasValue)
                {
                    // If the clip index is null:
                    // The clip of the audio source should be the entry's first clip.
                    return source.clip = entry.clips[0];
                }
                else
                {
                    // However if there is a given clip index:
                    // Set the clip to the entry's clip at the clip index.
                    return source.clip = entry.clips[clipIndex.Value];
                }
            }
            else
            {
                if (string.IsNullOrEmpty(clipName) || !clipIndex.HasValue) return null;

                // If we have given a clip name, and the clip index isn't null:

                // Find the correct clip by name
                return source.clip = FindAudioEntryByName(clipName).clips[clipIndex.Value];
            }
        }

        /// <summary>
        /// Play a sound from a source given an Audio Entry
        /// </summary>
        public static void PlayWithEntry(AudioSource source, AudioEntry entry, int? clipIndex = null, Vector3? pos = null)
        {
            // Setup source with source, entry and given clip index (defaults to zero)

            SetupSource(source, entry, clipIndex, "", pos);
            source.Play();
        }

        /// <summary>
        /// Play a one shot clip from a source without an Audio Entry, searching by name instead
        /// </summary>
        public static void PlayOneShot(AudioSource source, string name, int clipIndex = 0, float volume = 1.0f, float pitch = 1.0f)
        {
            // Setup source with source, no entry: but define clip index etc.

            source.PlayOneShot(SetupSource(source, null, clipIndex, name, null, volume, pitch));
        }

        /// <summary>
        /// Play a sound from a source at a defined world position
        /// </summary>
        public static void PlayAtWorldPosition(AudioSource source, string name, int clipIndex, Vector3 pos, float volume = 1.0f, float pitch = 1.0f)
        {
            // Setup source with all values, as well as a world position

            SetupSource(source, null, clipIndex, name, pos, volume, pitch);
            source.Play();
        }

        /// <summary>
        /// Create an audio source and play a sound from it without using an Audio Entry.
        /// </summary>
        public static async Task<AudioSource> PlayAndCreateSource(string name, int clipIndex, Vector3 pos, float volume = 1.0f, float pitch = 1.0f, bool destroyOnComplete = true, string mixerGroupName = "")
        {
            var sourceObject = new GameObject();
            var source = sourceObject.AddComponent<AudioSource>();

            if (AudioMixerTable.Current != null)
            {
                source.outputAudioMixerGroup = AudioMixerTable.Current.GetOutputMixerGroupByName(mixerGroupName);
            }
            else
            {
                Debug.LogWarning("No AudioMixerTable instance was found: Output Audio Mixer Group of source could not be set.");    
            }

            SetupSource(source, null, clipIndex, name, pos, volume, pitch);
            source.Play();

            if (destroyOnComplete)
            {
                await Utility.Tasks.WaitUntilAudioSourceFinishedPlaying(source);
                return null;
            }
            else
            {
                return source;
            }
        }
    }
}
