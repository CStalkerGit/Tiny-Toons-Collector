using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class Actor2D : MonoBehaviour
{
    public float speed = 3;
    public float jumpHeight = 3;
    public Text debug;

    public bool IsGrounded { get; private set; }
    public Vector2 Position => transform.position;

    BoxCollider2D boxCollider;
    Rigidbody2D rb;
    Vector2 motion;
    bool jump;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(motion.x * speed, rb.velocity.y); 

        if (jump) //&& IsGrounded
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            IsGrounded = false;
        }
        jump = false;
    }

    public void Move(float x)
    {
        motion.x = x;
    }

    public void Jump()
    {
        jump = true;
    }

    bool IsCollision(Vector2 move)
    {
        var hits = Physics2D.OverlapBoxAll(Position + boxCollider.offset + move, boxCollider.size, 0);
        foreach (var hit in hits)
        {
            if (hit == boxCollider) continue;
            return true;
        }

        return false;
    }
}
