using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingAI : BaseAI
{
    public float jumpingTime = 3.0f;

    float timer;

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= jumpingTime)
        {
            timer = 0;
            actor.Jump();
        }

        actor.FaceTo(Player.LastPosition);
    }
}
