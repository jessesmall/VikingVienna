using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action
{
    private float nextPointDistance = 0.35f;
    public override void Act(StateController controller)
    {
        Chase(controller);
    }

    private void Chase(StateController controller)
    {
        var distanceToPlayer = Mathf.Abs(Vector2.Distance(controller.eyes.position, controller.chaseTarget.position));
        if(distanceToPlayer <= controller.enemyStats.attackRange * 0.75)
        {
            controller.rb.velocity = new Vector2(0, 0);
            controller.anim.SetBool("Chase", false);
            return;
        }
        if (controller.chasePathUpdate)
        {
            controller.chasePathUpdate = false;
            controller.pathCompleted = false;
            PathRequestManager.RequestPath(new PathRequest(controller.transform.position, controller.chaseTarget.position, controller, OnPathFound));
        }

        if (!controller.pathCompleted)
        {
            return;
        }

        Vector3 direction = (controller.path[controller.currentPathPoint] - controller.transform.position).normalized;
        float dist = Vector3.Distance(controller.transform.position, controller.path[controller.currentPathPoint]);

        // Move Enemy
        controller.rb.velocity = new Vector2((controller.isFacingRight ? 1 : -1)*controller.enemyStats.speed, controller.rb.velocity.y);
        //Handle Animation
        controller.anim.SetBool("Chase", true);
        //Check to update next point along the path
        if (dist < nextPointDistance)
        {
            Debug.Log("WALKING, PLAY SOUND EFFECT");
            SoundManager.instance.RandomSfx(controller.transform, controller.audioClips.walkAudio);
            controller.currentPathPoint = (controller.currentPathPoint + 1) % controller.path.Length;
            if(controller.currentPathPoint > 2 || controller.currentPathPoint == 0)
            {
                controller.chasePathUpdate = true;
            }
        }
    }

    private void OnPathFound(Vector3[] newPath, bool pathSuccessful, StateController controller)
    {
        if (pathSuccessful)
        {
            controller.path = newPath;
            controller.currentPathPoint = 0;
            controller.currentTarget = controller.chaseTarget;
            controller.pathCompleted = true;

            var lineRenderer = controller.GetComponent<LineRenderer>();
            if(lineRenderer != null)
            {
                lineRenderer.positionCount = newPath.Length;
                lineRenderer.SetPositions(newPath);
            }
        }
    }
}
