using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMountController : BaseController, IJump, IMelee {

    [SerializeField]
    private float _jumpForce;
    public float JumpForce
    {
        get { return _jumpForce; }
        set { _jumpForce = value; }
    }

    private bool _hasDoubleJump;
    public bool HasDoubleJumped { get { return _hasDoubleJump; } set { _hasDoubleJump = value; } }

    private bool _canDoubleJump = true;
    public bool CanDoubleJump
    {
        get { return _canDoubleJump; }
        set { _canDoubleJump = value; }
    }

    [SerializeField]
    private float _attackSpeed;
    public float AttackSpeed
    {
        get { return _attackSpeed; }
        set { _attackSpeed = value; }
    }

    private float _canAttackIn;
    public float CanAttackIn
    {
        get { return _canAttackIn; }
        set { _canAttackIn = value; }
    }

    private bool _isFlying;
    private float _gravityStore;

    public override void Start()
    {
        base.Start();
        _gravityStore = RBody2D.gravityScale;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if(CanAttackIn > 0)
        {
            CanAttackIn -= Time.fixedDeltaTime;
        }
        if (IsGrounded)
        {
            _isFlying = false;
            RBody2D.gravityScale = _gravityStore;
            Anim.SetBool("isFlying", false);
        }
            HandleFlyingMountMovement();
    }

    private void HandleFlyingMountMovement()
    {
        if (CanMove && IsGrounded && !_isFlying)
        {
            RBody2D.velocity = new Vector2(XInput * MaxSpeed, RBody2D.velocity.y);
            Anim.SetFloat("mountSpeed", Mathf.Abs(XInput));
        }
        else if(CanMove && !IsGrounded && _isFlying)
        {
            RBody2D.velocity = new Vector2(XInput * MaxSpeed, YInput * MaxSpeed);
            var velocity = Mathf.Abs(XInput) > 0.1 ? XInput : YInput;
            Anim.SetFloat("mountSpeed", Mathf.Abs(velocity));
        }
    }

    public void HandleJump()
    {
        if ((IsGrounded || _isFlying) && CanMove)
        {
            Anim.SetTrigger("mountJump");
            RBody2D.velocity = new Vector2(RBody2D.velocity.x, 0);
            RBody2D.AddForce(new Vector2(0, JumpForce));
            HasDoubleJumped = false;
        }
        else if (CanDoubleJump && !HasDoubleJumped && CanMove)
        {
            RBody2D.velocity = new Vector2(RBody2D.velocity.x, 0);
            RBody2D.AddForce(new Vector2(0, JumpForce));
            HasDoubleJumped = true;
            _isFlying = true;
            RBody2D.gravityScale = 0;
            Anim.SetBool("isFlying", true);
        }
    }

    public void HandleMelee()
    {
        if(CanAttackIn <= 0)
        {
            CanAttackIn = AttackSpeed;
            Anim.SetTrigger("meleeAttack");
        }
    }
}
