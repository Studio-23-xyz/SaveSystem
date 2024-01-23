using Cysharp.Threading.Tasks;
using Studio23.SS2.CloudSave.Core;
using Studio23.SS2.SaveSystem.Data;
using Studio23.SS2.SaveSystem.Interfaces;
using Studio23.SS2.SaveSystem.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

[assembly: InternalsVisibleTo("com.studio23.ss2.savesystem.editor")]
namespace Studio23.SS2.SaveSystem.Core
{

    [RequireComponent(typeof(FileProcessor))]
    internal class SaveSlotProcessor : MonoBehaviour
    {
        public static readonly string LastSelectedSaveSlotPrefsKey = "lasSelectedSlotIndex";

        [SerializeField] internal SaveSlot _selectedSlot;

        [SerializeField] internal CloudSaveManager _cloudSaveManager;
        [SerializeField] internal FileProcessor _fileProcessor;
        [SerializeField] internal ArchiverBase _archiverBase;
        [SerializeField] internal SlotConfiguration _slotConfiguration;

        public async UniTask Initialize()
        {
            _fileProcessor = GetComponent<FileProcessor>();
            CreateSlotFolders();
            await _cloudSaveManager.Initialize();
            await GetAllCloudMeta();
        }

        internal async UniTask SelectSlot(int index)
        {
            _selectedSlot= await GetSaveSlotMetaDataLocal(index);
            PlayerPrefs.SetInt(LastSelectedSaveSlotPrefsKey, index);
        }

        internal async UniTask SelectLastSelectedSlot()
        {
            int index=PlayerPrefs.GetInt(LastSelectedSaveSlotPrefsKey,0);
            await SelectSlot(index);
        }

        internal string GetSelectedSlotPath()
        {
            return Path.Combine(_slotConfiguration.SavePathRoot, _selectedSlot.Name);
        }

        internal void CreateSlotFolders()
        {

            for (int i = 0; i < _slotConfiguration.SlotCount; i++)
            {
                SaveSlot slot = new SaveSlot(i);

                string slotPath = Path.Combine(_slotConfiguration.SavePathRoot, slot.Name,_slotConfiguration.SlotDatafolderName);
                if (Directory.Exists(slotPath))
                {
                    continue;
                }
                Directory.CreateDirectory(slotPath);

            }
        }

        internal async UniTask<SaveSlot> GetSaveSlotMetaDataLocal(int index)
        {
            SaveSlot slotMeta=new SaveSlot(index);

            string slotMetaPath = Path.Combine(_slotConfiguration.SavePathRoot, slotMeta.Name, _slotConfiguration.SlotMetafileName);
            if (File.Exists(slotMetaPath))
            {
                slotMeta = await _fileProcessor.Load<SaveSlot>(slotMetaPath);
            }

            return slotMeta;

        }

        internal async UniTask GetAllCloudMeta()
        {
            List<UniTask> cloudMetaDownloadTaskList = new List<UniTask>();

            for(int i=0;i< _slotConfiguration.SlotCount; i++)
            {
                SaveSlot slotMeta = new SaveSlot(i);
                string slotMetaPath = Path.Combine(_slotConfiguration.SavePathRoot, slotMeta.Name, _slotConfiguration.SlotMetafileName);
                UniTask metafileTask = _cloudSaveManager.DownloadFromCloud(slotMeta.Name, _slotConfiguration.SlotMetafileName, slotMetaPath);
              
                cloudMetaDownloadTaskList.Add(metafileTask);
            }

            await UniTask.WhenAll(cloudMetaDownloadTaskList);
        }

        internal async UniTask SyncSelectedSlotData()
        {
            List<UniTask> datafileDownloadTaskList=new List<UniTask>();

            foreach(string key in _selectedSlot.FileKeys.Keys)
            {
                string filePath = Path.Combine(_slotConfiguration.SavePathRoot, _selectedSlot.Name, _slotConfiguration.SlotDatafolderName,$"{key}{_slotConfiguration.SaveFileExtention}");
                UniTask fileTask = _cloudSaveManager.DownloadFromCloud(_selectedSlot.Name, key, filePath);
                datafileDownloadTaskList.Add(fileTask);
            }

            await UniTask.WhenAll(datafileDownloadTaskList);
        }


