using Cysharp.Threading.Tasks;
using Studio23.SS2.SaveSystem.Data;
using Studio23.SS2.SaveSystem.Interfaces;
using Studio23.SS2.SaveSystem.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;


[assembly: InternalsVisibleTo("PlayMode")]
[assembly: InternalsVisibleTo("com.studio23.ss2.savesystem.editor")]
namespace Studio23.SS2.SaveSystem.Core
{

    public delegate void SaveEvent();

    public class SaveSystem : MonoBehaviour
    {
        public static SaveSystem Instance;


        internal List<SaveSlot> _slot;
        private int _selectedSlotIndex;
        private SaveSlot SelectedSlot => _slot[_selectedSlotIndex];

        [Header("Config")]
        [SerializeField] internal int _slotCount = 3;
        [SerializeField] internal bool _enableEncryption;
        [SerializeField] internal string _encryptionKey;
        [SerializeField] internal string _encryptionIV;

        [SerializeField] internal string _saveRootFolderName = "SaveData";
        [SerializeField] internal string _cloudsaveFileName = "cloud.sav";

        internal string SavePath => Path.Combine(Application.persistentDataPath, _saveRootFolderName);
        internal string CloudSavePath => Path.Combine(Application.persistentDataPath, _cloudsaveFileName);
        internal string SelectedSlotPath => Path.Combine(SavePath, SelectedSlot.Name);


        public SaveEvent OnSaveComplete;
        public SaveEvent OnLoadComplete;

        public SaveEvent OnBundleComplete;
        public SaveEvent OnUnbundleComplete;


        private void Awake()
        {
            Instance = this;
            OnSaveComplete += async () => await UpdateSelectedSlotMetadata();
            Initialize();
            CreateSlots();
        }


        private void Initialize()
        {
            _slot = new List<SaveSlot>();
        }


        private void CreateSlots()
        {

            for (int i = 0; i < _slotCount; i++)
            {
                SaveSlot slot = new SaveSlot($"Save Slot {i}");

                string slotPath = Path.Combine(SavePath, slot.Name);
                if (!Directory.Exists(slotPath))
                {
                    Directory.CreateDirectory(slotPath);
                }

                _slot.Add(slot);
            }
        }

        /// <summary>
        /// Returns the list of slot metadata. You can use this to show UI
        /// </summary>
        /// <returns>List<SaveSlot></returns>
        public async UniTask<List<SaveSlot>> GetSaveSlotMetaData()
        {
            List<SaveSlot> tempList = new List<SaveSlot>();
            foreach (SaveSlot slot in _slot)
            {
                string slotPath = Path.Combine(SavePath, slot.Name);
                SaveProcessor saveSlotMetaData = new SaveProcessor($"{slot.Name}", slotPath, ".m23",
                    true,
                    "1234567891234567",
                    "0234567891234567");

                SaveSlot tempSlot = await saveSlotMetaData.Load<SaveSlot>();
                tempList.Add(tempSlot);
            }

            return tempList;

        }

        /// <summary>
        /// You can update selected slot metadata description from here.
        /// </summary>
        /// <param name="description">Optional Parameter</param>
        /// <returns></returns>
        public async UniTask UpdateSelectedSlotMetadata(string description = "")
        {
            SelectedSlot.TimeStamp = DateTime.Now;
            SelectedSlot.Description = description;
            string slotPath = Path.Combine(SavePath, SelectedSlot.Name);
            SaveProcessor saveSlotMetaData = new SaveProcessor($"{SelectedSlot.Name}", slotPath, ".m23",
                true,
                "1234567891234567",
                "0234567891234567");

            await saveSlotMetaData.Save(SelectedSlot);
        }

        /// <summary>
        /// Delete all saved files
        /// </summary>
        /// <returns>UniTask</returns>
        public async UniTask ClearSlotsAsync()
        {

            for (int i = 0; i < _slotCount; i++)
            {
                string filepath = Path.Combine(SavePath, _slot[i].Name);
                await UniTask.RunOnThreadPool(() => Directory.Delete(filepath, true));
            }
            CreateSlots();
        }

        public void SelectSlot(int index)
        {
            _selectedSlotIndex = index;
        }


