using UnityEngine;

public abstract class AbstractCloudSaveProvider : MonoBehaviour
{
    public delegate void CloudSaveEvent();
    public CloudSaveEvent OnUploadSuccess;
    public CloudSaveEvent OnDownloadSuccess;

    /// <summary>
    /// You should fire OnUploadSuccess in the implementation.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="filepath"></param>
    public abstract void UploadToCloud(string key,string filepath);

    /// <summary>
    /// You should fire OnDownloadSuccess in the implementation
    /// </summary>
    /// <param name="key"></param>
    /// <param name="downloadLocation"></param>
    public abstract void DownloadFromCloud(string key,string downloadLocation);


    



}
