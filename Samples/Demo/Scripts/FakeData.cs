using System;

namespace Studio23.SS2.SaveSystem.Samples
{
    [Serializable]
    public class PlayerData
    {
        public int playerLevel;
        public string playerName;
    }

    [Serializable]
    public class ItemData
    {
        public string itemName;
        public int itemQuantity;
    }
}