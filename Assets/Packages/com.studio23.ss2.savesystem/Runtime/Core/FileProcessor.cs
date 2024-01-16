using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Studio23.SS2.SaveSystem.Utilities;
using System.IO;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Core
{
    public class FileProcessor : MonoBehaviour
    {

        [SerializeField]internal EncryptorBase _encryptor;

        public async UniTask Save<T>(T data,string _saveFilePath)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
           
            if (_encryptor != null)
                json = await _encryptor.Encrypt(json);

            if (File.Exists(_saveFilePath))
            {
                File.Delete(_saveFilePath);
            }
            await File.WriteAllTextAsync(_saveFilePath, json);
        }


        public async UniTask<T> Load<T>(string _saveFilePath)
        {

            if (File.Exists(_saveFilePath))
            {
                string json = await File.ReadAllTextAsync(_saveFilePath);

                if (_encryptor != null)
                    json =  await _encryptor.Decrypt(json);
                
                return JsonConvert.DeserializeObject<T>(json);
            }
            return default;

        }



    }
}
