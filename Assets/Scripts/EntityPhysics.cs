using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class EntityPhysics : MonoBehaviour
{
    // data
    public Vector3 velocity;
    public bool bouncing;

    // components
    Entity entity = null;

    // physics
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
        Vector3 position = transform.position;

        AlignRect align = new AlignRect();

        // axis Y
        OnGround = false;
        position.y = newPosition.y;
        if (CollisionGrid.IsCollision(entity, position, ref align))
        {
            position.y = transform.position.y;          
            if (velocity.y < 0) OnGround = true;
            velocity.y = 0;
        }

        // axis X
        position.x = newPosition.x;
        if (CollisionGrid.IsCollision(entity, position, ref align))
        {
            position.x = transform.position.x;
            velocity.x = 0;
        }

        transform.position = position;

        // gravity
        velocity.y -= CollisionGrid.Gravity * Time.deltaTime;
        if (velocity.y < -20) velocity.y = -20;

        // deceleration
        if (deceleration)
        {
            if (OnGround)
                velocity.x = velocity.x * 0.80f;
            else
                velocity.x = velocity.x * 0.95f;
            if (Mathf.Abs(velocity.x) < CollisionGrid.MinStep) velocity.x = 0;
        }
    }
}
