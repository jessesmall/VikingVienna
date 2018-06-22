using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private LevelLoader levelLoader;

    private void Start()
    {
        levelLoader = GetComponent<LevelLoader>();
    }

    public void StartNewGame()
    {
        levelLoader.LoadLevel(1);
    }

    public void ContinueGame()
    {
        levelLoader.LoadLevel(FindObjectOfType<GameStart>().currentLevel);
    }
}
