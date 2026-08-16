using Newtonsoft.Json;
using Reign.API.Saving;
using Reign.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Reign.Core.Saving
{
    public class SaveFileManager
    {
        private bool encrypt;
        private string savePath;
        private byte[] pass;
        private byte[] salt;
        private uint iter;

        private const string HeaderEnd = "\n\n";

        public SaveFileManager()
        {
            encrypt = Config.Project.ENCRYPTED_SAVES;
            savePath = Path.Combine(
                Application.persistentDataPath,
                Config.Project.SAVE_FILE_NAME
            );

            if (encrypt)
            {
                pass = Encoding.UTF8.GetBytes(Config.Project.SAVE_FILE_PASSWORD);
                salt = Encoding.UTF8.GetBytes(Config.Project.SAVE_FILE_SALT);
                iter = Config.Project.SAVE_ENCRYPTION_ITERATIONS;
            }
        }

        private static int FindBytes(byte[] source, byte[] pattern)
        {
            for (int i = 0; i <= source.Length - pattern.Length; i++)
            {
                bool match = true;

                for (int j = 0; j < pattern.Length; j++)
                {
                    if (source[i + j] != pattern[j])
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                    return i;
            }

            return -1;
        }

        public async Task<bool> SaveAsync(SaveData data)
        {
            try
            {
                string dir = Path.GetDirectoryName(savePath);

                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                string json = JsonConvert.SerializeObject(
                    data,
                    Formatting.Indented
                );

                string headingText =
                    $"Reign Version: {Config.Reign.VERSION}\n" +
                    $"Project Version: {Config.Project.VERSION}\n\n";

                byte[] heading = Encoding.UTF8.GetBytes(headingText);

                byte[] fileBytes = encrypt
                    ? Encrypt(json)
                    : Encoding.UTF8.GetBytes(json);

                byte[] output = new byte[heading.Length + fileBytes.Length];

                Buffer.BlockCopy(
                    heading,
                    0,
                    output,
                    0,
                    heading.Length
                );

                Buffer.BlockCopy(
                    fileBytes,
                    0,
                    output,
                    heading.Length,
                    fileBytes.Length
                );

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
                byte[] fileBytes = await File.ReadAllBytesAsync(savePath);

                byte[] headerEnd = Encoding.UTF8.GetBytes(HeaderEnd);

                int dataStart = FindBytes(fileBytes, headerEnd);

                if (dataStart == -1)
                {
                    throw new InvalidDataException(
                        "Invalid save file: header terminator was not found."
                    );
                }

                dataStart += headerEnd.Length;

                byte[] dataBytes = new byte[fileBytes.Length - dataStart];

                Buffer.BlockCopy(
                    fileBytes,
                    dataStart,
                    dataBytes,
                    0,
                    dataBytes.Length
                );

                string json = encrypt
                    ? Decrypt(dataBytes)
                    : Encoding.UTF8.GetString(dataBytes);

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
            aes.Padding = PaddingMode.PKCS7;

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

            using (CryptoStream cryptoStream = new(
                memStream,
                aes.CreateEncryptor(),
                CryptoStreamMode.Write))
            using (StreamWriter writer = new(
                cryptoStream,
                new UTF8Encoding(false)))
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
            aes.Padding = PaddingMode.PKCS7;

            using var key = new Rfc2898DeriveBytes(
                pass,
                salt,
                Mathf.FloorToInt(Mathf.Abs(iter)),
                HashAlgorithmName.SHA256
            );

            aes.Key = key.GetBytes(32);

            if (cipher.Length < aes.BlockSize / 8)
            {
                throw new InvalidDataException(
                    "Encrypted save is too small to contain an IV."
                );
            }

            using MemoryStream memStream = new(cipher);

            byte[] iv = new byte[aes.BlockSize / 8];

            int bytesRead = memStream.Read(iv, 0, iv.Length);

            if (bytesRead != iv.Length)
            {
                throw new InvalidDataException(
                    "Could not read the complete AES IV."
                );
            }

            aes.IV = iv;

            using CryptoStream cryptoStream = new(
                memStream,
                aes.CreateDecryptor(),
                CryptoStreamMode.Read
            );

            using StreamReader reader = new(
                cryptoStream,
                new UTF8Encoding(false)
            );

            return reader.ReadToEnd();
        }
    }
}