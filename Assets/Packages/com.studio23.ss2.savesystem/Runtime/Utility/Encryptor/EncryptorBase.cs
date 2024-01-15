using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Utilities
{
    internal abstract class EncryptorBase:ScriptableObject
    {
        internal abstract UniTask<string> Decrypt(string cipherText);
        internal abstract UniTask<string> Encrypt(string plainText);
    }
}