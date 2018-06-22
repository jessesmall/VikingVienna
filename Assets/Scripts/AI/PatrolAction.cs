using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Action
{
    private float nextPointDistance = 0.5f;
    public override void Act(StateController controller)
    {
        if(controller.wayPointList != null && controller.wayPointList.Count > 1)
        {
            Patrol(controller);
        }
        else
        {
            controller.rb.velocity = new Vector2(0, 0);
        }
    }

    private void Patrol(StateController controller)
    {
        if((controller.path == null || controller.currentTarget != controller.wayPointList[controller.currentWayPoint]))
        {
            controller.pathCompleted = false;
            controller.currentTarget = controller.wayPointList[controller.currentWayPoint];
            PathRequestManager.RequestPath(new PathRequest(controller.transform.position, controller.wayPointList[controller.currentWayPoint].position, controller, OnPathFound));
        }

        if (!controller.pathCompleted)
        {
            controller.rb.velocity = new Vector2(0, 0);
            return;
        }

        Vector3 direction = (controller.path[controller.currentPathPoint] - controller.transform.position).normalized;
        float dist = Vector3.Distance(controller.transform.position, controller.path[controller.currentPathPoint]);

        // Move enemy
        controller.rb.velocity = new Vector2((controller.isFacingRight ? 1 : -1)*controller.enemyStats.speed, controller.rb.velocity.y);
        //Handle Animation
        controller.anim.SetBool("Patrol", true);
        if (dist < nextPointDistance)
        {
            SoundManager.instance.RandomSfx(controller.transform, controller.audioClips.walkAudio);
            controller.currentPathPoint = (controller.currentPathPoint + 1) % controller.path.Length;
            // If we just hit the last path point, update the waypoint
            if (controller.currentPathPoint == 0)
            {
                controller.currentWayPoint = (controller.currentWayPoint + 1) % controller.wayPointList.Count;
            }
        }
    }

    private void OnPathFound(Vector3[] newPath, bool pathSuccessful, StateController controller)
    {
        if (pathSuccessful)
        {
            controller.path = newPath;
            controller.currentPathPoint = 0;
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