using UnityEngine;

public abstract class AbstractCloudSaveProvider : MonoBehaviour
{
    public delegate void CloudSaveEvent();
    public CloudSaveEvent OnUploadSuccess;
    public CloudSaveEvent OnDownloadSuccess;
    protected abstract void UploadToCloud(string key,string filepath);
    protected abstract void DownloadFromCloud(string key,string downloadLocation);


    public void Upload(string key, string filepath)
    {
        UploadToCloud(key,filepath);
        OnUploadSuccess?.Invoke();
    }

    public void Download(string key, string downloadLocation)
    {
        DownloadFromCloud(key, downloadLocation);
        OnDownloadSuccess?.Invoke();
    }



}
