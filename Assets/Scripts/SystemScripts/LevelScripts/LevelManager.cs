using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using Assets.Scripts.Player;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance { get; private set; }

    public PlayerController Player { get; private set; }
    public CameraController Camera { get; private set; }
    public Canvas endGame;

    public GameObject RespawnEffect;


    private List<CheckPoint> checkPoints;
    private int currentCheckPointIndex;
    private DateTime started;
    

    public CheckPoint debugSpawn;

    public bool DisableScripts;

    public Animator UIAnimator;

    public int NextLevel;

    public bool BossFight;

    public GameEvent OnPlayerDied;
    public GameEvent OnPlayerRespawned;
    public GameEvent OnLevelComplete;

    public void Awake()
    {
        if (DisableScripts)
        {
            foreach(var textBox in this.GetComponents<TextBoxManager>())
            {
                textBox.enabled = false;
            }
        }
        Instance = this;
        var time = DateTime.Now;
    }

    public void Start()
    {
        checkPoints = FindObjectsOfType<CheckPoint>().OrderBy(t => t.transform.position.x).ToList();
        currentCheckPointIndex = checkPoints.Count > 0 ? 0 : -1;

        // Check saved data
        var gameData = FindObjectOfType<GameDataManager>();
        if (gameData != null && gameData.GameDataExists)
        {
            //playerStats.totalHealth = gameData.GetPlayerData().MaxHealth;
            //playerStats.totalMana = gameData.GetPlayerData().MaxMana;
        }

        Player = FindObjectOfType<PlayerController>();
        Camera = FindObjectOfType<CameraController>();


#if UNITY_EDITOR
        if (debugSpawn != null)
            debugSpawn.SpawnPlayer(Player);
        else if (currentCheckPointIndex != -1)
            checkPoints[currentCheckPointIndex].SpawnPlayer(Player);
#else
        if(currentCheckPointIndex != -1)
            checkPoints[currentCheckPointIndex].SpawnPlayer(Player);   
#endif

        var listeners = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerRespawnListener>();
        foreach (var listener in listeners)
        {
            for (var i = checkPoints.Count - 1; i >= 0; i--)
            {
                var distance = ((MonoBehaviour)listener).transform.position.x - checkPoints[i].transform.position.x;
                if (distance < 0)
                    continue;

                checkPoints[i].AssignObjectToCheckPoint(listener);
                break;
            }
        }
    }


    public void Update()
    {
        var isAtLastCheckPoint = currentCheckPointIndex + 1 >= checkPoints.Count;
        if (isAtLastCheckPoint)
            return;
        var distanceToNextCheckPoint = checkPoints[currentCheckPointIndex + 1].transform.position.x - Player.transform.position.x;
        if (distanceToNextCheckPoint > 0)
            return;

        currentCheckPointIndex++;

    }

    public void KillPlayer()
    {
        if (!Player.IsDead)
        {
            OnPlayerDied.Raise();
            StartCoroutine(KillPlayerCo());
        }
    }

    public IEnumerator KillPlayerCo()
    {
        UIAnimator.SetTrigger("DeathScreen");

        yield return new WaitForSeconds(7.5f);
        OnPlayerRespawned.Raise();
        if (BossFight)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (currentCheckPointIndex != -1)
        {
            checkPoints[currentCheckPointIndex].SpawnPlayer(Player);
            if(RespawnEffect != null)
                Instantiate(RespawnEffect, checkPoints[currentCheckPointIndex].transform.position, RespawnEffect.transform.rotation);
        }
    }

    public void EndLevel()
    {
        OnLevelComplete.Raise();
        StartCoroutine(EndofLevel());
    }

    IEnumerator EndofLevel()
    {
        UIAnimator.SetTrigger("EndLevel");
        yield return new WaitForSeconds(6f);

        GoToNextLevel(NextLevel);
    }

    public void GoToNextLevel(int levelIndex)
    {
        //var gameData = FindObjectOfType<GameDataManager>();
        //var playerAbilities = playerStats.gameObject.GetComponent<PlayerAbilities>();
        //var playerData = new PlayerData()
        //{
        //    //MaxHealth = playerStats.totalHealth,
        //    //MaxMana = playerStats.totalMana,
        //    CanUseMagic = playerAbilities.CanUseMagic,
        //    CanUseThunder = playerAbilities.CanUseThunder,
        //    CanUseFire = playerAbilities.CanUseFire,
        //    CanUseIce = playerAbilities.CanUseIce
        //};

        //if(gameData != null)
        //{
        //    gameData.SubmitNewPlayerData(playerData);
        //    gameData.SubmitNewCurrentLevel(levelIndex);
        //    gameData.SaveGameData();
        //}
        GoToNextLevelCo(levelIndex);
    }

    private void GoToNextLevelCo(int levelIndex)
    {
        endGame.GetComponentInChildren<Text>().enabled = true;
        Player.FinishLevel();

        if (levelIndex == -1)
            SceneManager.LoadScene(0);
        else
            SceneManager.LoadScene(levelIndex);
    }


}
