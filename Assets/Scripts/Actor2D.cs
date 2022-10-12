using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class Actor2D : MonoBehaviour
{
    const int GroundLayer = 1 << 9;

    public float speed = 3;
    public float jumpHeight = 3;
    public float maxSlopeAngle = 50;
    public PhysicsMaterial2D fullFriction;
    public PhysicsMaterial2D zeroFriction;

    public bool IsGrounded { get; private set; }
    public Vector2 Position => transform.position;

    Animator anim;
    Rigidbody2D rb;
    Vector2 motion;

    bool isFlipped;
    bool isMoving;
    bool isJumping;
    bool isWall;
    bool inputJump;

    // tmp
    ContactPoint2D[] contacts = new ContactPoint2D[4];
    bool isOnSlope = false;
    float slopeAngle = 0;
    Vector2 perp = Vector2.up;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        //var count = rb.Cast(Vector2.down, results, 1.0f);

        isMoving = true;
        if (motion.x > 0.01f) Flip(false); else if (motion.x < -0.01f) Flip(true); else isMoving = false;
        if (isMoving) rb.sharedMaterial = zeroFriction; else rb.sharedMaterial = fullFriction;

        if (isJumping)
        {
            if (rb.velocity.y <= 0) isJumping = false;
            // TODO change animation
        }

        CheckContacts();
        UpdateVelocity();
        UpdateDebugStats();

        // update animation
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetBool("IsGrounded", IsGrounded);
    }

    void CheckContacts()
    {
        int countContacts = rb.GetContacts(contacts);
        float dirSign = motion.x > 0 ? 1 : -1;
        float contactX = -1000 * dirSign;

        isWall = false;
        for (int i = 0; i < countContacts; i++)
        {
            var contact = contacts[i];

            float angle = Vector2.Angle(contact.normal, Vector2.up);
            if (contactX * dirSign < contact.point.x * dirSign) //slopeAngle < angle
            {
                contactX = contact.point.x;
                slopeAngle = angle;
                perp = Vector2.Perpendicular(contact.normal).normalized;
            }
            if (contact.normal.y == 0 && contact.normal.x * dirSign > 0) isWall = true;
        }

        isOnSlope = false;
        if (countContacts > 0)
        {
            IsGrounded = true;
            if (slopeAngle > 0) isOnSlope = true;
        }
        else
        {
            IsGrounded = false;
        }
    }

    void UpdateVelocity()
    {
        var velocity = rb.velocity;
        bool isSteepSlope = slopeAngle > maxSlopeAngle;

        if (isJumping)
        {
            velocity.x = motion.x * speed;
        }
        else
        {
            if (isMoving)
            {
                // on slope
                if (isOnSlope)
                {
                    if (!isSteepSlope) velocity.Set(-motion.x * speed * perp.x, -motion.x * speed * perp.y);
                }
                else
                {
                    velocity.x = motion.x * speed;
                    if (velocity.y > 0) velocity.y = -1;
                }
            }
            else
            {
                if (!isSteepSlope) velocity.x = 0;
                if (velocity.y > 0) velocity.y = 0;
            }
        }

        // jumping
        if (inputJump && IsGrounded && !isJumping && !isSteepSlope)
        {
            velocity.y = jumpHeight;
            IsGrounded = false;
            isJumping = true;
            anim.SetTrigger("Jump");
        }
        inputJump = false;

        rb.velocity = velocity;
    }

    void Flip(bool flip)
    {
        if (isFlipped != flip)
        {
            transform.localScale = new Vector3(flip ? -1 : 1, 1, 1);
            isFlipped = flip;
        }
    }

    void UpdateDebugStats()
    {
        DebugStats.Velocity = rb.velocity;
        DebugStats.IsJumping = isJumping;
        DebugStats.IsGrounded = IsGrounded;
        DebugStats.IsOnSlope = isOnSlope;
        DebugStats.IsWall = isWall;
        DebugStats.Angle = slopeAngle;
    }

    // public

    public void Move(float x)
    {
        motion.x = x;
    }

    public void Jump()
    {
        inputJump = true;
    }
}
