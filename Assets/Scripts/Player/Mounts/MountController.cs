using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class should be used for non-player entities that the player will ride.
/// This means the player is able to control the mount without forfitting their control over the player, they may exit mount at anytime.
/// </summary>
public class MountController : BaseController, IJump {

    [SerializeField]
    private float _jumpForce;
    public float JumpForce
    {
        get { return _jumpForce; }
        set { _jumpForce = value; }
    }

    private bool _hasDoubleJump;
    public bool HasDoubleJumped { get { return _hasDoubleJump; } set { _hasDoubleJump = value; } }

    [SerializeField]
    private bool _canDoubleJump;
    public bool CanDoubleJump
    {
        get { return _canDoubleJump; }
        set { _canDoubleJump = value; }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        HandleMountMovement();
    }

    private void HandleMountMovement()
    {
        if (CanMove)
        {
            RBody2D.velocity = new Vector2(XInput * MaxSpeed, RBody2D.velocity.y);
            Anim.SetFloat("mountSpeed", Mathf.Abs(XInput));
        }
    }

    public void HandleJump()
    {
        if(IsGrounded && CanMove)
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
        }
    }
}
