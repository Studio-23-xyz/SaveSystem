
using System;

namespace Studio23.SS2.SaveSystem.Data
{
    public class SaveSlot
    {
        public int Id;
        public string Name;
        public string Description;
        public DateTime TimeStamp;

        public SaveSlot(string name)
        {
            Name = name;
            TimeStamp = DateTime.Now;
        }

    }

}
