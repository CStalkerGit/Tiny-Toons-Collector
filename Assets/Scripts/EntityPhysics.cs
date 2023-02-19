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
        AlignRect align = new AlignRect();

        // axis Y
        if (Mathf.Abs(newPosition.y - entity.pos.y) > CollisionGrid.MinStep)
        {
            OnGround = false;
            entity.pos.y = newPosition.y;
            align.Reset(entity, newPosition);
            if (CollisionGrid.IsCollision(entity, ref align))
            {
                entity.pos.y = transform.position.y;
                if (velocity.y < 0)
                {
                    OnGround = true;
                    entity.pos.y = align.top + entity.rh + CollisionGrid.MinStep;
                }
                else
                {
                    //float alignY = align.bottom - entity.rh - CollisionGrid.MinStep;
                    // position.y = alignY;
                }
                velocity.y = 0;
            }
        }

        // axis X
        entity.pos.x = newPosition.x;
        align.Reset(entity, newPosition);
        if (CollisionGrid.IsCollision(entity, ref align))
        {
            entity.pos.x = transform.position.x;
            velocity.x = 0;
        }

        transform.position = entity.pos;

        // gravity
        velocity.y -= CollisionGrid.Gravity * Time.deltaTime;
        if (velocity.y < -20) velocity.y = -20;

        // deceleration
        if (deceleration)
        {
            const float airFriction = 0.965f;
            const float groundFriction = 0.8f;
            if (OnGround)
                velocity.x = velocity.x * groundFriction;
            else
                velocity.x = velocity.x * airFriction;
            if (Mathf.Abs(velocity.x) < CollisionGrid.MinStep) velocity.x = 0;
        }
    }
}
