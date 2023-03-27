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

        if (actor.IsDown) return;

        // unstuck
        var data = CollisionGrid.IsSpecialCollision(entity);
        if (data.isStuck) entity.pos.y += 0.01f;
        if (data.isSpike)
        {
            actor.Kill();
            KillEffect();
        }
    }

    public static bool IsCollision(Entity target)
    {
        if (ptr == null) return false;
        return ptr.entity.IsCollision(target); 
    }

    public static bool IsTargetWasStomped(Entity target)
    {
        if (ptr == null) return false;
        if (ptr.actor.WasHit || ptr.actor.IsDown) return false;
        return ptr.entity.PrevBottomCoord > target.TopCoord;
    }

    public static void GiveFrag()
    {
        if (ptr) ptr.actor.physics.velocity.y = 7f;
    }

    public static void TakeDamage(Entity attacker)
    {
        if (ptr == null) return;
        if (ptr.actor.IsInvulnerable || ptr.actor.IsDown) return;

        ptr.actor.TakeDamage(attacker);

        float x = 5 * (attacker.pos.x > ptr.transform.position.x ? -1 : 1);
        ptr.actor.physics.velocity = new Vector3(x, 5, 0);

        if (ptr.actor.IsDown)
            ptr.KillEffect();
        else
            EffectSpawner.Hit(ptr.entity.pos);
    }

    void KillEffect()
    {
        actor.physics.velocity.x = 0;
        actor.physics.velocity.y = 5;
        EffectSpawner.Down(entity.pos);
        BlackScreen.FadeInEffect(actor.transform.position);
    }
}