        /// <summary>
        /// Use this method to save files from other systems. Essentially create new folders as you see fit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Data of any type. It will automatically be parsed to JSON</param>
        /// <param name="saveFileName">Name of the file. It should be unique</param>
        /// <param name="savefilePath">Path of the directory where the file should be saved</param>
        /// <param name="extention">Give your file an extetion for filter</param>
        /// <param name="enableEncryption">Optional Encryption Enable</param>
        /// <param name="encryptionKey">Optional 16 byte key</param>
        /// <param name="encryptionIV">Optional 16 byte key</param>
        /// <returns>Unitask Use this to impliment progress</returns>
        public async UniTask SaveData<T>(T data, 
            string saveFileName, 
            string savefilePath, 
            string extention,
            bool enableEncryption = false, 
            string encryptionKey = "1234567812345678",
            string encryptionIV = "8765432156785432" )
        {
            SaveProcessor saveManager = new SaveProcessor(saveFileName, 
                savefilePath,
                extention,
                enableEncryption: enableEncryption,
                encryptionKey: encryptionKey,
                encryptionIV: encryptionIV);

            await saveManager.Save(data);
        }


        /// <summary>
        /// Use this method to load external files that was saved by the save method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="saveFileName">Name of the file.Make sure you used the same key when saving</param>
        /// <param name="savefilePath">Folder path, Make sure this was used when saving</param>
        /// <param name="extention">Extention parameter for filtering</param>
        /// <param name="enableEncryption">Optional Encryption Enable</param>
        /// <param name="encryptionKey">Optional 16 byte key</param>
        /// <param name="encryptionIV">Optional 16 byte key</param>
        /// <returns>Unitask Use this to impliment progress</returns>
        public async UniTask<T> LoadData<T>(
            string saveFileName,
            string savefilePath,
            string extention,
            bool enableEncryption = false,
            string encryptionKey = "1234567812345678",
            string encryptionIV = "8765432156785432" )
        {
            SaveProcessor saveManager = new SaveProcessor(saveFileName,
                savefilePath, 
                extention,
                enableEncryption: enableEncryption,
                encryptionKey: encryptionKey,
                encryptionIV: encryptionIV);

            return await saveManager.Load<T>();
        }


        /// <summary>
        /// Finds all Components that implements ISavable interface and saves them. 
        /// You should implement a progress feedback as it will take some time.
        /// </summary>
        /// <returns>UniTask</returns>
        public async UniTask SaveAllSavable()
        {
            IEnumerable<ISaveable> savableComponents = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>();

            foreach (var savableComponent in savableComponents)
            {
                string uniqueID = savableComponent.UniqueID;
                string data = savableComponent.GetSerializedData();

                SaveProcessor saveManager = new SaveProcessor(uniqueID, SelectedSlotPath,
                enableEncryption: _enableEncryption,
                encryptionKey: _encryptionKey,
                encryptionIV: _encryptionIV);

                await saveManager.Save(data);

            }

            OnSaveComplete?.Invoke();
        }


        /// <summary>
        /// Finds all Components that implements ISavable interface and loads data if it was saved before.
        /// You should implement a progress feedback as it will take some time.
        /// </summary>
        /// <returns></returns>
        public async UniTask LoadAllSavable()
        {
            IEnumerable<ISaveable> savableComponents = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>();

            foreach (var savableComponent in savableComponents)
            {
                string uniqueID = savableComponent.UniqueID;

                SaveProcessor saveManager = new SaveProcessor(uniqueID, SelectedSlotPath,
                enableEncryption: _enableEncryption,
                encryptionKey: _encryptionKey,
                encryptionIV: _encryptionIV);

                string data = await saveManager.Load();

                savableComponent.AssignSerializedData(data);

            }

            OnLoadComplete?.Invoke();
        }


        /// <summary>
        /// Serializes all the files into one file for cloud save providers to work with
        /// </summary>
        /// <param name="bundleName">Name of your file(Optional)</param>
        /// <returns>UniTask</returns>
        public async UniTask BundleSaveFiles()
        {
            
            Stitcher stitcher = new Stitcher();
            await stitcher.ArchiveFiles(CloudSavePath, SavePath);

            OnBundleComplete?.Invoke();

        }

        /// <summary>
        /// Extracts the bundled file into it's original state.
        /// </summary>
        /// <param name="bundleName">Name of your file(Optional)</param>
        /// <returns>UniTask</returns>
        public async UniTask UnBundleSaveFiles()
        {
            await ClearSlotsAsync();
            Stitcher stitcher = new Stitcher();
            await stitcher.ExtractFiles(CloudSavePath, SavePath);

            OnUnbundleComplete?.Invoke();
        }



    }
}