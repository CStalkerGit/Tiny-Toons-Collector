using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class AI : MonoBehaviour
{
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
        actor.Move(moveToRight? 1 : -1);
    }
}
