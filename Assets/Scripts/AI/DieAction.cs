using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Die")]
public class DieAction : Action
{
    public override void Act(StateController controller)
    {
        Die(controller);
    }

    private void Die(StateController controller)
    {
        if (controller.IsActive && controller.isGrounded)
        {
            controller.tag = "Untagged";
            controller.healthBar.gameObject.SetActive(false);
            controller.rb.isKinematic = true;
            controller.rb.simulated = false;
            controller.IsActive = false;
            controller.anim.SetBool("IsDead", true);
            controller.anim.SetTrigger("Died");
            SoundManager.instance.PlaySingle(controller.transform, controller.audioClips.deathAudio[0]);
        }
    }
}
