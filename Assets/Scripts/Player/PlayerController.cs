using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    private PlayerAbilities _abilities;

    private bool _facingRight = true;
    private Rigidbody2D _rigidBody2D;

    private bool _isGrounded = false;
    private bool _hasDoubleJumped = false;

    public float JumpHeight;
    public bool CanDoubleJump = false;

    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    public float maxSpeed = 10f;

    private Animator _anim;

    private bool _isAttacking = false;

    public bool _canMove = true;

    private float _attackSpeed;
    public float fireRate;

    [HideInInspector]
    public float canFireIn = 0f;

    [HideInInspector]
    public float canMeleeIn = 0f;

    [HideInInspector]
    public float lastTimeDamaged;

    private BoxCollider2D _swordColliderBox;

    public Projectile rangedProjectile;
    public Transform projectileFireLocation;

    public AudioClip meleeSound;
    public AudioClip rangedSound;

    public bool canClimb = false;
    public float climbSpeed;
    private float climbVelocity;
    private float gravityStore;

    [HideInInspector]
    public List<Interactable> InteractableObjects = new List<Interactable>();

    [HideInInspector]
    public bool IsMounted;

    [HideInInspector]
    public bool IsDead;

    public AudioClip NotLearned;


    // Use this for initialization
    void Start () {
        _abilities = GetComponent<PlayerAbilities>(); 
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _rigidBody2D.freezeRotation = true;

        gravityStore = _rigidBody2D.gravityScale;

        _anim = GetComponentInChildren<Animator>();

        _swordColliderBox = GetComponentInChildren<MeleeAttack>().GetComponentInParent<BoxCollider2D>();
        _attackSpeed = GetComponentInChildren<MeleeAttack>().AttackSpeed;
        _swordColliderBox.enabled = false;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Physics2D.IgnoreLayerCollision(8, 11, (_rigidBody2D.velocity.y > 0.0f));
        if (IsMounted)
            _anim.SetFloat("playerSpeed", 0);
        CheckForGround();
        if(canFireIn > 0)
        {
            canFireIn -= Time.fixedDeltaTime;
        }
        if(canMeleeIn > 0)
        {
            canMeleeIn -= Time.fixedDeltaTime;
        }
    }

    void FlipCharacter()
    {
        _facingRight = !_facingRight;

        Vector3 characterScale = transform.localScale;
        characterScale.x *= -1;
        transform.localScale = characterScale;
    }

    public void HandleDirection(Vector2 directionalInput)
    {
        if (IsMounted)
            return;
        if (!_isAttacking && _canMove)
        {
            float characterMovement = directionalInput.x;
            _rigidBody2D.velocity = new Vector2(characterMovement * maxSpeed, _rigidBody2D.velocity.y);
            _anim.SetFloat("playerSpeed", Mathf.Abs(characterMovement));

            if (characterMovement > 0 && !_facingRight)
            {
                FlipCharacter();
            }
            else if (characterMovement < 0 && _facingRight)
            {
                FlipCharacter();
            }
        }
    }

    public bool HandleVert(float rawVert)
    {
        if (IsMounted)
            return false; ;
        if(rawVert > 0)
        {
            HandleClimb(rawVert);
        }
        else if(rawVert < 0)
        {
            if (canClimb)
                HandleClimb(rawVert);
            else
            {
                HandleDuck(true);
                return true;
            }
        }
        else
        {
            HandleDuck(false);
            HandleClimb(rawVert);
        }
        return false;
    }

    public void HandleDuck(bool shouldDuck)
    {
        if (shouldDuck && _isGrounded && !_anim.GetBool("playerDucking"))
        {
            _rigidBody2D.velocity = new Vector2(0, 0);
            _anim.SetBool("playerDucking", true);
            _anim.SetTrigger("playerDucked");
        }
        else if(!shouldDuck)
        {
            _anim.SetBool("playerDucking", false);
            _anim.SetTrigger("playerDucked");
        }
        return;
    }

    public void HandleClimb(float upValue)
    {
        if (canClimb)
        {
            _rigidBody2D.gravityScale = 0f;
            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, upValue * maxSpeed);
        }
        else
        {
            _rigidBody2D.gravityScale = gravityStore;
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (IsMounted)
            return;
        if(collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            transform.parent = collision.transform;
        }
        else
        {
            transform.parent = null;
        }
    }
    
    public void HandleJump()
    {
        if (IsMounted)
        {
            return;
        }
        if (_isGrounded && _canMove)
        {
            _anim.SetTrigger("playerJump");
            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0);
            var velocity = _rigidBody2D.velocity;
            velocity.y = CalculateJumpVerticalSpeed(JumpHeight);
            _rigidBody2D.velocity = velocity;
            _hasDoubleJumped = false;
        }
        else if(CanDoubleJump && !_hasDoubleJumped && _canMove)
        {
            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0);
            var velocity = _rigidBody2D.velocity;
            velocity.y = CalculateJumpVerticalSpeed(JumpHeight);
            _rigidBody2D.velocity = velocity;
            _hasDoubleJumped = true;
        }
    }

    private float CalculateJumpVerticalSpeed(float targetJumpHeight)
    {
        return Mathf.Sqrt(Mathf.Abs(2f * targetJumpHeight * Physics2D.gravity.y * _rigidBody2D.gravityScale));
    }

    private void CheckForGround()
    {
        if (IsMounted)
        {
            _isGrounded = true;
            return;
        }
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        _anim.SetBool("playerGrounded", _isGrounded);
        _anim.SetFloat("playerVerticalSpeed", _rigidBody2D.velocity.y);
    }

    public IEnumerator HandleAttack()
    {
        _rigidBody2D.velocity = new Vector2(0, _rigidBody2D.velocity.y);
        _swordColliderBox.enabled = true;
        GetComponent<AudioSource>().PlayOneShot(meleeSound);
        canMeleeIn = _attackSpeed;
        _anim.SetTrigger("playerSwordSlash");
        _isAttacking = true;
        yield return new WaitForSeconds(0.6f);
        _isAttacking = false;
        _swordColliderBox.enabled = false;
    }

    public IEnumerator HandleRangedAttack()
    {
        if (_abilities.CanUseMagic)
        {
            if (GetComponent<HealthManager>().UseMana(1))
            {
                var direction = _facingRight ? Vector2.right : Vector2.left;
                direction.Normalize();

                var projectile = (Projectile)Instantiate(rangedProjectile, projectileFireLocation.position, projectileFireLocation.rotation);
                projectile.Initialize(gameObject, direction, _rigidBody2D.velocity);
                canFireIn = fireRate;
                GetComponent<AudioSource>().PlayOneShot(rangedSound);
                _anim.SetTrigger("playerSwordSlash");

                _isAttacking = true;
                yield return new WaitForSeconds(0.6f);
                _isAttacking = false;
            }
            else
            {
                Debug.Log("Not Enough Mana");
            }
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(NotLearned);
        }
    }

    public void InputDisabled()
    {
        _rigidBody2D.velocity = new Vector2(0, 0);
        _anim.SetFloat("playerSpeed", 0);
    }

    public void FinishLevel()
    {
        _canMove = false;
        _anim.enabled = false;
    }

    public void InteractWith()
    {
        if (IsMounted)
        {
            transform.GetComponentInParent<Interactable>().Interact();
            return;
        }
        var obj = InteractableObjects.OrderByDescending(x => Vector2.Distance(transform.position, x.transform.position)).FirstOrDefault();
        if(obj != null)
        {
            obj.Interact();
        }
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
