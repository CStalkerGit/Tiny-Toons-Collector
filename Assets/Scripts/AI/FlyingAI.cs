﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAI : BaseAI
{
    public bool movingX;
    public float distanceY;
    public float speedY;
    public float accY;

    float originY;
    bool flyingDown = true;
    bool moveToRight;

    void Start()
    {
        originY = transform.position.y;
    }

    void FixedUpdate()
    {
        if (movingX)
        {
            actor.Move(moveToRight ? 1 : -1);
            if (actor.WasBlockedX()) moveToRight = !moveToRight;
        }
        else
        {
            actor.FaceTo(Player.LastPosition);
        }

        float y = accY * Time.deltaTime;
        if (flyingDown)
        {
            if (actor.WasBlockedY()) flyingDown = false;
            actor.AddForceY(-y, speedY);
            if (transform.position.y <= (originY - distanceY)) flyingDown = false; 
        }
        else
        {
            if (actor.WasBlockedY()) flyingDown = true;
            actor.AddForceY(y, speedY);
            if (transform.position.y >= originY) flyingDown = true;
        } 
    }
}
