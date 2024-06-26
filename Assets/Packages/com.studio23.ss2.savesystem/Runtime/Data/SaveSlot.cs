using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Data
{
    [Serializable]
    public class SaveSlot
    {
        public DateTime BackupStamp;
        public string Description;

        public Dictionary<string, int> FileKeys;

        public bool HasBackup;
        public int Index;
        [JsonIgnore]
        public Texture Thumbnail;

        public DateTime TimeStamp;

        public SaveSlot()
        {
            FileKeys = new Dictionary<string, int>();
        }

        public SaveSlot(int index)
        {
            Index = index;
            TimeStamp = DateTime.UtcNow;
            FileKeys = new Dictionary<string, int>();
        }

        public string Name => $"Save_Slot_{Index}";

        public bool IsEmpty=>FileKeys.Count==0;

        public int TotalSize => FileKeys.Sum(r => r.Value);


    }
}