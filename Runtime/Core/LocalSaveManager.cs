using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Studio23.SS2.SaveSystem.Utilities;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Core
{
    public class LocalSaveManager
    {
        private readonly string _saveFilePath;
        private readonly bool _useEncryption;
        private readonly AESEncryptor _encryptor;

        public LocalSaveManager(string saveFileName, bool enableEncryption = false, string encryptionKey = "null",
            string encryptionIV = "null")
        {
            // Define the path where save files will be stored (e.g., in the Application.persistentDataPath).
            _saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);
            _useEncryption = enableEncryption;

            if (_useEncryption && !string.IsNullOrEmpty(encryptionKey) && !string.IsNullOrEmpty(encryptionIV))
                _encryptor = new AESEncryptor(encryptionKey, encryptionIV);
        }

        // Save data to a local file using Newtonsoft.Json serialization.
        public async UniTask Save<T>(T data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);

                if (_useEncryption && _encryptor != null)
                    // Encrypt the JSON data before saving.
                    json = await _encryptor.Encrypt(json);

                await File.WriteAllTextAsync(_saveFilePath, json);
                Debug.Log("Data saved locally.");
            }
            catch (Exception ex)
            {
                Debug.LogError("Error saving data: " + ex.Message);
            }
        }

        
        public async UniTask<T> Load<T>()
        {
            try
            {
                if (File.Exists(_saveFilePath))
                {
                    var json = await File.ReadAllTextAsync(_saveFilePath);

                    if (_useEncryption && _encryptor != null)
                        // Decrypt the JSON data before deserialization.
                        json = _encryptor.Decrypt(json);

                    return JsonConvert.DeserializeObject<T>(json);
                }

                Debug.LogWarning("No save data found.");
                return default;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error loading data: " + ex.Message);
                return default;
            }
        }
    }
}