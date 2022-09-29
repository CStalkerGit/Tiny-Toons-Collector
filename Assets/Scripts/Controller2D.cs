using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    public bool IsGrounded {get; private set;}
    public Vector2 Position { get; private set; }
    public Text debug;

    BoxCollider2D boxCollider;
    Vector2 velocity = Vector3.zero;
    Vector2 motion = Vector3.zero;
    Vector2 lastPosition;
    float lerp = 0f;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        SetPosition(transform.position);
    }

    void Update()
    {
        if (transform.hasChanged) SetPosition(transform.position);

        lerp += Time.deltaTime;
        debug.text = $"{lerp / Time.fixedDeltaTime}";
        transform.position = Vector2.Lerp(lastPosition, Position, lerp / Time.fixedDeltaTime);
        transform.hasChanged = false;
    }

    void FixedUpdate()
    {
        lastPosition = transform.position;

        if (IsGrounded && velocity.y < 0) velocity.y = 0f;

        // Changes the height position of the player..
        //if (Input.GetButtonDown("Jump") && groundedPlayer)
        //{
        //    playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        //}

        // moving
        var speed = Mathf.Abs(motion.x);
        velocity.x = motion.x;
        motion = Vector2.zero;

        // gravity
        velocity.y += Physics2D.gravity.y * Time.deltaTime;

        IsGrounded = false;

        var hits = BoxCastAll(velocity.x > 0 ? Vector2.right : Vector2.left, Mathf.Abs(velocity.x) * Time.deltaTime);
        foreach (var hit in hits)
        {
            if (hit.collider == boxCollider) continue;

            velocity.x = 0;
            //IsGrounded = true;
            if (hit.normal.x < 0)
            {
                velocity.Set(hit.normal.y, -hit.normal.x);
                velocity *= speed;
            }
        }

        hits = BoxCastAll(velocity.y > 0 ? Vector2.up : Vector2.down, Mathf.Abs(velocity.y) * Time.deltaTime);
        foreach (var hit in hits)
        {
            if (hit.collider == boxCollider) continue;

            velocity.y = 0;
            IsGrounded = true;
        }

        Position = (Vector2)transform.position + velocity * Time.deltaTime;
        lerp = 0;
        //transform.position = position;
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
        lastPosition = position;
        Position = position;
    }

    public void Move(Vector3 motion)
    {
        
    }

    public void Move(float axisX)
    {
        motion.Set(axisX, 0);
    }

    RaycastHit2D[] BoxCastAll(Vector2 direction, float distance)
    {
        return Physics2D.BoxCastAll((Vector2)transform.position + boxCollider.offset, boxCollider.size, 0, direction, distance);
    }
}
