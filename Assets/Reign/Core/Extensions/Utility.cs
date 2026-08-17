using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Reign.Core.Extensions
{
    public static class Utility
    {
        /// <summary>
        /// Dictionary Utilities
        /// </summary>
        public static class Dictionary
        {
            public static string DictionaryPairsToString<TKey, TValue>(Dictionary<TKey, TValue> dict)
            {
                string fin = "";

                foreach (var kvp in dict)
                {
                    fin += $"{kvp.Key}: {kvp.Value}\n";
                }

                return fin;
            }
        }

        /// <summary>
        /// Task Utilities
        /// </summary>
        public static class Tasks
        {
            public static async Task WaitUntilDictionaryHasPairs<TKey, TValue>(Dictionary<TKey, TValue> dict)
            {
                while (dict.Count <= 0)
                {
                    await Task.Yield();
                }
            }

            public static async Task WaitUntilListHasValues<T>(List<T> list)
            {
                while (list.Count <= 0)
                {
                    await Task.Yield();
                }
            }

            public static async Task WaitUntilAudioSourceFinishedPlaying(AudioSource source)
            {
                while (source.isPlaying)
                {
                    await Task.Yield();
                }
            }
        }

        /// <summary>
        /// Audio Utilities
        /// </summary>
        public static class Audio
        {
            public static float DecibelToLinearAmplitude(float db)
            {
                return Mathf.Pow(10, (db/20));
            }

            public static float DecibelToLinearPower(float db)
            {
                return Mathf.Pow(10, (db/10));
            }

            public static float LinearAmplitudeToDecibel(float lin)
            {
                return 20 * Mathf.Log10(lin);
            }

            public static float LinearPowerToDecibel(float lin)
            {
                return 10 * Mathf.Log10(lin);
            }
        }

        /// <summary>
        /// Color Utilities
        /// </summary>
        public static class Color
        {
            public static string Color32ToHexString(Color32 col)
            {
                return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", col.r, col.g, col.b, col.a);
            }

            public static Color32 HexStringToColor32(string hexString)
            {
                hexString = hexString.Trim();

                // Convert given string to only letters and numbers
                hexString = hexString.Replace("0x", "");
                hexString = hexString.Replace("#", "");

                byte a = 255;
                byte r = 255;
                byte g = 255;
                byte b = 255;

                r = byte.Parse(hexString[..2], System.Globalization.NumberStyles.HexNumber);
                g = byte.Parse(hexString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                b = byte.Parse(hexString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                if (hexString.Length >= 8)
                {
                    a = byte.Parse(hexString.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                }

                return new Color32(r, g, b, a);
            }
        }
    }
}
