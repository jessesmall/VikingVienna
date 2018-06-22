using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SkillItem : Item
{
    public override bool Use(GameObject caller)
    {
        base.Use(caller);
        caller.gameObject.GetComponent<PlayerAbilities>().CanUseMagic = true;
        return true;
    }
}
