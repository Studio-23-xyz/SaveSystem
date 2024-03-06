using System;
using System.Collections.Generic;
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
        public Texture Thumbnail;

        public DateTime TimeStamp;

        public SaveSlot(int index)
        {
            Index = index;
            TimeStamp = DateTime.UtcNow;
            FileKeys = new Dictionary<string, int>();
        }

        public string Name => $"Save_Slot_{Index}";
    }
}