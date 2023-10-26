using Cysharp.Threading.Tasks;
using NUnit.Framework;
using Studio23.SS2.SaveSystem.Core;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.TestTools;

public class BasicSaveLoadTests
{
    private SaveSystem _saveSystem;

    private PlayerData _playerData;

    [UnityTest]
    [Order(0)]
    public IEnumerator SaveSystem_Setup() => UniTask.ToCoroutine(async () =>
    {

        _saveSystem = new GameObject().AddComponent<SaveSystem>();

        await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: false);

        await _saveSystem.ClearSlotsAsync();//Tear Down
        _saveSystem.SelectSlot(0);

        _saveSystem.enabled = false;

        _playerData = new PlayerData
        {
            playerName = "John",
            playerLevel = 10
        };


        Assert.NotNull(_saveSystem);
        Assert.NotNull(_playerData);


    });



    [UnityTest]
    [Order(1)]
    public IEnumerator Save_Load_Test() => UniTask.ToCoroutine(async () =>
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


        await _saveSystem.SaveData(_playerData, "playerSaveData",_saveSystem.SelectedSlotPath,".taz");

        PlayerData loadedPlayerData = await _saveSystem.LoadData<PlayerData>("playerSaveData",_saveSystem.SelectedSlotPath,".taz");


        Assert.AreEqual(_playerData.playerName, loadedPlayerData.playerName);
        Assert.AreEqual(_playerData.playerLevel, loadedPlayerData.playerLevel);


    });


    [UnityTest]
    [Order(2)]
    public IEnumerator Bundle_Create_Test() => UniTask.ToCoroutine(async () =>
    {

        await _saveSystem.BundleSaveFiles();
        string bundlePath = Path.Combine(Application.persistentDataPath, "SaveData", "cloudSave.sav");
        FileAssert.Exists(bundlePath);

    });

    [UnityTest]
    [Order(3)]
    public IEnumerator Bundle_Extrat_Test() => UniTask.ToCoroutine(async () =>
    {

        await _saveSystem.UnBundleSaveFiles();
        PlayerData loadedPlayerData = await _saveSystem.LoadData<PlayerData>("playerSaveData", _saveSystem.SelectedSlotPath, ".taz");

        Assert.AreEqual(_playerData.playerName, loadedPlayerData.playerName);
        Assert.AreEqual(_playerData.playerLevel, loadedPlayerData.playerLevel);


    });

    [OneTimeTearDown]
    public void TearDown()
    {
        Directory.Delete(_saveSystem.SavePath, true);
    }


}
