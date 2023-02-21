using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ActorAnimator : MonoBehaviour
{
    public Actor actor;

    Animator animator;
    SpriteRenderer spriteRenderer;

    // animation flags
    bool right = true;
    bool onGround;
    bool movingUp;
    bool moving;

    // animations hashes
    int B_GROUND, B_UP, B_MOVING;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //F_SPEED = Animator.StringToHash("Speed");
        B_GROUND = Animator.StringToHash("OnGround");
        B_UP = Animator.StringToHash("Up");
        B_MOVING = Animator.StringToHash("Moving");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // direction
        if (right != actor.MovingRight)
        {
            right = actor.MovingRight;
            spriteRenderer.flipX = !right;
        }

        // on ground
        if (onGround != actor.physics.OnGround)
        {
            onGround = !onGround;
            animator.SetBool(B_GROUND, onGround);
        }

        // movingUp
        if (movingUp != (actor.physics.velocity.y > 0.25f))
        {
            movingUp = !movingUp;
            animator.SetBool(B_UP, movingUp);
        }

        // moving
        if (moving != (actor.Moving))
        {
            moving = !moving;
            animator.SetBool(B_MOVING, moving);
        }    
    }
}
