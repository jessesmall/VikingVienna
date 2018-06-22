using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneEventTrigger : MonoBehaviour {

    public UnityEvent SceneEvent;

    private bool _sceneTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponentInParent<PlayerInput>();
        if(player != null && !_sceneTriggered)
        {
            SceneEvent.Invoke();
            _sceneTriggered = true;
        }
    }
}
