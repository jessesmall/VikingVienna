using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour {

    public HealthManager.PlayerStatus Status;
    public float Duration;


    public void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponentInParent<HealthManager>();
        if (player != null)
        {
            if(player.playerStatus != Status)
                player.ChangePlayerStatus(Status, Duration);
        }
    }
}
