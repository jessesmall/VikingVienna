using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepElevator : MonoBehaviour {

    public enum FollowType { MoveTowards, Lerp}

    public FollowType type = FollowType.MoveTowards;
    public PathDefinition path;
    public float speed = 1;
    public float maxDistanceToGoal = .1f;

    private bool playerOn;
    private bool activated = false;

    private IEnumerator<Transform> currentPoint;

    private float elevatorInitialize;

    public void Start()
    {
        if (path == null)
        {
            Debug.LogError("Path cannot be null", gameObject);
            return;
        }

        currentPoint = path.GetPathEnumerator();
        currentPoint.MoveNext();

        if (currentPoint.Current == null)
            return;

        transform.position = currentPoint.Current.position;

    }

    public void FixedUpdate()
    {
            if (currentPoint == null || currentPoint.Current == null)
                return;
            if (type == FollowType.MoveTowards)
                transform.position = Vector3.MoveTowards(transform.position, currentPoint.Current.position, Time.deltaTime * speed);
            else if (type == FollowType.Lerp)
                transform.position = Vector3.Lerp(transform.position, currentPoint.Current.position, Time.deltaTime * speed);

            var distanceSquared = (transform.position - currentPoint.Current.position).sqrMagnitude;
            if(elevatorInitialize > 0)
            {
                elevatorInitialize -= Time.fixedDeltaTime;
            }
            if (distanceSquared < maxDistanceToGoal * maxDistanceToGoal && playerOn && elevatorInitialize <= 0)
            {
                playerOn = false;
                currentPoint.MoveNext();
            }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<PlayerController>() != null && !activated)
        {
            elevatorInitialize = 1f;
            playerOn = true;
            activated = true;
        }
    }
}
