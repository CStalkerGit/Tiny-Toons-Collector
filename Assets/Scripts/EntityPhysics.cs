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
    public bool OnSlope { get; private set; }

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
        Vector3 moving = velocity * Time.deltaTime;
        entity.pos = transform.position;

        bool blockedY = ProcessMovingAxisY(moving.y);
        bool blockedX = ProcessMovingAxisX(moving.x);

        float distanceX = Mathf.Abs(moving.x);
        if (OnSlope && distanceX >= CollisionGrid.MinStep) 
        {
            // redirect
            if (blockedX)
            {
                blockedX = ProcessMovingAxisY(distanceX);
                ProcessMovingAxisX(moving.x);
                ProcessMovingAxisY(-distanceX);
            }
            else
            {
                //ProcessMovingAxisX(moving.x);
                ProcessMovingAxisY(-distanceX);
            }
            OnGround = true;
            OnSlope = true;
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
            float drag = 1f;
            if (OnGround) drag = 3.5f;

            velocity.x = velocity.x * (1 - Time.deltaTime * drag);
            if (Mathf.Abs(velocity.x) < 0.1f) velocity.x = 0;
        }
    }

    bool ProcessMovingAxisY(float y)
    {
        if (Mathf.Abs(y) < CollisionGrid.MinStep) return false;

        AlignRect rect = new AlignRect();

        OnGround = false;
        OnSlope = false;
        entity.pos.y += y;
        if (CollisionGrid.IsCollision(entity, ref rect))
        {
            if (y < 0)
            {
                entity.pos.y = rect.top + entity.rh + CollisionGrid.MinStep;
                OnGround = true;
                OnSlope = rect.isSlope;
            }
            else
                entity.pos.y = rect.bottom - entity.rh - CollisionGrid.MinStep;
            
            return true;
        }
        return false;
    }

    bool ProcessMovingAxisX(float x)
    {
        if (Mathf.Abs(x) < CollisionGrid.MinStep) return false;

        bool right = x > 0;
        AlignRect rect = new AlignRect();

        entity.pos.x += x;
        if (CollisionGrid.IsCollision(entity, ref rect))
        {
            if (right)
                entity.pos.x = rect.left - entity.rw - CollisionGrid.MinStep;
            else
                entity.pos.x = rect.right + entity.rw + CollisionGrid.MinStep;

            OnSlope = rect.isSlope;
            return true;
        }
        return false;
    }
}
