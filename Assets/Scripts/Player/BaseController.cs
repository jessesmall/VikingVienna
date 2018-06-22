using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour {

    // Should be in base class
    private bool _facingRight = true;
    protected bool FacingRight
    {
        get { return _facingRight; }
    }
    private Rigidbody2D _rigidBody2D;
    protected Rigidbody2D RBody2D
    {
        get { return _rigidBody2D; }
    }

    private bool _isGrounded = false;
    public bool IsGrounded
    {
        get { return _isGrounded; }
    }

    [SerializeField]
    private Transform GroundCheck;
    private float _groundRadius = 0.2f;

    [SerializeField]
    private LayerMask WhatIsGround;

    [SerializeField]
    private float _maxSpeed = 10f;
    [HideInInspector]
    public float MaxSpeed
    {
        get { return _maxSpeed; }
    }

    private Animator _anim;
    protected Animator Anim
    {
        get { return _anim; }
    }

    private bool _canMove = true;
    public bool CanMove
    {
        get { return _canMove; }
        set { _canMove = value; }
    }

    private float _xInput;
    protected float XInput
    {
        get { return _xInput; }
    }
    private float _yInput;
    protected float YInput
    {
        get { return _yInput; }
    }

    // Use this for initialization
    public virtual void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _rigidBody2D.freezeRotation = true;

        _anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per physics update
    public virtual void FixedUpdate()
    {
        CheckForGround();
    }

    // Flip character sprite to correct for direction change
    void FlipCharacter()
    {
        _facingRight = !_facingRight;

        Vector3 characterScale = transform.localScale;
        characterScale.x *= -1;
        transform.localScale = characterScale;
    }

    public void HandleHorizontalInput(float rawHorizontal)
    {
        _xInput = rawHorizontal;

            if (_xInput > 0 && !_facingRight)
            {
                FlipCharacter();
            }
            else if (_xInput < 0 && _facingRight)
            {
                FlipCharacter();
            }
    }

    public void HandleVerticalInput(float rawVertical)
    {
        _yInput = rawVertical;
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            transform.parent = collision.transform;
        }
        else
        {
            transform.parent = null;
        }
    }

    private void CheckForGround()
    {
        _isGrounded = Physics2D.OverlapCircle(GroundCheck.position, _groundRadius, WhatIsGround);
    }

    public void FinishLevel()
    {
        _canMove = false;
        _anim.enabled = false;
    }

    public void RespawnAt(Transform spawnPoint)
    {
        if (!_facingRight)
        {
            FlipCharacter();
        }

        transform.position = spawnPoint.position;
    }
}
