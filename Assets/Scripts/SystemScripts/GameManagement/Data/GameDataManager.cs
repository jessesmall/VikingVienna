using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour {

    public bool GameDataExists;

    private PlayerData playerData;
    private int currentLevel;

    private string saveFileName = "savedData.json";

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadGameData();
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void SubmitNewPlayerData(PlayerData pData)
    {
        playerData.MaxHealth = pData.MaxHealth;
        playerData.MaxMana = pData.MaxMana;
        playerData.Weapon = pData.Weapon;
        playerData.CanUseMagic = pData.CanUseMagic;
        playerData.CanUseFire = pData.CanUseFire;
        playerData.CanUseIce = pData.CanUseIce;
        playerData.CanUseThunder = pData.CanUseThunder;
    }

    public void SubmitNewCurrentLevel(int level)
    {
        currentLevel = level;
    }

    private void LoadGameData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, saveFileName);
        Debug.Log(filePath);
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

            playerData = loadedData.playerData;
            currentLevel = loadedData.currentLevel;
            GameDataExists = true;
        }
        else
        {
            playerData = new PlayerData();
            GameDataExists = false;
        }
    }

    public void SaveGameData()
    {
        var gameData = new GameData
        {
            playerData = playerData,
            currentLevel = currentLevel
        };
        string dataAsJson = JsonUtility.ToJson(gameData);

        string filePath = Path.Combine(Application.persistentDataPath, saveFileName);
        File.WriteAllText(filePath, dataAsJson);
    }

}
