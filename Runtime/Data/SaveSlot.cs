
using System;

namespace Studio23.SS2.SaveSystem.Data
{
    internal class SaveSlot
    {
        public int Id;
        public string Name;
        public string Description;
        public DateTime TimeStamp;

        public SaveSlot(string name)
        {
            TimeStamp = DateTime.Now;
            Name = name;
         
        }

    }

}
