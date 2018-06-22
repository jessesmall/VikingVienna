using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class KeyItem : Item
{
    public enum Type { Bronze, Silver, Gold, Boss};
    public Type KeyType;
    public override bool Use(GameObject caller)
    {
        //Check if we are within a container
        var chests = GameObject.FindGameObjectsWithTag("Chest");
        Container container = null;
        foreach(var chest in chests)
        {
            var cont = chest.GetComponent<Container>();
            if (cont != null && cont.playerNearby)
            {
                container = cont;
            }
        }
        if (container != null)
        {
            if(container.KeyType == KeyType)
            {
                container.OpenContainer();
                return true;
            }
        }
        // We were unable to use the key
        return false;
    }
}
