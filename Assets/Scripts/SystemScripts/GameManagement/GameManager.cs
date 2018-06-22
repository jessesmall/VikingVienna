using UnityEngine;
using System.Collections;

public class GameManager{

    private static GameManager instance;
    public static GameManager Instance { get { return instance ?? (instance = new GameManager()); } }

    private GameManager()
    {

    }



}
