using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenDoor : MonoBehaviour{

    private bool doorOpened = false;

    public void OpenDoor()
    {
        if (!doorOpened && gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
        else if(!doorOpened && !gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
    }

}
