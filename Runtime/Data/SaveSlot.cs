
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Data
{
    [Serializable]
    public class SaveSlot
    {
        public int Id;

        public string Name { get; private set; }
        public Texture Thumbnail;
        public string Description;

        public DateTime TimeStamp;
        public DateTime BackupStamp;

        public bool HasBackup;

        public List<string> fileKeys;

        public SaveSlot(string name)
        {
            Name = name;
            TimeStamp = DateTime.Now;
            fileKeys = new List<string>();
        }

    }

}
