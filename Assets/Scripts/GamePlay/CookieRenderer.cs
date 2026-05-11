using System;
using UnityEngine;

public class CookieRenderer : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    public Animator Animator => _animator;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSpriteLocation(CookieData data)
    {
        Vector3 newPosition = new();
        newPosition.y = data.Type switch
        {
            CookieType.Pirate => 0,
            CookieType.Cherry => -0.21f,
            CookieType.Hero => -0.21f,
            _ => throw new ArgumentOutOfRangeException(),
        };

        transform.localPosition = newPosition;
    }
}
