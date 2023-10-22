using Cysharp.Threading.Tasks;
using Studio23.SS2.SaveSystem.Data;
using Studio23.SS2.SaveSystem.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;


[assembly: InternalsVisibleTo("PlayMode")]
[assembly: InternalsVisibleTo("com.studio23.ss2.savesystem.editor")]
namespace Studio23.SS2.SaveSystem.Core
{
    public class SaveSystem : MonoBehaviour
    {
        public static SaveSystem Instance;

        private List<SaveSlot> _slot;
        private int _selectedSlotIndex;
        private SaveSlot SelectedSlot => _slot[_selectedSlotIndex];

        [Header("Config")]
        [SerializeField] internal int _slotCount=3;
        [SerializeField] internal bool _enableEncryption;
        [SerializeField] internal string _encryptionKey;
        [SerializeField] internal string _encryptionIV;

        [SerializeField] internal string _saveRootFolderName="SaveData";

        private string SavePath => Path.Combine(Application.persistentDataPath, _saveRootFolderName);
        private string SelectedSlotPath=> Path.Combine(SavePath, SelectedSlot.Name);
        

        private async void Awake ()
        {
            Instance = this;
            await CreateSlotsAsync();
        }

        private async Task CreateSlotsAsync()
        {
            _slot = new List<SaveSlot>();
            for(int i=0; i< _slotCount; i++)
            {
                SaveSlot slot = new SaveSlot($"Save Slot {i}");

                string slotPath=Path.Combine(SavePath,slot.Name);
                if (!Directory.Exists(slotPath))
                {
                    Directory.CreateDirectory(slotPath);
                    await UpdateSlotMetadata(slot);
                }

                _slot.Add(slot);
            }
        }

        private async Task UpdateSlotMetadata(SaveSlot saveSlot)
        {
            string slotPath = Path.Combine(SavePath, saveSlot.Name);
            SaveManager saveSlotMetaData = new SaveManager($"{saveSlot.Name}", slotPath, ".m23",
                true,
                "1234567891234567",
                "0234567891234567");

            await saveSlotMetaData.Save(saveSlot);
        }

        public async Task ClearSlotsAsync()
        {
            
            for(int i= 0; i< _slotCount; i++)
            {
                string filepath = Path.Combine(SavePath, _slot[i].Name);
                await UniTask.RunOnThreadPool(()=> Directory.Delete(filepath, true));
            }
            await CreateSlotsAsync();
        }

        public void SelectSlot(int index)
        {
            _selectedSlotIndex= index;
        }

        public async UniTask SaveData<T>(T data,string id)
        {
            SaveManager saveManager=new SaveManager(id,SelectedSlotPath,
                enableEncryption:_enableEncryption,
                encryptionKey:_encryptionKey,
                encryptionIV: _encryptionIV);

            await saveManager.Save(data);

            SelectedSlot.TimeStamp = DateTime.Now;
            await UpdateSlotMetadata(SelectedSlot);
        }

        public async UniTask<T> LoadData<T>(string id)
        {
            SaveManager saveManager = new SaveManager(id, SelectedSlotPath,
                enableEncryption: _enableEncryption,
                encryptionKey: _encryptionKey,
                encryptionIV: _encryptionIV);

            return await saveManager.Load<T>();

   
        }



        public async UniTask BundleSaveFiles(string bundleName="cloudSave")
        {
            List<string> FilePaths = Directory.GetFiles(SavePath, "*.taz", SearchOption.AllDirectories).ToList();
            Stitcher stitcher = new Stitcher();
            await stitcher.ArchiveFiles($"{SavePath}/{bundleName}.sav",FilePaths);
        }

        public async UniTask UnBundleSaveFiles(string bundleName = "cloudSave")
        {
            await ClearSlotsAsync();
            Stitcher stitcher = new Stitcher();
            await stitcher.ExtractFiles($"{SavePath}/{bundleName}.sav", SavePath);
        }



    }
}