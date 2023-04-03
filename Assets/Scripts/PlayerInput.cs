using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class PlayerInput : MonoBehaviour
{
    Actor actor;

    bool userPressJump = false;
    bool actorIsJumping = false;
    float originPosY;

    void Awake()
    {
        actor = GetComponent<Actor>();
    }

    void Update()
    {
        if (BlackScreen.InProcess) return;

        if (Input.GetButton("Jump"))
            userPressJump = true;

        //actor.Move(-1);
        actor.Move(Input.GetAxisRaw("Horizontal"));
    }

    void FixedUpdate()
    {
        if (userPressJump && actor.physics.OnGround)
        {
            actor.Jump();
            originPosY = transform.position.y;
            actorIsJumping = true;
            Game.JumpEffect();
        }

        if (actorIsJumping && !userPressJump && originPosY < transform.position.y - 1.1f)
        {
            actor.StopJumping();
            actorIsJumping = false;
        }

        userPressJump = false;
    }
}