        private async UniTask SaveSelectedSlotMetadata()
        {
            _selectedSlot.TimeStamp = DateTime.Now;
            string slotPath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration.SlotMetafileName);
            await _fileProcessor.Save(_selectedSlot, slotPath);
            _cloudSaveManager.UploadToCloud(_selectedSlot.Name, _slotConfiguration.SlotMetafileName, slotPath).Forget();
        }

        internal async UniTask ClearAllSlotsAsync()
        {

            for (int i = 0; i < _slotConfiguration.SlotCount; i++)
            {
                string filepath = Path.Combine(_slotConfiguration.SavePathRoot, $"Save_Slot_{i}");
                if(Directory.Exists(filepath))
                {
                    await UniTask.RunOnThreadPool(() => Directory.Delete(filepath, true));
                }
              
            }
            CreateSlotFolders();
        }

        internal async UniTask ClearSelectedSlotAsync()
        {
            string filepath = GetSelectedSlotPath();
            await UniTask.RunOnThreadPool(() => Directory.Delete(filepath, true));
            CreateSlotFolders();
        }

        internal async UniTask SaveAllSavable(bool dirtyOnly)
        {
            IEnumerable<ISaveable> savableComponents = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>();
            if (dirtyOnly)
            {
                savableComponents = savableComponents.Where(r => r.IsDirty);
            }
            await SaveISavables(savableComponents);

        
        }

        private async UniTask SaveISavables(IEnumerable<ISaveable> savableComponents)
        {

            foreach (var savableComponent in savableComponents)
            {
                string key = savableComponent.GetUniqueID();
                string data = savableComponent.GetSerializedData();

                string filepath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration.SlotDatafolderName, $"{key}{_slotConfiguration.SaveFileExtention}");

                await _fileProcessor.Save(data, filepath);

                _cloudSaveManager.UploadToCloud(_selectedSlot.Name, $"{key}{_slotConfiguration.SaveFileExtention}", filepath).Forget();

                _selectedSlot.FileKeys[$"{key}{_slotConfiguration.SaveFileExtention}"]= Encoding.Unicode.GetByteCount(data);

                savableComponent.IsDirty = false;
            }

            await SaveSelectedSlotMetadata();
        }

        internal async UniTask LoadAllSavable()
        {
            IEnumerable<ISaveable> savableComponents = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>();

            foreach (var savableComponent in savableComponents)
            {
                string key = savableComponent.GetUniqueID();
                string filepath = Path.Combine(GetSelectedSlotPath(),_slotConfiguration.SlotDatafolderName, $"{key}{_slotConfiguration.SaveFileExtention}");
                if (!File.Exists(filepath))
                {
                    throw new Exception($"{key} Not found");
                }
                string data = await _fileProcessor.Load<string>(filepath);
                savableComponent.AssignSerializedData(data);
            }

            if (_slotConfiguration.EnableBackups)
            {
                Debug.Log($"Save Integrity Validated,<color=yellow> Creating Backup </color>");
                CreateBackup().Forget();  
            }

        }

        internal async UniTask CreateBackup()
        {
            string dataFolderPath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration.SlotDatafolderName);
            string backupFilePath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration.SlotDataBackupFileName);
           

            await _archiverBase.ArchiveFiles(backupFilePath, dataFolderPath);
            Debug.Log($"<color=green>Backup</color> Created at {backupFilePath}");
            _cloudSaveManager.UploadToCloud(_selectedSlot.Name, $"{_slotConfiguration.SlotDataBackupFileName}", backupFilePath).Forget();

            _selectedSlot.HasBackup = true;
            _selectedSlot.BackupStamp = DateTime.Now;

            await SaveSelectedSlotMetadata();

        }



        internal async UniTask RestoreBackup()
        {
            string dataFolderPath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration.SlotDatafolderName);
            string backupFilePath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration.SlotDataBackupFileName);

            if(!File.Exists(backupFilePath))
            {
                Debug.Log("Backup File not found, Attempting to restore from cloud");
                await _cloudSaveManager.DownloadFromCloud(_selectedSlot.Name, $"{_slotConfiguration.SlotDataBackupFileName}", backupFilePath);
            }

            if (!File.Exists(backupFilePath))
            {
                Debug.Log("Backup file doesn't exist on cloud either! Cry.");
                throw new Exception("Cloud Save doesn't exist, Provide proper UI");
            }

            await _archiverBase.ExtractFiles(backupFilePath, dataFolderPath,true);

        }





    }
}