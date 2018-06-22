using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Suicide")]
public class SuicideAction : Action
{
    public LayerMask layerMask;
    public override void Act(StateController controller)
    {
        Suicide(controller);
    }

    private void Suicide(StateController controller)
    {
        var distanceToPlayer = Mathf.Abs(Vector2.Distance(controller.eyes.position, controller.chaseTarget.position));
        if (distanceToPlayer <= controller.enemyStats.attackRange)
        {
            var direction = (controller.chaseTarget.position - controller.eyes.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(controller.eyes.position, direction, controller.enemyStats.lookDistance, layerMask);
            if (hit.collider != null && hit.collider.GetComponentInParent<PlayerController>())
            {
                Instantiate(controller.enemyStats.destroyEffect, controller.effectLocation.position, controller.effectLocation.rotation);
                SoundManager.instance.RandomSfx(controller.transform, controller.audioClips.deathAudio);
                controller.TransitionToState(controller.deadState);
            }
        }
    }
}