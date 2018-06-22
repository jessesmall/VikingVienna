using UnityEngine;
using System.Collections;
using Assets.Scripts.Player;

public class FinishLevel : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponentInParent<PlayerController>();
        if (player == null)
            return;

        LevelManager.Instance.EndLevel();
    }
}
