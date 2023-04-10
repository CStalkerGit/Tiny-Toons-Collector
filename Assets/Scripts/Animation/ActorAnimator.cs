using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteAnimator))]
public class ActorAnimator : MonoBehaviour
{
    public Actor actor;

    public SpriteAnimation staying;
    public SpriteAnimation walking;
    public SpriteAnimation down;

    public Sprite jumping;
    public Sprite falling;
    public Sprite hit;

    // components
    SpriteAnimator animator;
    SpriteRenderer spriteRenderer;

    float invulnerability;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<SpriteAnimator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // direction
        spriteRenderer.flipX = !actor.FacingRight;
        
        // flashing
        if (actor.IsInvulnerable && !actor.WasHit)
        {
            invulnerability -= Time.deltaTime;
            if (invulnerability < 0)
            {
                invulnerability = 0.05f;
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }
        }
        else if (!spriteRenderer.enabled) spriteRenderer.enabled = true;

        if (actor.IsDown)
        {
            animator.SetAnimation(down);
            return;
        }

        if (actor.WasHit)
            animator.SetSprite(hit);
        else
        {
            // on ground
            if (actor.physics.OnGround)
            {
                if (actor.Moving)
                    animator.SetAnimation(walking);
                else
                    animator.SetAnimation(staying);
            }
            else
            {
                if (actor.physics.velocity.y > 0.25f)
                    animator.SetSprite(jumping);
                else
                    animator.SetSprite(falling);
            }
        }
    }
}
