using UnityEngine;
using Reign.Generic.Saving;
using Reign.Generic.Audio;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Audio;
using Reign.Interfaces;
using System.Threading.Tasks;

namespace Reign.Systems
{
    public sealed class AudioSystem : System<AudioSystem>
    {
        [SerializeField] private AudioPool audioPool;
        private readonly Dictionary<string, AudioPoolEntry> audioEntries = new();

        public void OnValidate()
        {
            if (audioPool == null || audioEntries == null) return;

            foreach (AudioPoolGroup group in audioPool.audioGroups)
            {
                foreach (AudioPoolEntry entry in group.audioPoolEntries)
                {
                    audioEntries[$"{group.name}.{entry.name}"] = entry;
                }
            }
        }

        /// <summary>
        /// Check if audio entries contains the key of name
        /// </summary>
        public bool HasSound(string name)
        {
            return audioEntries != null && audioEntries.ContainsKey(name);
        }

        /// <summary>
        /// Return entry in audio entries by name
        /// </summary>
        public AudioPoolEntry GetEntry(string name)
        {
            AudioPoolEntry entry = null;                // Initialise as null
            audioEntries?.TryGetValue(name, out entry); // Try get value and output entry

            return entry;
        }

        /// <summary>
        /// Get random audio clip from audio entries
        /// </summary>
        public AudioClip GetRandomClip(string entryName)
        {
            var entry = GetEntry(entryName);

            if (entry.clips.Length == 0)
            {
                return entry.clips[0];
            }

            return entry.clips[Random.Range(0, entry.clips.Length)];
        }

        /// <summary>
        /// Set up the source depending on parameters
        /// </summary>
        private static void SourceSetup(AudioSource source, AudioPoolEntry entry, Vector3? pos, bool loop = false)
        {
            source.loop = loop;
            source.volume = entry.localVolume;

            if (pos.HasValue)
            {
                source.spatialBlend = entry.spatialBlend != 0.0f ? entry.spatialBlend : 1.0f;
                source.transform.position = pos.Value;
            }
            else
            {
                source.spatialBlend = 0.0f;
            }

            return;
        }

        /// <summary>
        /// Play an audio pool entry with the same name from a given source
        /// </summary>
        public void Play(AudioSource source, string name, Vector3? pos, int index = 0, bool loop = false)
        {
            var entry = GetEntry(name);
            if (entry == null) return;

            SourceSetup(source, entry, pos);
            source.clip = entry.clips[index];
            source.Play();
        }

        /// <summary>
        /// Play one shot audio pool entry with the same name from a source. For looping, use Play()
        /// </summary>
        public void PlayOneShot(AudioSource source, string name, Vector3? pos, int index = 0)
        {
            var entry = GetEntry(name);
            if (entry == null) return;

            SourceSetup(source, entry, pos);
            source.PlayOneShot(entry.clips[index]);
        }

        /// <summary>
        /// Create a new GameObject with an audio source and play the audio pool entry with the same name
        /// </summary>
        public AudioSource PlayCreateInstance(string name, Vector3? pos, AudioMixerGroup mixerGroup, int index = 0, bool loop = false, bool destroyOnComplete = true)
        {
            GameObject newSound = new($"Sound Instance ({name})");
            AudioSource source = newSound.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = mixerGroup;
            Play(source, name, pos, index, loop);

            if (destroyOnComplete)
            {
                DestroyOnComplete(source);
            }

            return source;
        }

        private async void DestroyOnComplete(AudioSource source)
        {
            while (source != null && source.isPlaying)
            {
                await Task.Yield();
            }

            if (source != null)
            {
                Destroy(source.gameObject);
            }
        }
    }
}
