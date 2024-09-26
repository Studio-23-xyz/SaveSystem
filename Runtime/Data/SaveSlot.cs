using System;
using System.Collections.Generic;
using System.Linq;


namespace Studio23.SS2.SaveSystem.Data
{
    [Serializable]
    public class SaveSlot
    {
        public string Seed;
        public DateTime BackupStamp;
        public string Description;

        public Dictionary<string, int> FileKeys;

        public bool HasBackup;
        public int Index;


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