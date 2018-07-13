using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : Action
{
    public LayerMask layerMask;
    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        var distanceToPlayer = Mathf.Abs(Vector2.Distance(controller.eyes.position, controller.chaseTarget.position));
        var canAttack = controller.CheckIfCountDownElapsed(controller.enemyStats.attackSpeed);
        if (distanceToPlayer <= controller.enemyStats.attackRange && canAttack)
        {
            var direction = (controller.chaseTarget.position - controller.eyes.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(controller.eyes.position, direction, controller.enemyStats.lookDistance, layerMask);
            Debug.DrawRay(controller.eyes.position, direction * controller.enemyStats.lookDistance, Color.green);
            if (hit.collider != null && hit.collider.GetComponentInParent<PlayerController>())
            {
                controller.anim.SetTrigger("Attack");
                controller.stateTimeElapsed = 0;
                controller.rb.velocity = new Vector2(0, controller.rb.velocity.y);
                SoundManager.instance.RandomSfx(controller.transform, controller.audioClips.meleeAttackAudio);
            }
        }
    }
}