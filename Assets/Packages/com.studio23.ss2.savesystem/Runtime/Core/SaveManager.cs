using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Studio23.SS2.SaveSystem.Utilities;
using System.IO;

namespace Studio23.SS2.SaveSystem.Core
{
    internal class SaveManager
    {
        private readonly string _saveFilePath;
        private readonly bool _useEncryption;
        private readonly AESEncryptor _encryptor;

        public SaveManager(string saveFileName, string savefilePath, string extention = ".taz", bool enableEncryption = false, string encryptionKey = "null",
            string encryptionIV = "null")
        {

            _saveFilePath = Path.Combine(savefilePath, $"{saveFileName}{ extention}");
            _useEncryption = enableEncryption;

            if (_useEncryption && !string.IsNullOrEmpty(encryptionKey) && !string.IsNullOrEmpty(encryptionIV))
                _encryptor = new AESEncryptor(encryptionKey, encryptionIV);
        }

        public async UniTask Save<T>(T data)
        {

            var json = JsonConvert.SerializeObject(data, Formatting.Indented);

            if (_useEncryption && _encryptor != null)
                json = await _encryptor.Encrypt(json);

            if (File.Exists(_saveFilePath))
            {
                File.Delete(_saveFilePath);
            }

            await File.WriteAllTextAsync(_saveFilePath, json);


        }


        public async UniTask<T> Load<T>()
        {

            if (File.Exists(_saveFilePath))
            {
                var json = await File.ReadAllTextAsync(_saveFilePath);

                if (_useEncryption && _encryptor != null)
                    json = _encryptor.Decrypt(json);

                return JsonConvert.DeserializeObject<T>(json);
            }

            return default;
        }

    }
}
