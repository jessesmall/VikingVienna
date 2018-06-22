using UnityEngine;
using System.Collections;

public interface IPlayerRespawnListener {

    void OnPlayerRespawnInThisCheckPoint(CheckPoint checkPoint, PlayerController player);
}
