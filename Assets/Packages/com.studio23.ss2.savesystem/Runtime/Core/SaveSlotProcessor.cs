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

[assembly:InternalsVisibleTo("com.studio23.ss2.savesystem.editor")]
namespace Studio23.SS2.SaveSystem.Core
{

    [RequireComponent(typeof(FileProcessor))]
    internal class SaveSlotProcessor : MonoBehaviour
    {

        [SerializeField] internal SaveSlot _selectedSlot;

        [SerializeField] internal FileProcessor _fileProcessor;
        [SerializeField] internal ArchiverBase archiverBase;
        [SerializeField] internal SlotConfiguration _slotConfiguration;

        private async void Start()
        {
            _fileProcessor = GetComponent<FileProcessor>();
            CreateSlotFolders();
            await SelectSlot(0);
        }

        internal async UniTask SelectSlot(int index)
        {
            _selectedSlot= await GetSaveSlotMetaData(index);
        }

        internal string GetSelectedSlotPath()
        {
            return Path.Combine(_slotConfiguration.SavePathRoot, _selectedSlot.Name);
        }

        internal void CreateSlotFolders()
        {

            for (int i = 0; i < _slotConfiguration.SlotCount; i++)
            {
                SaveSlot slot = new SaveSlot($"Save_Slot_{i}");

                string slotPath = Path.Combine(_slotConfiguration.SavePathRoot, slot.Name,_slotConfiguration.SlotDatafolderName);
                if (Directory.Exists(slotPath))
                {
                    continue;
                }
                Directory.CreateDirectory(slotPath);

            }
        }

        internal async UniTask<SaveSlot> GetSaveSlotMetaData(int index)
        {
            SaveSlot slotMeta=new SaveSlot($"Save_Slot_{index}");

            string slotMetaPath = Path.Combine(_slotConfiguration.SavePathRoot, $"Save_Slot_{index}", _slotConfiguration.SlotMetafileName);
            if (File.Exists(slotMetaPath))
            {
                slotMeta = await _fileProcessor.Load<SaveSlot>(slotMetaPath);
            }

            return slotMeta;

        }

        private async UniTask SaveSelectedSlotMetadata()
        {
            _selectedSlot.TimeStamp = DateTime.Now;
            string slotPath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration.SlotMetafileName);
            await _fileProcessor.Save(_selectedSlot, slotPath);
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



        internal async UniTask SaveAllSavable()
        {
            IEnumerable<ISaveable> savableComponents = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>();


            _selectedSlot.fileKeys.Clear();
            foreach (var savableComponent in savableComponents)
            {
                string key = savableComponent.GetUniqueID();
                string data = savableComponent.GetSerializedData();

                string filepath = Path.Combine(GetSelectedSlotPath(),_slotConfiguration.SlotDatafolderName, $"{key}{_slotConfiguration.SaveFileExtention}");

                await _fileProcessor.Save(data, filepath);
                _selectedSlot.fileKeys.Add($"{key}{_slotConfiguration.SaveFileExtention}");
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
                _= CreateBackup();  
            }

        }


        internal async UniTask CreateBackup()
        {
            string dataFolderPath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration.SlotDatafolderName);
            string backupFilePath = Path.Combine(GetSelectedSlotPath(), _slotConfiguration.SlotDataBackupFileName);
           

            await archiverBase.ArchiveFiles(backupFilePath, dataFolderPath);

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
                Debug.LogWarning("Backup File not found");
                return;
            }
            await archiverBase.ExtractFiles(backupFilePath, dataFolderPath,true);

        }





    }
}