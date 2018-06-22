using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class GameEventListener : MonoBehaviour
{
    public List<GameEventResponse> GameEvents;

    private void OnEnable()
    {
        foreach(var evnt in GameEvents)
        {
            evnt.Event.RegisterListener(this);
        }
    }

    private void OnDisable()
    {
        foreach (var evnt in GameEvents)
        {
            evnt.Event.UnregisterListener(this);
        }
    }

    public void OnEventRaised(GameEvent evnt)
    {
        var raisedEvent = GameEvents.Where(x => x.Event == evnt).SingleOrDefault();
        if(raisedEvent.Event != null)
        {
            raisedEvent.Response.Invoke();
        }
    }

    [Serializable]
    public class GameEventResponse
    {
        public GameEvent Event;
        public UnityEvent Response;
    }
}
