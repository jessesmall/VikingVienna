using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Follow")]
public class FollowDecision : Decision
{
    public LayerMask layerMask;
    public override bool Decide(StateController controller)
    {
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    private bool Look(StateController controller)
    {
        var distanceToPlayer = Mathf.Abs(Vector2.Distance(controller.eyes.position, controller.chaseTarget.position));
        var angleToPlayer = Vector2.Angle(controller.transform.right * (controller.isFacingRight ? 1 : -1), controller.chaseTarget.position - controller.eyes.position);
        var direction = (controller.chaseTarget.position - controller.eyes.position).normalized;

        if(distanceToPlayer <= controller.enemyStats.lookDistance + (controller.enemyStats.lookDistance * 0.5f) && (angleToPlayer <= controller.enemyStats.fov || (distanceToPlayer <= controller.enemyStats.attackRange)))
        {
            RaycastHit2D hit = Physics2D.Raycast(controller.eyes.position, direction, controller.enemyStats.lookDistance, layerMask);
            Debug.DrawRay(controller.eyes.position, direction * controller.enemyStats.lookDistance, Color.green);
            if (hit.collider != null && hit.collider.GetComponentInParent<PlayerController>())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }
}
