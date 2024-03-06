using Cysharp.Threading.Tasks;

namespace Studio23.SS2.SaveSystem.Interfaces
{
    public interface ISaveable
    {
        /// <summary>
        /// Must Return an Unique ID for save system to save file with this name
        /// </summary>
        public string GetUniqueID();

        /// <summary>
        /// Save System Would Only save files that are Dirty in short has changes to submit
        /// </summary>
        public bool IsDirty { get; set; }

        /// <summary>
        /// Return a serialized string for the save system to save
        /// </summary>
        /// <returns>String Data</returns>
        public string GetSerializedData();

        /// <summary>
        /// Implement how your component will adjust on data load
        /// </summary>
        /// <param name="data">String data</param>
        public void AssignSerializedData(string data);

#if UNITY_EDITOR
        /// <summary>
        /// Debug only Method. Do not use for any other reason
        /// </summary>
        public void DeleteLocalFile()
        {
           Core.SaveSystem.Instance.DeleteKeyFromSelectedSlot(GetUniqueID()).Forget();
        }

#endif
    }
}