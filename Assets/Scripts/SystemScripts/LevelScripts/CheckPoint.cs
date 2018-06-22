using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckPoint : MonoBehaviour {

    private List<IPlayerRespawnListener> listeners;

    public void Awake()
    {
        listeners = new List<IPlayerRespawnListener>();
    }

    private IEnumerator PlayerHitCheckPointCo(int bonus)
    {
        yield break;
    }

    public void PlayerLeftCheckPoint()
    {

    }

    public void SpawnPlayer(PlayerController player)
    {
        player.RespawnAt(transform);

        foreach (var listener in listeners)
            listener.OnPlayerRespawnInThisCheckPoint(this, player);
    }

    public void AssignObjectToCheckPoint(IPlayerRespawnListener listener)
    {
        
        listeners.Add(listener);
    }

}
