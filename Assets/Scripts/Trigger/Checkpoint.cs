using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class Checkpoint : MonoBehaviour
{
    public Sprite activated;
    public Sprite waiting;

    Entity entity;

    void Awake()
    {
        entity = GetComponent<Entity>();
    }

    void FixedUpdate()
    {
        if (Player.IsCollision(entity))
        {

        }
    }
}
