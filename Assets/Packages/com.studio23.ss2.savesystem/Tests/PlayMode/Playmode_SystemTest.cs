using Cysharp.Threading.Tasks;
using NUnit.Framework;
using Studio23.SS2.SaveSystem.Core;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.TestTools;

public class Playmode_SystemTest
{
    private SaveSystem _saveSystem;

    private PlayerData _playerData;

   

    [UnityTest]
    [Order(0)]
    public IEnumerator _SaveSystem_Save_LoadTest_Basic_01() => UniTask.ToCoroutine(async () =>
    {

        _saveSystem = new GameObject().AddComponent<SaveSystem>();

        await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: false);

        await _saveSystem.ClearSlotsAsync();
        _saveSystem.SelectSlot(0);
        
        _saveSystem.enabled = false;

        _playerData = new PlayerData
        {
            playerName = "John",
            playerLevel = 10
        };


        await _saveSystem.SaveData(_playerData, "playerSaveData");

        PlayerData loadedPlayerData = await _saveSystem.LoadData<PlayerData>("playerSaveData");


        Assert.AreEqual(_playerData.playerName, loadedPlayerData.playerName);
        Assert.AreEqual(_playerData.playerLevel, loadedPlayerData.playerLevel);


    });


    [UnityTest]
    [Order(1)]
    public IEnumerator _SaveSystem_Save_LoadTest_Bundle_01() => UniTask.ToCoroutine(async () =>
    {

        await _saveSystem.BundleSaveFiles();

        await _saveSystem.UnBundleSaveFiles();

        PlayerData loadedPlayerData = await _saveSystem.LoadData<PlayerData>("playerSaveData"); 


        Assert.AreEqual(_playerData.playerName, loadedPlayerData.playerName);
        Assert.AreEqual(_playerData.playerLevel, loadedPlayerData.playerLevel);


    });


}
