using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mount : Interactable {

    public Transform mountLocation;
    public BoxCollider2D BodyCollider;
    private bool _mounted = false;

    public override void Start()
    {
        base.Start();
        GetComponent<BaseController>().CanMove = false;
        GetComponent<ControllerInput>().AllowInput = false;
    }

    public override void Interact()
    {
        base.Interact();
        if (!_mounted)
        {
            BodyCollider.isTrigger = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GetComponent<BaseController>().CanMove = true;
            _player.GetComponent<Rigidbody2D>().simulated = false;
            _player.transform.position = mountLocation.position;
            _player.transform.parent = mountLocation;
            _player.GetComponent<PlayerController>().IsMounted = true;
            this.GetComponent<ControllerInput>().AllowInput = true;
            _mounted = true;
        }
        else
        {
            _mounted = false;
            this.GetComponent<ControllerInput>().AllowInput = false;
            _player = transform.GetComponentInChildren<PlayerController>().gameObject;
            _player.GetComponent<PlayerController>().IsMounted = false;
            _player.transform.parent = null;
            _player.GetComponent<Rigidbody2D>().simulated = true;
            StartCoroutine(FallToGround());
        }
    }

    IEnumerator FallToGround()
    {
        LayerMask layerMask = LayerMask.GetMask("Obstacle", "Platform");
        while (!GetComponent<BaseController>().IsGrounded)
        {
            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, layerMask);
            Debug.Log(rayHit.point);
            Debug.Log(rayHit.transform.gameObject);
            if (rayHit)
            {
                transform.position = Vector3.Lerp(transform.position, rayHit.point, 0.2f);
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(0.2f);
        }
        GetComponent<BaseController>().CanMove = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        BodyCollider.isTrigger = true;
    }
}
