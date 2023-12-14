using UnityEngine;

public abstract class AbstractCloudSaveProvider : MonoBehaviour
{
    public delegate void CloudSaveEvent();
    public CloudSaveEvent OnUploadSuccess;
    public CloudSaveEvent OnDownloadSuccess;
    public abstract void UploadToCloud(string key,string filepath);
    public abstract void DownloadFromCloud(string key,string downloadLocation);
}
