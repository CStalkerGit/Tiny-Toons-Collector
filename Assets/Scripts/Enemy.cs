using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class Enemy : MonoBehaviour
{
    public int health = 1;

    Entity entity;

    void Awake()
    {
        entity = GetComponent<Entity>();
    }

    void FixedUpdate()
    {
        if (Player.IsCollision(entity))
        {
            if (Player.IsTargetWasStomped(entity))
            {
                EffectSpawner.Poof(transform.position);
                Destroy(gameObject);
                Player.GiveFrag();
            }
            else
            {
                Player.TakeDamage();
            }
        }
    }
}
