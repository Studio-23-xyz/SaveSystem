using System;
using Cysharp.Threading.Tasks;
using Studio23.SS2.SaveSystem.Interfaces;
using UnityEngine;

public class ItemDataBehaviour : MonoBehaviour, ISaveable
{
    [SerializeField] private bool _isDirty;

    [SerializeField] private string _uniqueID;

    public ItemData itemData;

    public bool IsDirty
    {
        get => _isDirty;
        set => _isDirty = value;
    }


    public string GetUniqueID()
    {
        return _uniqueID;
    }

    public UniTask AssignSerializedData(string data)
    {
        itemData = JsonUtility.FromJson<ItemData>(data);
        return UniTask.CompletedTask;
    }

    public UniTask ManageSaveLoadException(Exception exception)
    {
        throw new NotImplementedException();
    }

    public UniTask<string> GetSerializedData()
    {
        string data= JsonUtility.ToJson(itemData);
        return new UniTask<string>(data);
    }
}