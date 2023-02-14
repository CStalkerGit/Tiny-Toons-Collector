using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class EntityPhysics : MonoBehaviour
{
    // data
    public float friction = 1.0f;
    public bool bouncing;

    // components
    Entity entity = null;

    // physics
    public bool OnGround { get; private set; }
    Vector3 velocity;

    void Awake()
    {
        // components
        entity = GetComponent<Entity>();

        OnGround = false;
    }

    void LateUpdate()
    {
        //Vector2 lerp = Vector2.Lerp(smoothBegin, smoothEnd, Engine.alpha);
        //transform.position = new Vector3(lerp.x, lerp.y, 0);
    }

    void FixedUpdate()
    {
        Vector3 oldPosition = transform.position;
        Vector3 newPosition = transform.position + velocity * Time.deltaTime;

        transform.position = newPosition;
        if (CollisionGrid.IsCollision(entity))
        {
            transform.position = oldPosition;
        }

        velocity.y -= CollisionGrid.Gravity * Time.deltaTime;
        if (velocity.y < -10) velocity.y = -10;
    }

    public void Move(int move, float speed)
    {

    }
}
