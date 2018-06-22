using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatedSpriteCollider : MonoBehaviour
{
    public bool isTrigger;
    public PhysicsMaterial2D _material;

    private SpriteRenderer spriteRenderer;
    public List<Sprite> spritesList;
    public Dictionary<int, PolygonCollider2D> spriteColliders;
    private bool _processing;

    private int _frame;
    public int Frame
    {
        get { return _frame; }
        set
        {
            Debug.Log(value + " " + _frame);
            if (value != _frame)
            {
                if (value > -1 && value < spriteColliders.Count)
                {
                    spriteColliders[_frame].enabled = false;
                    _frame = value;
                    spriteColliders[_frame].enabled = true;
                }
                else
                {
                    _processing = true;
                    StartCoroutine(AddSpriteCollider(spriteRenderer.sprite));
                }
            }
        }
    }

    private void OnEnable()
    {
        spriteColliders[Frame].enabled = true;
    }

    private void OnDisable()
    {
        spriteColliders[Frame].enabled = false;
    }

    private IEnumerator AddSpriteCollider(Sprite sprite)
    {
        spritesList.Add(sprite);
        int index = spritesList.IndexOf(sprite);
        PolygonCollider2D spriteCollider = gameObject.AddComponent<PolygonCollider2D>();
        spriteCollider.isTrigger = isTrigger;
        spriteCollider.sharedMaterial = _material;
        spriteColliders.Add(index, spriteCollider);
        yield return new WaitForEndOfFrame();
        Frame = index;
        _processing = false;
    }

    private void Awake()
    {
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();

        spritesList = new List<Sprite>();

        spriteColliders = new Dictionary<int, PolygonCollider2D>();

        Frame = spritesList.IndexOf(spriteRenderer.sprite);
    }

    private void Update()
    {
        Debug.Log(_processing);
        Debug.Log(Frame);
        if (!_processing)
            Frame = spritesList.IndexOf(spriteRenderer.sprite);
    }
}