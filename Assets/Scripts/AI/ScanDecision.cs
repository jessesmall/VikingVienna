using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decision/Scan")]
public class ScanDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool noEnemyInSight = Scan(controller);
        return noEnemyInSight;
    }

    private bool Scan(StateController controller)
    {
        return false;
    }
}
