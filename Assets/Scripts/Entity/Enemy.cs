using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class Enemy : MonoBehaviour
{
    public int Points;
    public bool spikeImmunity;

    Entity entity;
    Actor actor;

    void Awake()
    {
        entity = GetComponent<Entity>();
        actor = GetComponent<Actor>();
    }

    void FixedUpdate()
    {
        // unstuck
        var data = CollisionGrid.IsSpecialCollision(entity);
        if (data.isStuck) entity.pos.y += 0.01f;
        if (data.isSpike && !spikeImmunity) actor.Kill();

        if (Player.IsCollision(entity))
        {
            if (Player.IsTargetWasStomped(entity))
            {
                actor.Kill();
                Game.Poof(transform.position);
                Data.points += Points;
                Player.EnemyWasStomped();
            }
            else
            {
                Player.TakeDamage(entity);
            }
        }
    }
}
