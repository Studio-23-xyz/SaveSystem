using Studio23.SS2.SaveSystem.Interfaces;
using UnityEngine;


public class ItemDataBehaviour : MonoBehaviour, ISaveable
{

    public string _uniqueID;
    public ItemData itemData;

    public string GetUniqueID()
    {
        return _uniqueID;
    } 

    public void AssignSerializedData(string data)
    {
       itemData = JsonUtility.FromJson<ItemData>(data);
    }

    public string GetSerializedData()
    {
       return JsonUtility.ToJson(itemData);
    }
}
