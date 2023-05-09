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
    public static bool pressedDown;

    void Awake()
    {
        ptr = this;
        entity = GetComponent<Entity>();
        actor = GetComponent<Actor>();

        if (Data.startFromCheckpoint)
        {
            transform.position = Data.lastCheckpoint;
            var checkpoints = FindObjectsOfType<Checkpoint>();
            foreach (var chk in checkpoints)
                if ((transform.position - chk.transform.position).sqrMagnitude < 0.2f)
                {
                    chk.Activate();
                    Data.lastCheckpoint = chk.transform.position; // HACK checkpoint may miss Awake()
                    if (chk.camera1) CameraControl.Set(chk.camera1, chk.camera2);
                    break;
                }
        }

        LastPosition = transform.position;
        entity.pos = transform.position;

        BlackScreen.FadeOut();

        actor.FacingRight = true;
    }

    void OnDestroy()
    {
        ptr = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LastPosition = actor.transform.position;
        Data.health = actor.health;

        if (actor.IsDown) return;

        // unstuck
        var data = CollisionGrid.IsSpecialCollision(entity);
        if (data.isStuck) entity.pos.y += 0.01f;
        if (data.isSpike)
        {
            actor.Kill();
            EndScene();
        }
        if (data.isExit)
        {
            //gameObject.SetActive(false);
            Game.EndScene(false);
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

    public static void EnemyWasStomped()
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
        {
            Debug.Log(ptr.entity.pos);
            Game.Hit(ptr.entity.pos);
        }
    }

    public static void RestoreHealth(int count)
    {
        if (ptr == null) return;
        ptr.actor.health += count;
        if (ptr.actor.health > Data.maxHealth) ptr.actor.health = Data.maxHealth;
    }

    void EndScene()
    {
        actor.physics.velocity.x = 0;
        actor.physics.velocity.y = 5;
        Game.DownSound(entity.pos);
        Game.EndScene(true);  
    }

    public static void Teleport(Vector3 pos)
    {
        if (ptr && ptr.actor) ptr.actor.Teleport(pos);
    }
}
