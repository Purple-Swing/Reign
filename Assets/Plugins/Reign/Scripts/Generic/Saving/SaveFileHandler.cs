using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Reign.Systems;
using UnityEngine;
using Newtonsoft.Json;

namespace Reign.Generic.Saving
{
    public sealed class SaveFileHandler
    {
        // We only use SaveFileHandler in SaveSystem, so we don't need to check 
        // that CurrentGameCertificates is valid. (SaveSystem does it for us.)

        private byte[] Pass => Encoding.UTF8.GetBytes(Reign.CurrentGameCertificates.SAVE_PASSWORD);
        private byte[] Salt => Encoding.UTF8.GetBytes(Reign.CurrentGameCertificates.SAVE_SALT);
        private string SavePath => Path.Combine(Application.persistentDataPath, Reign.CurrentGameCertificates.SAVE_FILE_DIRECTORY);
        private bool DoEncrypt => Reign.CurrentGameCertificates.SAVE_ENCRYPT;

        #region API

        /// <summary>
        /// Save asynchronously by writing all bytes to the save path
        /// </summary>
        public async Task<bool> SaveAsync(GameData data)
        {
            try
            {
                string directory = Path.GetDirectoryName(SavePath);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonConvert.SerializeObject(data, Formatting.Indented);

                byte[] fileBytes = DoEncrypt ? Encrypt(json) : Encoding.UTF8.GetBytes(json);

                await File.WriteAllBytesAsync(SavePath, fileBytes);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Save failed: {e}");
                return false;
            }
        }

        /// <summary>
        /// Alternative to SaveAsync() where it is instead synchronous
        /// </summary>
        public void SaveSync(GameData data)
        {
            try
            {
                string directory = Path.GetDirectoryName(SavePath);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                byte[] fileBytes = DoEncrypt ? Encrypt(json) : Encoding.UTF8.GetBytes(json);

                File.WriteAllBytes(SavePath, fileBytes);
            }
            catch (Exception e)
            {
                Debug.LogError($"Save failed: {e}");
            }
        }

        /// <summary>
        /// Load asynchronously from the save path
        /// </summary>
        public async Task<GameData> LoadAsync()
        {
            // Async load, create new data if the file is missing

            if (!File.Exists(SavePath))
            {
                Debug.LogWarning("Save file missing, creating new data");

                GameData newGameData = new();
                bool created = await SaveAsync(newGameData);

                if (!created)
                {
                    Debug.LogError("Couldn't create new file");
                    return null;
                }

                return newGameData;
            }

            try
            {
                byte[] fileBytes = File.ReadAllBytes(SavePath);

                string json = DoEncrypt ? Decrypt(fileBytes) : Encoding.UTF8.GetString(fileBytes);

                GameData data = JsonConvert.DeserializeObject<GameData>(json);

                return data;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Load failed: {e}");
                return null;
            }
        }

        #endregion

        #region Encryption

        /// <summary>
        /// Encrypt plaintext using GameCertificates' password and salt
        /// </summary>
        private byte[] Encrypt(string plaintext)
        {
            using Aes aes = Aes.Create();

            aes.KeySize = 256;
            aes.BlockSize = 128;

            using var key = new Rfc2898DeriveBytes(
                Pass,
                Salt,
                Reign.CurrentGameCertificates.SAVE_ITERATIONS,
                HashAlgorithmName.SHA256
            );

            aes.Key = key.GetBytes(32);
            aes.GenerateIV();

            using MemoryStream memStream = new();

            memStream.Write(aes.IV, 0, aes.IV.Length);

            using (CryptoStream cryptoStream = new(memStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (StreamWriter writer = new(cryptoStream))
            {
                writer.Write(plaintext);
            }

            return memStream.ToArray();
        }

        /// <summary>
        /// Decrypt byte array back into plaintext using GameCertificates' password and salt
        /// </summary>
        private string Decrypt(byte[] cipher)
        {
            using Aes aes = Aes.Create();

            aes.KeySize = 256;
            aes.BlockSize = 128;

            using var key = new Rfc2898DeriveBytes(
                Pass,
                Salt,
                Reign.CurrentGameCertificates.SAVE_ITERATIONS,
                HashAlgorithmName.SHA256
            );

            aes.Key = key.GetBytes(32);

            using MemoryStream memStream = new(cipher);

            byte[] iv = new byte[16];
            memStream.Read(iv, 0, iv.Length);

            aes.IV = iv;

            using CryptoStream cryptoStream = new(memStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using StreamReader reader = new(cryptoStream);

            return reader.ReadToEnd();
        }

        #endregion
    }
}