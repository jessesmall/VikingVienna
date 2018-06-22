using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public PlayerData playerData;
    public int currentLevel;

    public GameData()
    {
        playerData = new PlayerData();
    }
}
