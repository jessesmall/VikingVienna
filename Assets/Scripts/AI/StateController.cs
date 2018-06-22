using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateController : MonoBehaviour
{
    public ScriptableAudio audioClips;
    public State currentState;
    public State remainState;
    public State deadState;
    public EnemyStats enemyStats;
    public Transform eyes;

    public bool isFacingRight;

    public bool isGrounded;

    public LayerMask WhatIsGround;

    public List<Transform> wayPointList;

    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;

    [HideInInspector] public int currentWayPoint;

    [HideInInspector] public Vector3[] path;
    [HideInInspector] public bool pathCompleted;
    [HideInInspector] public int currentPathPoint;

    public Transform chaseTarget;
    [HideInInspector] public float stateTimeElapsed;
    [HideInInspector] public bool chasePathUpdate = true;

    private Transform _currentTarget;
    public Transform currentTarget
    {
        get { return _currentTarget; }
        set
        {
            var oldValue = _currentTarget;
            _currentTarget = value;
            if (_currentTarget != oldValue)
                CheckTargetPosition();
        }
    }

    public Transform effectLocation;

    public bool IsActive = true;

    // Health Management
    [HideInInspector] public Slider healthBar;
    [HideInInspector] public int currentHealth;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        currentHealth = enemyStats.maxHealth;
        healthBar = GetComponentInChildren<Slider>();
    }

    void FixedUpdate()
    {
        if (!IsActive)
            return;
        currentState.UpdateState(this);
        CheckTargetPosition();
        CheckIsGrounded();
    }

    void OnDrawGizmos()
    {
        if (currentState != null && eyes != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawRay(eyes.position, (isFacingRight ? Vector3.right : Vector3.left) * enemyStats.lookDistance);
        }
    }

    public void TransitionToState(State nextState)
    {
        if(nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.fixedDeltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
        anim.Rebind();
    }

    private void CheckTargetPosition()
    {
        if(currentTarget != null)
        {
            // If the target is to the right of us and we are not facing right, face right
            if (currentTarget.position.x > transform.position.x && !isFacingRight && IsActive)
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                isFacingRight = true;
            }
            //If target is to the left of us and we are facing right, face left
            else if (currentTarget.position.x < transform.position.x && isFacingRight && IsActive)
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                isFacingRight = false;
            }
        }
    }

    private void CheckIsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.1f, WhatIsGround);
    }

    public void SetActive(bool active)
    {
        IsActive = active;
    }

    public void TakeDamage()
    {
        if (IsActive)
        {
            currentHealth--;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Instantiate(enemyStats.hurtEffect, effectLocation.position, effectLocation.rotation);
                TransitionToState(deadState);
            }
            else
            {
                Instantiate(enemyStats.hurtEffect, effectLocation.position, effectLocation.rotation);
                anim.SetTrigger("Hurt");
            }
            healthBar.value = Mathf.Clamp01((float)currentHealth / (float)enemyStats.maxHealth);
        }
    }
}