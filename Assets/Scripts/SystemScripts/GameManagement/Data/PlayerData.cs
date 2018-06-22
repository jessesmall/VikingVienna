using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {

    public int MaxHealth;
    public int MaxMana;

    public GameObject Weapon;

    public bool CanUseMagic;
    public bool CanUseThunder;
    public bool CanUseFire;
    public bool CanUseIce;
}
