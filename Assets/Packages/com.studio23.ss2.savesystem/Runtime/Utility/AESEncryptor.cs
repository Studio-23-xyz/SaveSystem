using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Cysharp.Threading.Tasks;


namespace Studio23.SS2.SaveSystem.Utilities
{
    public class AESEncryptor
    {
        private readonly string _key;
        private readonly string _IV;

        public AESEncryptor(string key, string iv)
        {
            if (key.Length != 16 || iv.Length != 16)
                throw new ArgumentException("Key and IV must be 16 bytes (128 bits) each.");

            _key = key;
            _IV = iv;
        }

        public async UniTask<string> Encrypt(string plainText)
        {
            using var aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(_key);
            aesAlg.IV = Encoding.UTF8.GetBytes(_IV);

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using var msEncrypt = new MemoryStream();
            await using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                await using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    await swEncrypt.WriteAsync(plainText);
                }
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public string Decrypt(string cipherText)
        {
            using var aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(_key);
            aesAlg.IV = Encoding.UTF8.GetBytes(_IV);

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();
        }
    }
}