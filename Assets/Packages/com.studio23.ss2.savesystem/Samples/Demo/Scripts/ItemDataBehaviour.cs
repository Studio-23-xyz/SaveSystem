using Studio23.SS2.SaveSystem.Interfaces;
using UnityEngine;


public class ItemDataBehaviour : MonoBehaviour, ISaveable
{
    [SerializeField]
    private string _uniqueID;

    [SerializeField]
    private bool _isDirty;

    public bool IsDirty
    {
        get
        {
            return _isDirty;
        }
        set
        {
            _isDirty = value;
        }
    }

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
