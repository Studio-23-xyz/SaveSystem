using UnityEngine;

namespace Studio23.SS2.SaveSystem.Data
{
    [CreateAssetMenu(fileName = "SlotConfig", menuName = "Studio-23/SaveSystem/Config", order = 1)]
    public class SlotConfiguration : ScriptableObject
    {
        public bool EnableBackups = true;
        public string SaveFileExtention = ".taz";


        public string SaveRootFolderName = "SaveData";
        public int SlotCount = 10;
        public string SlotDataBackupFileName = "Data.bak";


        public string SlotDatafolderName = "Data";
        public string SlotMetafileName = "data.meta";
    }
}