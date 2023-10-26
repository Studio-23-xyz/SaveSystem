using Cysharp.Threading.Tasks;
using NUnit.Framework;
using Studio23.SS2.SaveSystem.Core;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.TestTools;

public class EncryptionTest
{
    private SaveSystem _saveSystem;

    private PlayerData _playerData;
    private ItemData _itemData;


    [UnityTest]
    [Order(0)]
    public IEnumerator SaveSystem_Setup() => UniTask.ToCoroutine(async () =>
    {

        _saveSystem = new GameObject().AddComponent<SaveSystem>();

        await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: false);

        await _saveSystem.ClearSlotsAsync();//Tear Down


        _saveSystem._enableEncryption = true;
        _saveSystem._encryptionKey = "1234567887654321";
        _saveSystem._encryptionIV = "1234567887654321";


        _saveSystem.SelectSlot(0);

        _saveSystem.enabled = false;

        _playerData = new PlayerData
        {
            playerName = "Mark",
            playerLevel = 20
        };

        _itemData = new ItemData
        {
            itemName = "Item",
            itemQuantity = 5,
        };




        Assert.NotNull(_saveSystem);
        Assert.NotNull(_playerData);
        Assert.NotNull(_itemData);


    });


    [UnityTest]
    [Order(1)]
    public IEnumerator Save_Test() => UniTask.ToCoroutine(async () =>
    {

        _saveSystem.SelectSlot(0);
        await _saveSystem.SaveData(_playerData, "playerSaveData", _saveSystem.SelectedSlotPath, ".taz");
        await _saveSystem.SaveData(_itemData, "items", _saveSystem.SelectedSlotPath, ".taz");

        string filePath=Path.Combine(Application.persistentDataPath, "SaveData", "Save Slot 0", "playerSaveData.taz");
        FileAssert.Exists(filePath);

        filePath = Path.Combine(Application.persistentDataPath, "SaveData", "Save Slot 0", "items.taz");
        FileAssert.Exists(filePath);



        _saveSystem.SelectSlot(1);
        await _saveSystem.SaveData(_playerData, "playerSaveData", _saveSystem.SelectedSlotPath, ".taz");
        filePath = Path.Combine(Application.persistentDataPath, "SaveData", "Save Slot 1", "playerSaveData.taz");
        FileAssert.Exists(filePath);


    });


    [UnityTest]
    [Order(1)]
    public IEnumerator Load_Test() => UniTask.ToCoroutine(async () =>
    {
        _saveSystem.SelectSlot(0);

        PlayerData loadedPlayerData = await _saveSystem.LoadData<PlayerData>("playerSaveData", _saveSystem.SelectedSlotPath, ".taz");
        Assert.AreEqual(_playerData.playerName, loadedPlayerData.playerName);
        Assert.AreEqual(_playerData.playerLevel, loadedPlayerData.playerLevel);


        ItemData loadedItemData = await _saveSystem.LoadData<ItemData>("items", _saveSystem.SelectedSlotPath, ".taz");
        Assert.AreEqual(_itemData.itemName, loadedItemData.itemName);
        Assert.AreEqual(_itemData.itemQuantity, loadedItemData.itemQuantity);


    });



}
