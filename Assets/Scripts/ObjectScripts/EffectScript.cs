using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour {

    public void OneTimeThenKill()
    {
        var children = new List<GameObject>();
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }
        children.ForEach(child => Destroy(child));
        Destroy(gameObject);
    }
}
