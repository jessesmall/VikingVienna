using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;
using UnityEngine.UI;
using Assets.Scripts.Player;

public class HealthManager : MonoBehaviour {

    public Rigidbody2D playerBody;

    public PlayerController Player;
    public AudioClip hitSound;
    private PlayerInput _playerInput;
    private AudioSource audioSource;

    private float lastTimeHit = -0.5f;
    private float hitTime = 1f;
    private bool invincible = false;

    private int counter = 0;
    private int delay = 4;

    public bool disableHits = false;

    public enum PlayerStatus { None, Poison, Frozen, Burning, Stunned};
    public PlayerStatus playerStatus = PlayerStatus.None;
    private float infectedTime;

    public ScriptableInt health;
    public ScriptableInt mana;

    // Use this for initialization
    void Start() {
        audioSource = GetComponent<AudioSource>();
        _playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update() {

        if (invincible && Time.time < lastTimeHit + hitTime)
        {
            if(counter >= delay)
            {
                counter = 0;
                foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.enabled = !sprite.enabled;
                }
            }
            else
            {
                counter++;
            }
        }
        else if(invincible)
        {
            foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.enabled = true;
            }
            invincible = false;
        }
    }

    public void ChangePlayerStatus(PlayerStatus status, float duration)
    {
        infectedTime = duration;
        playerStatus = status;
        StartCoroutine(CheckPlayerStatus());
    }

    private IEnumerator CheckPlayerStatus()
    {
        while (true)
        {
            switch (playerStatus)
            {
                case PlayerStatus.None:
                    foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
                    {
                        sprite.color = Color.white;
                    }
                    break;
                case PlayerStatus.Stunned:
                    _playerInput.DisablePlayerInput();
                    break;
                case PlayerStatus.Poison:
                    foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
                    {
                        sprite.color = Color.green;
                    }
                    TakeNoIFrameDamage();
                    break;
            }
            if (infectedTime > 0)
            {
                infectedTime -= 1f;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                playerStatus = PlayerStatus.None;
                _playerInput.EnablePlayerInput();
                break;
            }
        }
    }

    public bool UseMana(int manaCost)
    {
        if(manaCost > mana.RuntimeValue)
        {
            return false;
        }
        else
        {
            mana.RuntimeValue -= manaCost;
            return true;
        }
    }

    public void GainMana(int manaValue)
    {
        if(manaValue + mana.RuntimeValue > mana.InitialValue)
        {
            mana.RuntimeValue = mana.InitialValue;
        }
        else
        {
            mana.RuntimeValue += manaValue;
        }
    }

    public void GainHealth(int healthValue)
    {
        if (healthValue + health.RuntimeValue > health.InitialValue)
        {
            health.RuntimeValue = health.InitialValue;
        }
        else
        {
            health.RuntimeValue += healthValue;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy" && Time.time > lastTimeHit + hitTime && !disableHits)
        {
            health.RuntimeValue--;
            KnockBack(coll.transform.position);
            audioSource.PlayOneShot(hitSound);
            lastTimeHit = Time.time;
            invincible = true;
        }

        if (health.RuntimeValue == 0 && !disableHits)
        {
            LevelManager.Instance.KillPlayer();
        }
    }

    public void TakeNoIFrameDamage()
    {
        if(health.RuntimeValue != 0 && !disableHits)
        {
            health.RuntimeValue--;
            audioSource.PlayOneShot(hitSound);
        }
        if (health.RuntimeValue == 0 && !disableHits)
        {
            LevelManager.Instance.KillPlayer();
        }
    }

    public void TakeDamage()
    {
        if(Time.time > lastTimeHit + hitTime && !disableHits)
        {
            health.RuntimeValue--;
            lastTimeHit = Time.time;
            invincible = true;
            audioSource.PlayOneShot(hitSound);
        }

        if (health.RuntimeValue == 0 && !disableHits)
        {
            LevelManager.Instance.KillPlayer();
        }
    }

    void KnockBack(Vector3 HazardPosition)
    {
        StartCoroutine("haltMovement");
        Vector3 heading = transform.position - HazardPosition;
        float distance = heading.magnitude;
        Vector3 direction = heading / distance;

        Vector2 directionForVelocity = new Vector2(direction.x, direction.y);
        playerBody.velocity = directionForVelocity * 2f;

    }

    public void OnPlayerDied()
    {
        health.RuntimeValue = 0;
        disableHits = true;
        Player.IsDead = true;
        Player.GetComponent<PlayerBounds>().enabled = false;
        Player.GetComponent<PlayerInput>().DisablePlayerInput();
        Player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        Player.GetComponentInChildren<Animator>().SetTrigger("Died");
        Player.GetComponentInChildren<Animator>().SetBool("IsDead", Player.IsDead);
    }

    public void OnPlayerRespawned()
    {
        health.RuntimeValue = health.InitialValue;
        mana.RuntimeValue = mana.InitialValue;
        disableHits = false;
        Player.IsDead = false;
        Player.GetComponent<PlayerBounds>().enabled = true;
        Player.GetComponent<PlayerInput>().EnablePlayerInput();
        Player.GetComponentInChildren<Animator>().SetBool("IsDead", Player.IsDead);
    }

    IEnumerator haltMovement()
    {
        Player._canMove = false;
        yield return new WaitForSeconds(0.5f);
        Player._canMove = true;
    }

}

