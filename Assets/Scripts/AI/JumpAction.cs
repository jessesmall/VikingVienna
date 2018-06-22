using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Jump")]
public class JumpAction : Action
{
    public override void Act(StateController controller)
    {
        Jump(controller);
    }

    private void Jump(StateController controller)
    {
        if(controller.path != null && controller.pathCompleted)
        {
            Vector3 direction = (controller.path[controller.currentPathPoint] - controller.transform.position).normalized;
            if (direction.y > 0.8 && controller.isGrounded)
            {
                controller.rb.velocity = new Vector2(0, 0);
                float gravity = Physics2D.gravity.y * controller.rb.gravityScale;
                float initialVelocity = Mathf.Sqrt(-2f * controller.enemyStats.jumpHeight * gravity);
                controller.rb.velocity = new Vector2(controller.rb.velocity.x, initialVelocity);
                controller.anim.SetTrigger("Jump");
                if(controller.audioClips.jumpAudio.Length != 0)
                    SoundManager.instance.PlaySingle(controller.transform, controller.audioClips.jumpAudio[0]);
            }
        }
    }
}
