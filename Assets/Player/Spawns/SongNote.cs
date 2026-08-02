using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class SongNote : MonoBehaviour
{
    public List<Sprite> sprites;
    [HideInInspector] public Vector3 velocity = Vector3.zero;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [SerializeField] protected float speed = 5f;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        if (sprites != null && sprites.Count > 0 && spriteRenderer != null)
        {
            spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count)];
        }
    }

    protected virtual void Update()
    {
        transform.position += speed * Time.deltaTime * velocity;

        if (ShouldBeDestroyed())
        {
            Destroy(gameObject);
        }
    }

    public abstract bool ShouldBeDestroyed();
}