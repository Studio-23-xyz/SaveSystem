using Cysharp.Threading.Tasks;
using Studio23.SS2.SaveSystem.Data;
using Studio23.SS2.SaveSystem.Interfaces;
using Studio23.SS2.SaveSystem.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor.Graphs;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Core
{

    [RequireComponent(typeof(FileProcessor))]
    internal class SaveSlotProcessor : MonoBehaviour
    {

        private SaveSlot _selectedSlot;

        [SerializeField] internal SlotConfiguration _slotConfiguration;
        [SerializeField] internal FileProcessor _fileProcessor;
        [SerializeField] internal ArchiverBase archiverBase;

        private async void Start()
        {
            _fileProcessor = GetComponent<FileProcessor>();
            CreateSlotFolders();
            await SelectSlot(0);
        }

        internal async Task SelectSlot(int index)
        {
            _selectedSlot= await GetSaveSlotMetaData(index);
        }

        internal string GetSelectedSlotPath()
        {
            return Path.Combine(_slotConfiguration.SavePathRoot, _selectedSlot.Name);
        }

        internal void CreateSlotFolders()
        {

            for (int i = 0; i < _slotConfiguration._slotCount; i++)
            {
                SaveSlot slot = new SaveSlot($"Save Slot {i}");

                string slotPath = Path.Combine(_slotConfiguration.SavePathRoot, slot.Name,_slotConfiguration._slotdatafolderName);
                if (Directory.Exists(slotPath))
                {
                    continue;
                }
                Directory.CreateDirectory(slotPath);

            }
        }

        internal async UniTask<SaveSlot> GetSaveSlotMetaData(int index)
        {
            SaveSlot slotMeta=new SaveSlot($"Save Slot {index}");

            string slotMetaPath = Path.Combine(_slotConfiguration.SavePathRoot, $"Save Slot {index}", _slotConfiguration._slotMetafileName);
            if (File.Exists(slotMetaPath))
            {
                slotMeta = await _fileProcessor.Load<SaveSlot>(slotMetaPath);
            }

            return slotMeta;

        }

        private async UniTask SaveSelectedSlotMetadata()
        {
            _selectedSlot.TimeStamp = DateTime.Now;
            string slotPath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration._slotMetafileName);
            await _fileProcessor.Save(_selectedSlot, slotPath);
        }


        internal async UniTask ClearAllSlotsAsync()
        {

            for (int i = 0; i < _slotConfiguration._slotCount; i++)
            {
                string filepath = Path.Combine(_slotConfiguration.SavePathRoot, $"Save Slot {i}");
                await UniTask.RunOnThreadPool(() => Directory.Delete(filepath, true));
            }
            CreateSlotFolders();
        }


        internal async UniTask ClearSelectedSlotAsync()
        {
            string filepath = GetSelectedSlotPath();
            await UniTask.RunOnThreadPool(() => Directory.Delete(filepath, true));
            CreateSlotFolders();
        }



        internal async UniTask SaveAllSavable()
        {
            IEnumerable<ISaveable> savableComponents = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>();


            _selectedSlot.fileKeys.Clear();
            foreach (var savableComponent in savableComponents)
            {
                string key = savableComponent.UniqueID;
                string data = savableComponent.GetSerializedData();

                string filepath = Path.Combine(GetSelectedSlotPath(),_slotConfiguration._slotdatafolderName, $"{key}{_slotConfiguration.saveFileExtention}");

                await _fileProcessor.Save(data, filepath);
                _selectedSlot.fileKeys.Add($"{key}{_slotConfiguration.saveFileExtention}");
            }

            await SaveSelectedSlotMetadata();
        }


        internal async UniTask LoadAllSavable()
        {
            IEnumerable<ISaveable> savableComponents = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>();

            foreach (var savableComponent in savableComponents)
            {
                string key = savableComponent.UniqueID;
                string filepath = Path.Combine(GetSelectedSlotPath(),_slotConfiguration._slotdatafolderName, $"{key}{_slotConfiguration.saveFileExtention}");
                if (!File.Exists(filepath))
                {
                    throw new Exception($"{key} Not found");
                }
                string data = await _fileProcessor.Load<string>(filepath);
                savableComponent.AssignSerializedData(data);
            }

            if (_slotConfiguration._enableBackups)
            {
                _ = CreateBackup(); //Don't wait for backup being created  
            }

        }


        internal async UniTask CreateBackup()
        {
            string dataFolderPath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration._slotdatafolderName);
            string backupFilePath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration._slotDataBackupFileName);
           

            await archiverBase.ArchiveFiles(backupFilePath, dataFolderPath);

            _selectedSlot.HasBackup = true;
            _selectedSlot.BackupStamp = DateTime.Now;

            await SaveSelectedSlotMetadata();

        }

        internal async UniTask RestoreBackup()
        {
            string dataFolderPath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration._slotdatafolderName);
            string backupFilePath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration._slotDataBackupFileName);

            await archiverBase.ExtractFiles(backupFilePath, dataFolderPath,true);

        }





    }
}