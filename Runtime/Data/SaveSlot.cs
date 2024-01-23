
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Data
{
    [Serializable]
    public class SaveSlot
    {
        public int Index;

        public string Name => $"Save_Slot_{Index}";
        public Texture Thumbnail;
        public string Description;

        public DateTime TimeStamp;
        public DateTime BackupStamp;

        public bool HasBackup;

        public Dictionary<string,int> FileKeys;

        public SaveSlot(int index)
        {
            Index = index;
            TimeStamp = DateTime.Now;
            FileKeys = new Dictionary<string,int>();
        }

    }



}
