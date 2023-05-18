using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionState
{
    Pending, None, Wall, Slope
}

[RequireComponent(typeof(Entity))]
public class EntityPhysics : MonoBehaviour
{
    // data
    public bool gravity = true;
    public bool bouncing = false;

    // components
    Entity entity = null;

    // physics
    [System.NonSerialized]
    public Vector3 velocity;
    [System.NonSerialized]
    public Vector3 impulse;
    [System.NonSerialized]
    public bool deceleration;
    public bool OnGround { get; private set; }
    public bool OnSlope { get; private set; }
    public bool BlockedX { get; set; }
    public bool BlockedY { get; set; }

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
        Vector3 moving = (velocity + impulse) * Time.deltaTime;
        impulse = Vector3.zero;
        entity.prev_pos = transform.position;
        entity.pos = transform.position;

        var stateY = ProcessMovingAxisY(moving.y);
        var stateX = ProcessMovingAxisX(moving.x);

        Vector3 lastPosition = entity.pos;

        if (stateY != CollisionState.Pending)
        {
            OnGround = false;
            if (stateY != CollisionState.None)
            {
                BlockedY = true;
                if (velocity.y < 0) OnGround = true;
                if (gravity) velocity.y = -1; else velocity.y = 0;
            }
            OnSlope = (stateY == CollisionState.Slope && OnGround);
        }
 
        float distanceX = Mathf.Abs(moving.x);
        if (stateX != CollisionState.Pending) 
        {
            if (stateX == CollisionState.Slope)
            {
                var tmp1 = ProcessMovingAxisY(distanceX);
                var tmp2 = ProcessMovingAxisX(moving.x);
                var tmp3 = ProcessMovingAxisY(-distanceX);
                if (Mathf.Abs(entity.pos.x - lastPosition.x) < CollisionGrid.MinStep / 2) // distanceX / 2
                {
                    velocity.x = 0;
                    BlockedX = true;
                }
            }
            else if (OnSlope) //&& stateX == CollisionState.None
            {
                //ProcessMovingAxisX(moving.x);
                ProcessMovingAxisY(-distanceX);
            }

            if (stateX == CollisionState.Wall)
            {
                velocity.x = 0;
                BlockedX = true;
            }
        }

        //if (stateY == CollisionState.Wall || stateY == CollisionState.Slope) velocity.y = 0;
        transform.position = entity.pos;

        // gravity
        if (gravity)
        {
            velocity.y -= CollisionGrid.Gravity * Time.deltaTime;
            if (velocity.y < -20) velocity.y = -20;
        }

        // deceleration
        if (deceleration)
        {
            float drag = 3f;
            if (OnGround) drag = 11f;

            velocity.x = velocity.x * (1 - Time.deltaTime * drag);
            if (Mathf.Abs(velocity.x) < 0.1f) velocity.x = 0;
        }
    }

    CollisionState ProcessMovingAxisY(float y)
    {
        if (Mathf.Abs(y) < CollisionGrid.MinStep) return CollisionState.Pending;

        float lastY = entity.pos.y;

        entity.pos.y += y;
        var data = CollisionGrid.IsCollision(entity);
        if (data.isCollision)
        {
            if (y < 0)
            {
                entity.pos.y = data.top + entity.rh + CollisionGrid.MinStep;
                if (entity.pos.y >= lastY) entity.pos.y = lastY;
            }
            else
            {
                entity.pos.y = data.bottom - entity.rh - CollisionGrid.MinStep;
                if (entity.pos.y <= lastY) entity.pos.y = lastY;
            }

            return data.isSlope ? CollisionState.Slope : CollisionState.Wall;
        }

        return CollisionState.None;
    }

    CollisionState ProcessMovingAxisX(float x)
    {
        if (Mathf.Abs(x) < CollisionGrid.MinStep) return CollisionState.Pending;

        float lastX = entity.pos.x;

        entity.pos.x += x;
        var data = CollisionGrid.IsCollision(entity);
        if (data.isCollision)
        {
            if (x > 0)
            {
                entity.pos.x = data.left - entity.rw - CollisionGrid.MinStep;
                if (entity.pos.x <= lastX) entity.pos.x = lastX;
            }
            else
            {
                entity.pos.x = data.right + entity.rw + CollisionGrid.MinStep;
                if (entity.pos.x >= lastX) entity.pos.x = lastX;
            }

            return data.isSlope ? CollisionState.Slope : CollisionState.Wall;
        }

        return CollisionState.None;
    }
}
