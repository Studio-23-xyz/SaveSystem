namespace Studio23.SS2.SaveSystem.Interfaces
{
    public interface ISaveable
    {
        string GenerateUniqueID(string name);
        void SaveData();
        void LoadData();
    }
}