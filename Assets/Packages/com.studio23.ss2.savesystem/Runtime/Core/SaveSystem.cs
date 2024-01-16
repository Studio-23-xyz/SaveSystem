
using Cysharp.Threading.Tasks;
using System.Runtime.CompilerServices;
using UnityEngine;


[assembly: InternalsVisibleTo("com.studio23.ss2.savesystem.playmodetest")]
[assembly: InternalsVisibleTo("com.studio23.ss2.savesystem.editor")]
namespace Studio23.SS2.SaveSystem.Core
{

    public class SaveSystem : MonoBehaviour
    {
        public static SaveSystem Instance;

        [SerializeField] internal SaveSlotProcessor _slotProcessor;

        void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Saves All ISavable
        /// </summary>
        /// <returns></returns>
        [ContextMenu("Save")]
        public async UniTask Save()
        {
           await _slotProcessor.SaveAllSavable();
        }

        /// <summary>
        /// Loads All ISavable
        /// </summary>
        /// <returns></returns>
        [ContextMenu("Load")]
        public async UniTask Load()
        {
            await _slotProcessor.LoadAllSavable();
        }

        /// <summary>
        /// Restores Backup if available
        /// </summary>
        public async UniTask RestoreBackup()
        {
            await _slotProcessor.RestoreBackup();
        }

        /// <summary>
        /// Selects Save Slot
        /// </summary>
        /// <param name="index">Starts from 0</param>
        /// <returns></returns>
        public async UniTask SelectSlot(int index)
        {
            await _slotProcessor.SelectSlot(index);
        }

        /// <summary>
        /// Clears Selected Slot
        /// </summary>
        /// <returns></returns>
        public async UniTask ClearSelectedSlot()
        {
            await _slotProcessor.ClearSelectedSlotAsync();
        }

        /// <summary>
        /// Clears All Slots
        /// </summary>
        /// <returns></returns>
        public async UniTask ClearAllSlots()
        {
            await _slotProcessor.ClearAllSlotsAsync();
        }



    }
}