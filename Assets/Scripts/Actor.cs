using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(EntityPhysics))]
public class Actor : MonoBehaviour
{
    public int health = 1;
    public bool keepBody = false;

    public float acceleration = 2.5f;
    public float maxSpeed = 3.5f;
    public float jumpHeight = 2f;

    [System.NonSerialized]
    public Entity entity;
    [System.NonSerialized]
    public EntityPhysics physics;

    // moving
    float move;
    bool jump;

    float statusTimer = -1;

    // Start is called before the first frame update
    void Start()
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
            //add sound
            physics.velocity.y = Mathf.Sqrt(2 * CollisionGrid.Gravity * jumpHeight);
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

    public void StopJumping()
    {
        //if (physics.velocity.y > 0) physics.velocity.y = 0;
    }

    public bool WasBlocked()
    {
        bool result = physics.Blocked;
        physics.Blocked = false;
        return result;
    }

    public void TakeDamage(Entity attacker)
    {
        if (IsInvulnerable) return;
        if (IsDown) return;

        FacingRight = attacker.transform.position.x > transform.position.x;

        health--;
        if (health <= 0)
        {
            if (!keepBody) Destroy(gameObject);
            IsDown = true;
        }

        WasHit = true;
        IsInvulnerable = true;
        statusTimer = 1.0f;
    }

    public void Move(float v) => move = v;
    public void Jump() => jump = true;
    public void AddForce(Vector3 force) => physics.velocity += force;
    public bool FacingRight { get; private set; } = true;
    public bool Moving { get; private set; }
    public bool WasHit { get; private set; } = false;
    public bool IsInvulnerable { get; private set; } = false;
    public bool IsDown { get; private set; } = false;
}
