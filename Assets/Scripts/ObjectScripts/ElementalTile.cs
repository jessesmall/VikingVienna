using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalTile : MonoBehaviour {

    public ElementType TileElement;
    public ElementType EffectElement;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var projectile = collision.gameObject.GetComponent<SimpleProjectile>();
        if(projectile != null)
        {
            var element = projectile.Element;

            if(element == EffectElement)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void HitWall(ElementType element)
    {
        if (element == EffectElement)
        {
            gameObject.SetActive(false);
        }
    }
}
