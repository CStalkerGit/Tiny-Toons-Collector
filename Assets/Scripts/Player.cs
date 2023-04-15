using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Player : MonoBehaviour
{
    Entity entity;
    Actor actor;

    private static Player ptr = null;

    public static Vector3 LastPosition;

    void Awake()
    {
        ptr = this;
        LastPosition = transform.position;
        entity = GetComponent<Entity>();
        actor = GetComponent<Actor>();
        BlackScreen.FadeOut();
    }

    void OnDestroy()
    {
        ptr = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LastPosition = actor.transform.position;
        Stats.health = actor.health;

        if (actor.IsDown) return;

        // unstuck
        var data = CollisionGrid.IsSpecialCollision(entity);
        if (data.isStuck) entity.pos.y += 0.01f;
        if (data.isSpike)
        {
            actor.Kill();
            EndScene();
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
        return ptr.entity.PrevBottomCoord > target.PrevTopCoord;
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
            ptr.EndScene();
        else
            Game.Hit(ptr.entity.pos);
    }

    void EndScene()
    {
        actor.physics.velocity.x = 0;
        actor.physics.velocity.y = 5;
        Game.Down(entity.pos);
        Game.EndScene(true);  
    }
}
