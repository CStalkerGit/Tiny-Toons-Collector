using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingAI : BaseAI
{
    public bool waitForPlayer;
    public bool avoidPits;
    
    bool moveToRight;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        moveToRight = actor.FacingRight;
    }

    void FixedUpdate()
    {
        if (waitForPlayer)
        {
            if (Mathf.Abs(transform.position.x - Player.LastPosition.x) < 6) waitForPlayer = false;
            return;
        }

        if (actor.WasBlockedX()) moveToRight = !moveToRight;

        if (avoidPits && actor.physics.OnGround)
        {
            if (CollisionGrid.IsPitAhead(actor.entity, actor.FacingRight))
                moveToRight = !moveToRight;
        }

        actor.Move(moveToRight ? 1 : -1);
    }
}
