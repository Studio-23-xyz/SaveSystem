using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SlotConfig", menuName = "Studio-23/SaveSystem/Config", order = 1)]
public class SlotConfiguration : ScriptableObject
{
    [SerializeField] public int _slotCount = 10;

    [SerializeField] public bool _enableBackups = true;


    public string _slotDatafolderName = "Data";
    public string _slotDataBackupFileName = "Data.bak";
    public string _slotMetafileName = "data.meta";

    public string saveFileExtention = ".taz";


    public string _saveRootFolderName = "SaveData";

    public string SavePathRoot => Path.Combine(Application.persistentDataPath, _saveRootFolderName);

}
