using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class SwitchEvent : UnityEvent<Switch.SwitchState> { }

public class Switch : Interactable
{
    public enum SwitchState { High, Mid, Low};
    public SwitchState state;
    public bool ThreeStateSwitch = false;
    public Sprite[] SwitchIcons;

    private SpriteRenderer _sprite;

    public SwitchEvent SwitchEvent;

    public override void Start()
    {
        base.Start();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _sprite.sprite = SwitchIcons[(int)state];
    }

    public override void Interact()
    {
        if (ThreeStateSwitch)
        {
            int newState = (int)state <= 1 ? (int)state + 1 : 0;
            state = (SwitchState)newState;
        }
        else
        {
            int newState = (int)state == 0 ? 2 : 0;
            state = (SwitchState)newState;
        }
        _sprite.sprite = SwitchIcons[(int)state];
        RunSwitchScript(state);

        base.Interact();
    }

    

    public void RunSwitchScript(SwitchState state)
    {
        if(SwitchEvent != null)
        {
            SwitchEvent.Invoke(state);
        }
    }
}
