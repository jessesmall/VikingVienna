using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour {

    public Button ContinueButton;
    private GameDataManager gameDataManager;
    public int currentLevel;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        gameDataManager = FindObjectOfType<GameDataManager>();
        if (gameDataManager.GameDataExists)
        {
            currentLevel = gameDataManager.GetCurrentLevel();
        }
        else
        {
            ContinueButton.gameObject.SetActive(false);
        }
    }
}
