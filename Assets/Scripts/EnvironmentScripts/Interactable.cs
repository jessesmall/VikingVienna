using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public abstract class Interactable : MonoBehaviour {

    protected Canvas _uiCanvas;
    [TextArea(1, 10)]
    public string UIString;

    [HideInInspector]
    public bool playerNearby;
    protected GameObject _player;

    public AudioClip InteractableSound;
    protected AudioSource _audioSource;

    [TextArea(1,10)]
    public string InteractionText;
    public float DisplayTime;

    public FontStyle fontStyle = FontStyle.Normal;

    public virtual void Start()
    {
        _uiCanvas = GetComponentInChildren<Canvas>();
        _uiCanvas.GetComponentInChildren<Text>().text = UIString;
        _uiCanvas.enabled = false;
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            _uiCanvas.transform.position = new Vector2(transform.position.x, player.transform.position.y);
            _uiCanvas.enabled = true;
            playerNearby = true;
            _player = player.gameObject;
            player.InteractableObjects.Add(this);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            _uiCanvas.enabled = false;
            _player = null;
            playerNearby = false;
            player.InteractableObjects.Remove(this);
        }
    }

    public virtual void Interact()
    {
        if(!string.IsNullOrEmpty(InteractionText))
            TextBoxInteraction.Instance.ShowText(InteractionText, DisplayTime, fontStyle);
        if (InteractableSound != null)
            _audioSource.PlayOneShot(InteractableSound);
    }
}
