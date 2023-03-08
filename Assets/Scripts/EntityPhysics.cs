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
        entity.prev_pos = transform.position;
        entity.pos = transform.position;
        Vector3 lastPosition = entity.pos;

        var stateY = ProcessMovingAxisY(moving.y);
        var stateX = ProcessMovingAxisX(moving.x);

        if (stateY != CollisionState.Pending)
        {
            OnGround = false;
            if (stateY != CollisionState.None)
            {
                if (velocity.y < 0) OnGround = true;                   
                velocity.y = 0;
            }
            OnSlope = (stateY == CollisionState.Slope && OnGround);
        }
 
        float distanceX = Mathf.Abs(moving.x);
        if (stateX != CollisionState.Pending) 
        {
            if (stateX == CollisionState.Slope)
            {
                ProcessMovingAxisY(distanceX);
                ProcessMovingAxisX(moving.x);
                ProcessMovingAxisY(-distanceX);
                if (Mathf.Abs(entity.pos.x - lastPosition.x) < distanceX / 2) velocity.x = 0;
            }
            else if (OnSlope && stateX == CollisionState.None)
            {
                //ProcessMovingAxisX(moving.x);
                ProcessMovingAxisY(-distanceX);
            }

            if (stateX == CollisionState.Wall) velocity.x = 0;
        }

        //if (stateY == CollisionState.Wall || stateY == CollisionState.Slope) velocity.y = 0;
        

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

    CollisionState ProcessMovingAxisY(float y)
    {
        if (Mathf.Abs(y) < CollisionGrid.MinStep) return CollisionState.Pending;

        CollisionRect data = new CollisionRect();

        entity.pos.y += y;
        if (CollisionGrid.IsCollision(entity, ref data))
        {
            if (y < 0)
                entity.pos.y = data.top + entity.rh + CollisionGrid.MinStep;
            else
                entity.pos.y = data.bottom - entity.rh - CollisionGrid.MinStep;

            return data.isSlope ? CollisionState.Slope : CollisionState.Wall;
        }

        return CollisionState.None;
    }

    CollisionState ProcessMovingAxisX(float x)
    {
        if (Mathf.Abs(x) < CollisionGrid.MinStep) return CollisionState.Pending;

        CollisionRect data = new CollisionRect();

        entity.pos.x += x;
        if (CollisionGrid.IsCollision(entity, ref data))
        {
            if (x > 0)
                entity.pos.x = data.left - entity.rw - CollisionGrid.MinStep;
            else
                entity.pos.x = data.right + entity.rw + CollisionGrid.MinStep;

            return data.isSlope ? CollisionState.Slope : CollisionState.Wall;
        }

        return CollisionState.None;
    }
}
