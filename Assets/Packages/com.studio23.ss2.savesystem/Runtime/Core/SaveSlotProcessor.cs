using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Cysharp.Threading.Tasks;
using Studio23.SS2.CloudSave.Core;
using Studio23.SS2.SaveSystem.Data;
using Studio23.SS2.SaveSystem.Interfaces;
using Studio23.SS2.SaveSystem.Utilities;
using UnityEngine;

[assembly: InternalsVisibleTo("com.studio23.ss2.savesystem.editor")]

namespace Studio23.SS2.SaveSystem.Core
{
    [RequireComponent(typeof(FileProcessor))]
    internal class SaveSlotProcessor : MonoBehaviour
    {
        public static readonly string LastSelectedSaveSlotPrefsKey = "lasSelectedSlotIndex";
        [SerializeField] internal ArchiverBase _archiverBase;

        [SerializeField] internal CloudSaveManager _cloudSaveManager;
        [SerializeField] internal FileProcessor _fileProcessor;

        [SerializeField] internal SaveSlot _selectedSlot;
        [SerializeField] internal SlotConfiguration _slotConfiguration;
        [SerializeField] internal string SavePathRoot;

        internal async UniTask SelectSlot(int index)
        {
            _selectedSlot = await GetSaveSlotMetaDataLocal(index);
            PlayerPrefs.SetInt(LastSelectedSaveSlotPrefsKey, index);
        }

        internal async UniTask SelectLastSelectedSlot()
        {
            var index = PlayerPrefs.GetInt(LastSelectedSaveSlotPrefsKey, 0);
            await SelectSlot(index);
        }

        internal async UniTask LoadAllSavable()
        {
            var savableComponents = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>();

            foreach (var savableComponent in savableComponents)
            {
                var key = savableComponent.GetUniqueID();
                var filepath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration.SlotDatafolderName,
                    $"{key}{_slotConfiguration.SaveFileExtention}");
                if (!File.Exists(filepath)) throw new Exception($"{key} Not found");
                var data = await _fileProcessor.Load<string>(filepath);
                savableComponent.AssignSerializedData(data);
            }

            if (_slotConfiguration.EnableBackups)
            {
                Debug.Log("Save Integrity Validated,<color=yellow> Creating Backup </color>");
                CreateBackup().Forget();
            }
        }

        #region Setup

        internal string GetSelectedSlotPath()
        {
            return Path.Combine(SavePathRoot, _selectedSlot.Name);
        }

        internal async UniTask<SaveSlot> GetSaveSlotMetaDataLocal(int index)
        {
            var slotMeta = new SaveSlot(index);

            var slotMetaPath = Path.Combine(SavePathRoot, slotMeta.Name, _slotConfiguration.SlotMetafileName);
            if (File.Exists(slotMetaPath)) slotMeta = await _fileProcessor.Load<SaveSlot>(slotMetaPath);

            return slotMeta;
        }

        public async UniTask Initialize()
        {
            SavePathRoot = Path.Combine(Application.persistentDataPath, _slotConfiguration.SaveRootFolderName);
            _fileProcessor = GetComponent<FileProcessor>();
            CreateSlotFolders();
            await _cloudSaveManager.Initialize();
            await GetAllCloudMeta();
        }

        internal void CreateSlotFolders()
        {
            for (var i = 0; i < _slotConfiguration.SlotCount; i++)
            {
                var slot = new SaveSlot(i);

                var slotPath = Path.Combine(SavePathRoot, slot.Name, _slotConfiguration.SlotDatafolderName);
                if (Directory.Exists(slotPath)) continue;
                Directory.CreateDirectory(slotPath);
            }
        }

        #endregion

        #region Delete

        internal async UniTask ClearAllSlotsAsync()
        {
            for (var i = 0; i < _slotConfiguration.SlotCount; i++)
            {
                var filepath = Path.Combine(SavePathRoot, $"Save_Slot_{i}");
                if (Directory.Exists(filepath)) await UniTask.RunOnThreadPool(() => Directory.Delete(filepath, true));
            }

            CreateSlotFolders();
        }

        internal async UniTask ClearSelectedSlotAsync()
        {
            var filepath = GetSelectedSlotPath();
            await UniTask.RunOnThreadPool(() => Directory.Delete(filepath, true));
            CreateSlotFolders();
        }

        internal async UniTask ClearSelectedSlotCloudAsync()
        {
            await _cloudSaveManager.DeleteContainerFromCloud(_selectedSlot.Name);
        }


#if UNITY_EDITOR
        /// <summary>
        ///     Should only be used for debug purposes. This does not update meta file keys
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal async UniTask DeleteKeyFromSelectedSlot(string key)
        {
            var filepath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration.SlotDatafolderName,
                $"{key}{_slotConfiguration.SaveFileExtention}");
            await UniTask.RunOnThreadPool(() => File.Delete(filepath));
        }
