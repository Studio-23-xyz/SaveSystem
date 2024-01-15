using Studio23.SS2.SaveSystem.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyBehaviour : MonoBehaviour, ISaveable
{

    public string UniqueID => id;

    [SerializeField] private string id = "asdasdasd";
    public string Data;

    public void AssignSerializedData(string data)
    {
        Data = data;
    }

    public string GetSerializedData()
    {
        return Data;
    }

    
}
