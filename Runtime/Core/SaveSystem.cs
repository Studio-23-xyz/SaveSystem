using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Studio23.SS2.SaveSystem.Data;
using Studio23.SS2.SaveSystem.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Studio23.SS2.SaveSystem.Core
{
    public class SaveSystem : MonoBehaviour
    {
        public static SaveSystem Instance;

        [SerializeField] internal SaveSlotProcessor _slotProcessor;
        public UnityEvent OnLoadBegin;
        public UnityEvent OnLoadComplete;
        public UnityEvent OnSaveBegin;
        public UnityEvent OnSaveComplete;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }


        public async UniTask Initialize()
        {
            await _slotProcessor.Initialize();
        }

        /// <summary>
        ///  By Default it saves all savable regardless of it's state
        ///  If DirtyOnly is passed then it only saves Those who's state is dirty
        ///  On Save All ISavable's IsDirty is set to false
        ///  And attempts to save the files to cloud accordingly with the given provider
        /// </summary>
        /// <param name="description">Optional Description to be shown in game</param>
        /// <param name="dirtyOnly"></param>
        /// <returns></returns>
        public async UniTask Save(string description = "", bool dirtyOnly = false)
        {
            OnSaveBegin?.Invoke();
            await _slotProcessor.SaveAllSavable(dirtyOnly, description);
            OnSaveComplete?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savableComponent">This is for saving ISavable components externally</param>
        /// <param name="description">Optional Description to be shown in game</param>
        /// <returns></returns>
        public async UniTask Save(ISaveable savableComponent, string description)
        {
            OnSaveBegin?.Invoke();
            await _slotProcessor.SaveISavables(new List<ISaveable>(){savableComponent}, description);
            OnSaveComplete?.Invoke();
        }

        /// <summary>
        ///     Loads All ISavable
        /// </summary>
        /// <returns></returns>
        public async UniTask Load()
        {
            OnLoadBegin?.Invoke();
            await _slotProcessor.LoadAllSavable();
            OnLoadComplete?.Invoke();
        }

        /// <summary>
        /// This is for Loading a component Externally
        /// </summary>
        /// <param name="savableComponent"></param>
        /// <returns></returns>
        public async UniTask Load(ISaveable savableComponent)
        {
            OnLoadBegin?.Invoke();
            await _slotProcessor.LoadISavables(new List<ISaveable>() { savableComponent });
            OnLoadComplete?.Invoke();
        }


        /// <summary>
        /// Loads Savables in the order they are given in the list
        /// </summary>
        /// <param name="orderedList"></param>
        /// <returns></returns>
        public async UniTask Load(List<ISaveable> orderedList)
        {
            OnLoadBegin?.Invoke();
            await _slotProcessor.LoadISavables(orderedList);
            OnLoadComplete?.Invoke();
        }


        /// <summary>
        ///     Restores Backup if available
        ///     If not available locally attempts to restore from cloud
        ///     if still fails then throws error
        ///     Wrap in try catch and show proper UI
        /// </summary>
        public async UniTask RestoreBackup()
        {
            await _slotProcessor.RestoreBackup();
        }

        /// <summary>
        ///     Returns Slot metadata for the given index.
        ///     If it exists locally then returns that.
        ///     If it doesn't exists creates a new slot meta and returns that
        /// </summary>
        /// <param name="index">Index starts from 0</param>
        /// <returns></returns>
        public async UniTask<SaveSlot> GetSlotMeta(int index)
        {
            return await _slotProcessor.GetSaveSlotMetaDataLocal(index);
        }


        /// <summary>
        ///     Selects Save Slot
        /// </summary>
        /// <param name="index">Starts from 0</param>
        /// <returns></returns>
        public async UniTask SelectSlot(int index)
        {
            await _slotProcessor.SelectSlot(index);
        }

        /// <summary>
        ///     Selects Last Selected Slot from PlayerPrefs.
        ///     If not found selects 0 index
        /// </summary>
        /// <returns></returns>
        public async UniTask SelectLastSelectedSlot()
        {
            await _slotProcessor.SelectLastSelectedSlot();
        }

        public async UniTask SyncSelectedSlotData()
        {
            await _slotProcessor.SyncSelectedSlotData();
        }

        /// <summary>
        ///     Clears selected slot from local
        /// </summary>
        /// <returns></returns>
        public async UniTask ClearSelectedSlot()
        {
            await _slotProcessor.ClearSelectedSlotAsync();
        }

        /// <summary>
        ///     Clears selected slot from cloud
        /// </summary>
        /// <returns></returns>
        public async UniTask ClearSelectedSlotCloud()
        {
            await _slotProcessor.ClearSelectedSlotCloudAsync();
        }

        /// <summary>
        ///     Clears All Slots
        /// </summary>
        /// <returns></returns>
        public async UniTask ClearAllSlots()
        {
            await _slotProcessor.ClearAllSlotsAsync();
        }

#if UNITY_EDITOR
        public async UniTask DeleteKeyFromSelectedSlot(string key)
        {
            await _slotProcessor.DeleteKeyFromSelectedSlot(key);
        }
#endif
    }
}