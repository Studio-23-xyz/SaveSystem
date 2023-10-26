using Cysharp.Threading.Tasks;
using NUnit.Framework;
using Studio23.SS2.SaveSystem.Core;
using Studio23.SS2.SaveSystem.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.TestTools;

public class SlotMetaDataTest 
{
    private SaveSystem _saveSystem;

    [UnityTest]
    [Order(0)]
    public IEnumerator SaveSystem_Setup() => UniTask.ToCoroutine(async () =>
    {
        _saveSystem = new GameObject().AddComponent<SaveSystem>();
        await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: false);
        await _saveSystem.ClearSlotsAsync();//Tear Down
        Assert.NotNull(_saveSystem);
    });


    [UnityTest]
    [Order(1)]
    public IEnumerator SaveSystem_SaveSlots() => UniTask.ToCoroutine(async () =>
    {
        _saveSystem = new GameObject().AddComponent<SaveSystem>();
        _saveSystem.SelectSlot(0);
        await _saveSystem.UpdateSelectedSlotMetadata();

        string filePath = Path.Combine(Application.persistentDataPath, "SaveData", "Save Slot 0", "Save Slot 0.m23");
        FileAssert.Exists(filePath);

        _saveSystem.SelectSlot(1);
        await _saveSystem.UpdateSelectedSlotMetadata();

        filePath = Path.Combine(Application.persistentDataPath, "SaveData", "Save Slot 1", "Save Slot 1.m23");
        FileAssert.Exists(filePath);


        _saveSystem.SelectSlot(2);
        await _saveSystem.UpdateSelectedSlotMetadata();

        filePath = Path.Combine(Application.persistentDataPath, "SaveData", "Save Slot 2", "Save Slot 2.m23");
        FileAssert.Exists(filePath);


    });


    [UnityTest]
    [Order(2)]
    public IEnumerator SaveSystem_LoadSlot() => UniTask.ToCoroutine(async () =>
    {
        
        List<SaveSlot> slotMetadata = await _saveSystem.GetSaveSlotMetaData();

        Assert.AreEqual(_saveSystem._slot[0].TimeStamp, slotMetadata[0].TimeStamp);
        Assert.AreEqual(_saveSystem._slot[1].TimeStamp, slotMetadata[1].TimeStamp);
        Assert.AreEqual(_saveSystem._slot[2].TimeStamp, slotMetadata[2].TimeStamp);

    });

    [OneTimeTearDown]
    public void TearDown()
    {
        Directory.Delete(_saveSystem.SavePath, true);
    }


}
