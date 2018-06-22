using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class FoodItem : Item
{
    public enum Type { Health, Mana, PowerUp, Script, Other}
    
    public Type FoodType;
    public int FoodValue;

    public UnityEvent foodScript;

    public override bool Use(GameObject caller)
    {
        HealthManager healthManager = caller.GetComponent<HealthManager>();
        switch (FoodType)
        {
            case Type.Health:
                healthManager.GainHealth(FoodValue);
                return true;
            case Type.Mana:
                healthManager.GainMana(FoodValue);
                return true;
            case Type.Script:
                foodScript.Invoke();
                return true;
            case Type.Other:
                TextBoxInteraction.Instance.ShowText(OptionalItemUseText, DisplayTime, Font);
                return false;
            default:
                return false;
        }
    }
}
