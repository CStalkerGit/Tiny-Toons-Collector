using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Entity entity;
    Actor actor;

    private static Player ptr = null;

   void Awake()
    {
        ptr = this;
        entity = GetComponent<Entity>();
        actor = GetComponent<Actor>();
    }

    void OnDestroy()
    {
        ptr = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Stats.health = actor.health;
    }

    public static bool IsCollision(Entity target)
    {
        if (ptr == null) return false;
        return ptr.entity.IsCollision(target); 
    }

    public static bool IsTargetWasStomped(Entity target)
    {
        if (ptr == null) return false;
        return ptr.entity.PrevBottomCoord > target.TopCoord;
    }

    public static void GiveFrag()
    {
        if (ptr) ptr.actor.physics.velocity.y = 7f;
    }

    public static void TakeDamage()
    {
        if (ptr == null) return;
        ptr.actor.TakeDamage();
    }
}
