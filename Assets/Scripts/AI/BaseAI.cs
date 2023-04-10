﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class BaseAI : MonoBehaviour
{
    protected Actor actor;

    protected virtual void Awake()
    {
        actor = GetComponent<Actor>();
    }
}
