using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(EntityPhysics))]
public class Actor : MonoBehaviour
{
    public int health = 1;
    public bool FacingRight;
    public bool keepBody = false; 

    public float acceleration = 2.5f;
    public float maxSpeed = 3.5f;
    public float jumpHeight = 2f;

    public AudioClip jumpSound;

    [System.NonSerialized]
    public Entity entity;
    [System.NonSerialized]
    public EntityPhysics physics;

    // moving
    float move;
    bool jump;

    float statusTimer = -1;

    // Start is called before the first frame update
    void Awake()
    {
        entity = GetComponent<Entity>();
        physics = GetComponent<EntityPhysics>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool canMove = !WasHit && !IsDown;

        if (WasHit || IsInvulnerable)
        {
            statusTimer -= Time.deltaTime;
            if (statusTimer < 0)
            {
                if (WasHit)
                {
                    WasHit = false;
                    statusTimer = 1.5f; // Invulnerability timer
                }
                else
                    IsInvulnerable = false;
            }
        }

        if (!canMove) move = 0;
        float horSpeed = Mathf.Abs(physics.velocity.x);
        float newVelocity = physics.velocity.x + move * acceleration * Time.deltaTime;

        if (horSpeed < maxSpeed) // если текущая скорость меньше максимальной
            physics.velocity.x = Mathf.Clamp(newVelocity, -maxSpeed, maxSpeed); // ограничиваем ее максимальной скоростью
        else // иначе ограничиваем ее текущей скоростью
            physics.velocity.x = Mathf.Clamp(newVelocity, -horSpeed, horSpeed);

        // disable slowing down when moving
        physics.deceleration = Mathf.Abs(move) < 0.01f || horSpeed > maxSpeed
            || Mathf.Sign(move) != Mathf.Sign(physics.velocity.x); // включить замедление при резкой смене направления

        if (jump && physics.OnGround && canMove)
        {
            if (!CollisionGrid.IsCollision(entity, 0, 0.2f))
            {
                Game.PlayClip(jumpSound);
                physics.velocity.y = Mathf.Sqrt(2 * CollisionGrid.Gravity * jumpHeight);
            }
        }
        jump = false;

        if (-0.1f > move || move > 0.1f)
        {
            FacingRight = move > 0;
            Moving = true;
        }
        else
            Moving = false;
    }

    public void Teleport(Vector3 pos)
    {
        transform.position = pos;
        entity.pos = pos;
        physics.velocity.Set(0, 0, 0);
        move = 0;
    }

    public void StopJumping()
    {
        //if (physics.velocity.y > 0) physics.velocity.y = 0;
    }

    public void FaceTo(Vector3 position)
    {
        FacingRight = position.x > transform.position.x;
    }

    public bool WasBlockedX()
    {
        bool result = physics.BlockedX;
        physics.BlockedX = false;
        return result;
    }

    public bool WasBlockedY()
    {
        bool result = physics.BlockedY;
        physics.BlockedY = false;
        return result;
    }

    public void TakeDamage(Entity attacker)
    {
        if (IsInvulnerable) return;
        if (IsDown) return;

        WasHit = true;

        FacingRight = attacker.transform.position.x > transform.position.x;

        health--;
        if (health <= 0) Kill();

        if (health > 0)
        {
            IsInvulnerable = true;
            statusTimer = 1.0f;
        }
    }

    public void Kill()
    {
        if (IsDown) return;

        IsInvulnerable = false;
        health = 0;
        if (!keepBody) Destroy(gameObject);
        IsDown = true;
    }

    public void AddForceY(float y, float maxForce)
    {
        physics.velocity.y += y;
        physics.velocity.y = Mathf.Clamp(physics.velocity.y, -maxForce, maxForce);
    }

    public void Move(float v) => move = v;
    public void Jump() => jump = true;
    public void AddForce(Vector3 force) => physics.velocity += force;
    public void AddImpulse(Vector3 force) => physics.impulse += force;
    public bool Moving { get; private set; }
    public bool WasHit { get; private set; } = false;
    public bool IsInvulnerable { get; private set; } = false;
    public bool IsDown { get; private set; } = false;
}
