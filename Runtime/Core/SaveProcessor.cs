using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Studio23.SS2.SaveSystem.Utilities;
using System.IO;

namespace Studio23.SS2.SaveSystem.Core
{
    internal class SaveProcessor
    {
        private readonly string _saveFilePath;
        private readonly bool _useEncryption;
        private readonly AESEncryptor _encryptor;

        public SaveProcessor(string saveFileName, string savefilePath, string extention = ".taz", bool enableEncryption = false, string encryptionKey = "null",
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
            await Save(json);
        }

        public async UniTask Save(string data)
        {
            if (_useEncryption && _encryptor != null)
                data = await _encryptor.Encrypt(data);

            if (File.Exists(_saveFilePath))
            {
                File.Delete(_saveFilePath);
            }
            await File.WriteAllTextAsync(_saveFilePath, data);
        }


        public async UniTask<T> Load<T>()
        {
            string jsonData = await Load();
            return JsonConvert.DeserializeObject<T>(jsonData);
        }


        public async UniTask<string> Load()
        {
            if (File.Exists(_saveFilePath))
            {
                var json = await File.ReadAllTextAsync(_saveFilePath);

                if (_useEncryption && _encryptor != null)
                    json = _encryptor.Decrypt(json);

                return json;
            }
            return string.Empty;
        }
    }
}
