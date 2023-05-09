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
            Game.Poof(transform.position);
            if (Game.LastCheckpoint) Game.LastCheckpoint.Deactivate();
            Activate();
        }
    }

    public void Activate()
    {
        GetComponent<SpriteRenderer>().sprite = activated;
        active = true;
        Data.lastCheckpoint = origin;
        Data.startFromCheckpoint = true;
        Game.SetLastCheckpoint(this);
    }

    public void Deactivate()
    {
        GetComponent<SpriteRenderer>().sprite = waiting;
        active = false;
        if (Game.LastCheckpoint == this)
        {
            Data.startFromCheckpoint = false;
            Game.SetLastCheckpoint(null);
        }
    }
}
