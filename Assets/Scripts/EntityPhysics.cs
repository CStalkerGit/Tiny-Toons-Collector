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
            align.Reset(entity, entity.pos);
            if (CollisionGrid.IsCollision(entity, ref align))
            {
                //entity.pos.y = transform.position.y;
                if (velocity.y <= 0)
                    entity.pos.y = align.top + entity.rh + CollisionGrid.MinStep;
                else
                    entity.pos.y = align.bottom - entity.rh - CollisionGrid.MinStep;

                if (velocity.y <= 0) OnGround = true;
                velocity.y = 0;
            }
        }

        // axis X
        if (Mathf.Abs(newPosition.x - entity.pos.x) > CollisionGrid.MinStep)
        {
            entity.pos.x = newPosition.x;
            align.Reset(entity, entity.pos);
            if (CollisionGrid.IsCollision(entity, ref align))
            {
                //entity.pos.x = transform.position.x;
                if (velocity.x < 0)
                    entity.pos.x = align.right + entity.rw + CollisionGrid.MinStep;
                else
                    entity.pos.x = align.left - entity.rw - CollisionGrid.MinStep;

                velocity.x = 0;
            }
        }

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
}
