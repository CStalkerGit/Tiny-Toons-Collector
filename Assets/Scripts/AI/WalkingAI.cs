using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class WalkingAI : BaseAI
{
    public bool avoidPits;

    Actor actor;
    bool moveToRight;

    void Awake()
    {
        actor = GetComponent<Actor>();
        moveToRight = false;
    }

    void FixedUpdate()
    {
        if (actor.WasBlocked()) moveToRight = !moveToRight;

        if (avoidPits && actor.physics.OnGround)
        {
            if (CollisionGrid.IsPitAhead(actor.entity, actor.FacingRight))
                moveToRight = !moveToRight;
        }

        actor.Move(moveToRight ? 1 : -1);
    }
}
