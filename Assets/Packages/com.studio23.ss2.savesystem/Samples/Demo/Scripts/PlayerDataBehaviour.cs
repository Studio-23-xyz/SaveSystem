using System;
using Cysharp.Threading.Tasks;
using Studio23.SS2.SaveSystem.Interfaces;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Samples
{
    public class PlayerDataBehaviour : MonoBehaviour, ISaveable
    {
        public string _uniqueID;
        public PlayerData playerData;

        public bool IsDirty { get; set; }

        public string GetUniqueID()
        {
            return _uniqueID;
        }

        public UniTask AssignSerializedData(string data)
        {
            playerData = JsonUtility.FromJson<PlayerData>(data);
            return UniTask.CompletedTask;
        }

        public UniTask ManageSaveLoadException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public UniTask<string> GetSerializedData()
        {
            string data = JsonUtility.ToJson(playerData);
            return new UniTask<string>(data);
        }
    }
}