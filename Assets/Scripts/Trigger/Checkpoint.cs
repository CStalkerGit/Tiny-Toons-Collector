using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class Checkpoint : MonoBehaviour
{
    public Sprite activated;
    public Sprite waiting;

    Entity entity;

    bool active = false;
    Vector3 origin;

    void Awake()
    {
        entity = GetComponent<Entity>();

        origin = transform.position;
    }

    void FixedUpdate()
    {
        if (!active && Player.IsCollision(entity))
        {
            active = true;
            Data.lastCheckpoint = origin;
            Data.startFromCheckpoint = true;
            GetComponent<SpriteRenderer>().sprite = activated;
        }
    }
}
