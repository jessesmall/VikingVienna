using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartPathedProjectileSpawner : PathedProjectileSpawner {

    public float MaxAngleOfRotation;
    private Vector2 _currentDirection;

    public GameObject RotationalComponent;

    public float MaxRange;

    public LayerMask CollidableLayers;

    private bool AngledUp = true;

    private bool _foundPlayer = true;

    public override void Start()
    {
        _currentDirection = isFacingRight ? RotationalComponent.transform.right : -RotationalComponent.transform.right;
    }

    public override void Update()
    {
        if (LookForPlayer())
        {
            base.Update();
        }
        else if(_foundPlayer)
        {
            _foundPlayer = false;
            StartCoroutine(AdjustAngle());
        }
    }

    public new void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(SpawnLocation.transform.position, SpawnLocation.transform.position + SpawnLocation.transform.right * MaxRange);
    }

    private bool LookForPlayer()
    {
        RaycastHit2D hit;

        hit = Physics2D.Raycast(SpawnLocation.transform.position, _currentDirection, MaxRange, CollidableLayers);

        if (hit && hit.transform.gameObject.GetComponentInParent<PlayerController>() != null)
        {
            Destination.position = hit.point;
            _foundPlayer = true;
            return true;
        }
        else
        {
            return false;
        }

    }

    private IEnumerator AdjustAngle()
    {
        while (!_foundPlayer)
        {
            if (Mathf.Abs(WrapAngle(RotationalComponent.transform.localRotation.eulerAngles.z)) >= MaxAngleOfRotation)
            {
                AngledUp = !AngledUp;
            }
            if (AngledUp)
            {
                RotationalComponent.transform.Rotate(Vector3.forward * 1);
            }
            else
            {
                RotationalComponent.transform.Rotate(Vector3.forward * -1);
            }

            _currentDirection = isFacingRight ? SpawnLocation.transform.right : -RotationalComponent.transform.right;

            yield return new WaitForSeconds(0.05f);
        }
    }

    private float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }
}
