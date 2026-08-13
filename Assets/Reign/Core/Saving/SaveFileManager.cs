using Newtonsoft.Json;
using Reign.API.Saving;
using Reign.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEditor.ShaderData;

namespace Reign.Core.Saving
{
    public class SaveFileManager
    {
        private bool encrypt;
        private string savePath;
        private byte[] pass;
        private byte[] salt;
        private uint iter;

        public SaveFileManager()
        {
            encrypt = Config.Project.ENCRYPTED_SAVES;
            savePath = Path.Combine(Application.persistentDataPath, Config.Project.SAVE_FILE_NAME);
            
            if (encrypt)
            {
                pass = Encoding.UTF8.GetBytes(Config.Project.SAVE_FILE_PASSWORD);
                salt = Encoding.UTF8.GetBytes(Config.Project.SAVE_FILE_SALT);
                iter = Config.Project.SAVE_ENCRYPTION_ITERATIONS;
            }
        }

        public async Task<bool> SaveAsync(SaveData data)
        {
            try
            {
                string dir = Path.GetDirectoryName(savePath);

                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    // If the directory is a valid path but the directory doesn't exist yet, create it.
                    Directory.CreateDirectory(dir);
                }

                string json = JsonConvert.SerializeObject(data, Formatting.Indented);

                // Create the byte array with an unencrypted heading for the Reign version and project version the save originates from.

                string headingText = $"Reign Version: {Config.Reign.VERSION}\n" + "Project Version: {Config.Project.VERSION}\n\n";

                byte[] fileBytes = encrypt ? Encrypt(json) : Encoding.UTF8.GetBytes(json);

                byte[] heading = Encoding.UTF8.GetBytes(headingText);

                byte[] output = new byte[heading.Length + fileBytes.Length];

                Buffer.BlockCopy(heading, 0, output, 0, heading.Length);
                Buffer.BlockCopy(fileBytes, 0, output, heading.Length, fileBytes.Length);

                await File.WriteAllBytesAsync(savePath, output);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Save failed: " + e);
                return false;
            }
        }

        public async Task<SaveData> LoadAsync()
        {
            if (!File.Exists(savePath))
            {
                Debug.Log("Creating new save data at " + savePath);

                SaveData dat = new();

                bool created = await SaveAsync(dat);

                if (!created)
                {
                    Debug.LogError("Couldn't create new save data file.");
                    return null;
                }

                return dat;
            }

            try
            {
                byte[] fileBytes = File.ReadAllBytes(savePath);
                string json = encrypt ? Decrypt(fileBytes) : Encoding.UTF8.GetString(fileBytes);

                SaveData dat = JsonConvert.DeserializeObject<SaveData>(json);

                return dat;
            }
            catch (Exception e)
            {

                Debug.LogWarning("Load failed: " + e);
                return null;
            }
        }

        private byte[] Encrypt(string plaintext)
        {
            using Aes aes = Aes.Create();

            aes.KeySize = 256;
            aes.BlockSize = 128;

            using var key = new Rfc2898DeriveBytes(
                pass,
                salt,
                Mathf.FloorToInt(Mathf.Abs(iter)),
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

        private string Decrypt(byte[] cipher)
        {
            using Aes aes = Aes.Create();

            aes.KeySize = 256;
            aes.BlockSize = 128;

            using var key = new Rfc2898DeriveBytes(
                pass,
                salt,
                (int)Mathf.Abs(iter),
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

    }
}
