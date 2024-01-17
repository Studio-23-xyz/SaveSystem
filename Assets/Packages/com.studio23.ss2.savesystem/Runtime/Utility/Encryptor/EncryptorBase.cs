using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Utilities
{
    public abstract class EncryptorBase:ScriptableObject
    {
        public abstract UniTask<string> Decrypt(string cipherText);
        public abstract UniTask<string> Encrypt(string plainText);
    }
}