#endif

        #endregion

        #region Save

        internal async UniTask SaveAllSavable(bool dirtyOnly)
        {
            var savableComponents = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>();
            if (dirtyOnly) savableComponents = savableComponents.Where(r => r.IsDirty);
            await SaveISavables(savableComponents);
        }

        private async UniTask SaveISavables(IEnumerable<ISaveable> savableComponents)
        {
            foreach (var savableComponent in savableComponents)
            {
                var key = savableComponent.GetUniqueID();
                var data = savableComponent.GetSerializedData();

                var filepath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration.SlotDatafolderName,
                    $"{key}{_slotConfiguration.SaveFileExtention}");

                await _fileProcessor.Save(data, filepath);

                _cloudSaveManager.UploadToCloud(_selectedSlot.Name, $"{key}", filepath).Forget();

                _selectedSlot.FileKeys[$"{key}"] = Encoding.Unicode.GetByteCount(data);

                savableComponent.IsDirty = false;
            }

            await SaveSelectedSlotMetadata();
        }

        private async UniTask SaveSelectedSlotMetadata()
        {
            _selectedSlot.TimeStamp = DateTime.UtcNow;
            var slotPath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration.SlotMetafileName);
            await _fileProcessor.Save(_selectedSlot, slotPath);
            _cloudSaveManager.UploadToCloud(_selectedSlot.Name, _slotConfiguration.SlotMetafileName, slotPath).Forget();
        }

        #endregion

        #region Cloud Methods

        internal async UniTask GetAllCloudMeta()
        {
            var cloudMetaDownloadTaskList = new List<UniTask>();

            for (var i = 0; i < _slotConfiguration.SlotCount; i++)
            {
                var slotMeta = new SaveSlot(i);
                var slotMetaPath = Path.Combine(SavePathRoot, slotMeta.Name, _slotConfiguration.SlotMetafileName);
                var metafileTask = _cloudSaveManager.DownloadFromCloud(slotMeta.Name,
                    _slotConfiguration.SlotMetafileName, slotMetaPath);

                cloudMetaDownloadTaskList.Add(metafileTask);
            }

            await UniTask.WhenAll(cloudMetaDownloadTaskList);
        }

        internal async UniTask SyncSelectedSlotData()
        {
            var datafileDownloadTaskList = new List<UniTask>();

            foreach (var key in _selectedSlot.FileKeys.Keys)
            {
                var filePath = Path.Combine(SavePathRoot, _selectedSlot.Name, _slotConfiguration.SlotDatafolderName,
                    $"{key}{_slotConfiguration.SaveFileExtention}");
                var fileTask = _cloudSaveManager.DownloadFromCloud(_selectedSlot.Name, key, filePath);
                datafileDownloadTaskList.Add(fileTask);
            }

            await UniTask.WhenAll(datafileDownloadTaskList);
        }

        internal async UniTask CreateBackup()
        {
            var dataFolderPath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration.SlotDatafolderName);
            var backupFilePath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration.SlotDataBackupFileName);


            await _archiverBase.ArchiveFiles(backupFilePath, dataFolderPath);
            Debug.Log($"<color=green>Backup</color> Created at {backupFilePath}");
            _cloudSaveManager
                .UploadToCloud(_selectedSlot.Name, $"{_slotConfiguration.SlotDataBackupFileName}", backupFilePath)
                .Forget();

            _selectedSlot.HasBackup = true;
            _selectedSlot.BackupStamp = DateTime.UtcNow;

            await SaveSelectedSlotMetadata();
        }

        internal async UniTask RestoreBackup()
        {
            var dataFolderPath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration.SlotDatafolderName);
            var backupFilePath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration.SlotDataBackupFileName);

            if (!File.Exists(backupFilePath))
            {
                Debug.Log("Backup File not found, Attempting to restore from cloud");
                await _cloudSaveManager.DownloadFromCloud(_selectedSlot.Name,
                    $"{_slotConfiguration.SlotDataBackupFileName}", backupFilePath);
            }

            if (!File.Exists(backupFilePath))
            {
                Debug.Log("Backup file doesn't exist on cloud either! Cry.");
                throw new Exception("Cloud Save doesn't exist, Provide proper UI");
            }


            await _archiverBase.ExtractFiles(backupFilePath, dataFolderPath);
        }

        #endregion
    }
}