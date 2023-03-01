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
    public bool BlockedX { get; private set; }
    public bool BlockedY { get; private set; }

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
        Vector3 lastVelocity = velocity;
        Vector3 moving = velocity * Time.deltaTime;
        entity.pos = transform.position;

        ProcessMovingAxisY(moving.y);
        ProcessMovingAxisX(moving.x);

        float distanceX = Mathf.Abs(moving.x);
        if (OnSlope && distanceX >= CollisionGrid.MinStep) 
        {
            // slopes
            if (BlockedX)
            {
                Vector3 lastPos = entity.pos;
                float dist1 = Mathf.Abs(transform.position.x - entity.pos.x);
                ProcessMovingAxisY(distanceX);
                ProcessMovingAxisX(moving.x);
                float dist2 = Mathf.Abs(transform.position.x - entity.pos.x);
                //ProcessMovingAxisY(-distanceX);
                if (dist2 > dist1)
                {
                    BlockedX = false;
                    velocity = lastVelocity;
                }
                else
                {
                    entity.pos = lastPos;
                    BlockedX = true;
                }
            }
            else
            {
                //ProcessMovingAxisX(moving.x);
                ProcessMovingAxisY(-distanceX);
            }
            OnGround = true;
        }

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

    void ProcessMovingAxisY(float y)
    {
        if (Mathf.Abs(y) < CollisionGrid.MinStep) return;

        CollisionData data = new CollisionData();

        BlockedY = false;
        OnGround = false;
        entity.pos.y += y;
        if (CollisionGrid.IsCollision(entity, ref data))
        {
            if (y < 0)
            {
                entity.pos.y = data.top + entity.rh + CollisionGrid.MinStep;
                OnGround = true;
                if (data.isSlope) OnSlope = true;
            }
            else
                entity.pos.y = data.bottom - entity.rh - CollisionGrid.MinStep;

            velocity.y = 0;
            BlockedY = true;
        }
    }

    void ProcessMovingAxisX(float x)
    {
        if (Mathf.Abs(x) < CollisionGrid.MinStep) return;

        CollisionData data = new CollisionData();

        BlockedX = false;
        entity.pos.x += x;
        if (CollisionGrid.IsCollision(entity, ref data))
        {
            if (x > 0)
                entity.pos.x = data.left - entity.rw - CollisionGrid.MinStep;
            else
                entity.pos.x = data.right + entity.rw + CollisionGrid.MinStep;
            if (data.isSlope) OnSlope = true;

            velocity.x = 0;
            BlockedX = true;
        }
    }
}
