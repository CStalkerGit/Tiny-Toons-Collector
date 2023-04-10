using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteAnimator))]
public class EnemyAnimator : MonoBehaviour
{
    public Actor actor;

    public Sprite staying;
    public SpriteAnimation walking;
    public Sprite jumping;

    // components
    SpriteAnimator animator;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        animator = GetComponent<SpriteAnimator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // direction
        spriteRenderer.flipX = !actor.FacingRight;

        if (jumping)
        {
            if (actor.physics.OnGround)
                animator.SetSprite(staying);
            else
                animator.SetSprite(jumping);
        }
    }
}
