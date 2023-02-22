using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class EntityPhysics : MonoBehaviour
{
    // data
    public bool bouncing;

    // components
    Entity entity = null;

    // physics
    [System.NonSerialized]
    public Vector3 velocity;
    [System.NonSerialized]
    public bool deceleration;
    public bool OnGround { get; private set; }

    void Awake()
    {
        // components
        entity = GetComponent<Entity>();

        deceleration = true;
        OnGround = false;
    }

    void LateUpdate()
    {
        //Vector2 lerp = Vector2.Lerp(smoothBegin, smoothEnd, Engine.alpha);
        //transform.position = new Vector3(lerp.x, lerp.y, 0);
    }

    void FixedUpdate()
    {
        Vector3 newPosition = transform.position + velocity * Time.deltaTime;

        entity.pos = transform.position;
        AlignRect rect = new AlignRect();

        // axis Y
        bool blockedY = IsCollisionAxisY(newPosition.y, ref rect);
        bool blockedX = IsCollisionAxisX(newPosition.x, ref rect);

        if (blockedY)
        {
            float slope = velocity.x > 0 ? rect.leftSlope : rect.rightSlope;
            if (slope > 0.1f)
            {
                // redirect
                newPosition = transform.position + new Vector3(velocity.x, velocity.x * 1, 0) * Time.deltaTime;
                blockedX = IsCollisionAxisX(newPosition.x, ref rect);
                blockedY = IsCollisionAxisY(newPosition.y, ref rect);
                OnGround = true;
            }
        }

        if (blockedX)
        {
            float slope = velocity.x > 0 ? rect.leftSlope : rect.rightSlope;
            if (slope > 0.1f)
            {
                // redirect
                newPosition = transform.position + new Vector3(velocity.x, velocity.x * slope, 0) * Time.deltaTime;
                blockedY = IsCollisionAxisY(newPosition.y, ref rect);
                blockedX = IsCollisionAxisX(newPosition.x, ref rect);
                OnGround = true;
            }
        }

        if (blockedY) velocity.y = 0;
        if (blockedX) velocity.x = 0;

        transform.position = entity.pos;

        // gravity
        velocity.y -= CollisionGrid.Gravity * Time.deltaTime;
        if (velocity.y < -20) velocity.y = -20;

        // deceleration
        if (deceleration)
        {
            float drag = 3f;
            if (OnGround) drag = 10f;

            velocity.x = velocity.x * (1 - Time.deltaTime * drag);
            if (Mathf.Abs(velocity.x) < 0.1f) velocity.x = 0;
        }
    }

    bool IsCollisionAxisY(float y, ref AlignRect rect)
    {
        bool down = entity.pos.y >= y;
        // axis Y
        if (Mathf.Abs(y - entity.pos.y) < CollisionGrid.MinStep) return false;
       
        OnGround = false;
        entity.pos.y = y;
        if (CollisionGrid.IsCollision(entity, ref rect))
        {
            if (down)
                entity.pos.y = rect.top + entity.rh + CollisionGrid.MinStep;
            else
                entity.pos.y = rect.bottom - entity.rh - CollisionGrid.MinStep;

            if (velocity.y <= 0) OnGround = true;
            //velocity.y = 0;
            return true;
        }
        else
            return false;
    }

    bool IsCollisionAxisX(float x, ref AlignRect rect)
    {
        bool right = entity.pos.x < x;
        // axis X
        if (Mathf.Abs(x - entity.pos.x) < CollisionGrid.MinStep) return false;

        entity.pos.x = x;
        if (CollisionGrid.IsCollision(entity, ref rect))
        {
            //entity.pos.x = transform.position.x;
            if (right)
                entity.pos.x = rect.left - entity.rw - CollisionGrid.MinStep;
            else
                entity.pos.x = rect.right + entity.rw + CollisionGrid.MinStep;

            //velocity.x = 0;
            return true;
        }
        else
            return false;
    }
}
