﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor2D))]
public class PlayerInput : MonoBehaviour
{
    Actor2D actor;

    void Awake()
    {
        actor = GetComponent<Actor2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump")) actor.Jump();
        actor.Move(Input.GetAxisRaw("Horizontal"));
    }
}
