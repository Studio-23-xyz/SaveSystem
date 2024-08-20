using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Interfaces
{
    public interface ISaveable
    {

        
        /// <summary>
        ///     Save System Would Only save files that are Dirty in short has changes to submit
        /// </summary>
        public bool IsDirty { get; set; }

        /// <summary>
        ///     Must Return an Unique ID for save system to save file with this name
        /// </summary>
        public string GetUniqueID();

        /// <summary>
        ///     Return a serialized string for the save system to save
        /// </summary>
        /// <returns>String Data</returns>
        public UniTask<string> GetSerializedData();

        /// <summary>
        ///     Implement how your component will adjust on data load
        /// </summary>
        /// <param name="data">String data</param>
        public UniTask AssignSerializedData(string data);

        /// <summary>
        /// This should never happen. If anything comes here, You probably need to check the flow of loading.
        /// Override to manage this in edge case scenario. Strongly discouraged.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public UniTask ManageSaveLoadException(Exception exception)
        {
            Debug.Log($"{exception.Message}");
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Ideally you shouldn't be using this. However, this is for managing edge cases
        /// </summary>
        /// <returns></returns>
        public async UniTask SaveSelf()
        {
            await Core.SaveSystem.Instance.Save(this,$"Saved by {GetUniqueID()}");
        }

        public async UniTask LoadSelf()
        {
            await Core.SaveSystem.Instance.Load(this);
        }



#if UNITY_EDITOR
        /// <summary>
        ///     Debug only Method. Do not use for any other reason
        /// </summary>
        public void DeleteLocalFile()
        {
            Core.SaveSystem.Instance.DeleteKeyFromSelectedSlot(GetUniqueID()).Forget();
        }

#endif
    }
}