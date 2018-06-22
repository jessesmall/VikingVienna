using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMelee {

    float AttackSpeed { get; set; }
    float CanAttackIn { get; set; }

    void HandleMelee();
}
