using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(EntityPhysics))]
public class Actor : MonoBehaviour
{
    public float acceleration = 2.5f;
    public float maxSpeed = 3.5f;
    public float jumpHeight = 2f;

    Entity entity;
    [System.NonSerialized]
    public EntityPhysics physics;

    // moving
    float move;
    bool jump;

    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity>();
        physics = GetComponent<EntityPhysics>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horSpeed = Mathf.Abs(physics.velocity.x);
        float newVelocity = physics.velocity.x + move * acceleration * Time.deltaTime;

        if (horSpeed < maxSpeed) // если текущая скорость меньше максимальной
            physics.velocity.x = Mathf.Clamp(newVelocity, -maxSpeed, maxSpeed); // ограничиваем ее максимальной скоростью
        else // иначе ограничиваем ее текущей скоростью
            physics.velocity.x = Mathf.Clamp(newVelocity, -horSpeed, horSpeed);

        // disable slowing down when moving
        physics.deceleration = Mathf.Abs(move) < 0.01f || horSpeed > maxSpeed;

        if (jump && physics.OnGround)
            physics.velocity.y = Mathf.Sqrt(2 * CollisionGrid.Gravity * jumpHeight);
        jump = false;    
    }

    public void StopJumping()
    {
        if (physics.velocity.y > 0) physics.velocity.y = 0;
    }

    public void Move(float v) => move = v;
    public void Jump() => jump = true;
    public void AddForce(Vector3 force) => physics.velocity += force;
}
