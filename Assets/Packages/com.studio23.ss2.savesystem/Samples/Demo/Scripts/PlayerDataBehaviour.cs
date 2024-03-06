using Studio23.SS2.SaveSystem.Interfaces;
using UnityEngine;

public class PlayerDataBehaviour : MonoBehaviour, ISaveable
{
    public string _uniqueID;
    public PlayerData playerData;

    public bool IsDirty { get; set; }

    public string GetUniqueID()
    {
        return _uniqueID;
    }

    public void AssignSerializedData(string data)
    {
        playerData = JsonUtility.FromJson<PlayerData>(data);
    }

    public string GetSerializedData()
    {
        return JsonUtility.ToJson(playerData);
    }
}