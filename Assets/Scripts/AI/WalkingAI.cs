using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingAI : BaseAI
{
    public bool avoidPits;
    
    bool moveToRight;

    protected override void Awake()
    {
        base.Awake();
        moveToRight = false;
    }

    void FixedUpdate()
    {
        if (actor.WasBlockedX()) moveToRight = !moveToRight;

        if (avoidPits && actor.physics.OnGround)
        {
            if (CollisionGrid.IsPitAhead(actor.entity, actor.FacingRight))
                moveToRight = !moveToRight;
        }

        actor.Move(moveToRight ? 1 : -1);
    }
}
