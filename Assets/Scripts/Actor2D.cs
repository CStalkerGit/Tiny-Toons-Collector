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

        velocity.x = motion.x * speed;

        if (!isMoving && isOnSlope && !isJumping)
        {
            if (velocity.y > 0) velocity.y = 0;
        }

        // on slope
        if (isMoving && isOnSlope && !isJumping)
        {
            velocity.Set(-motion.x * speed * perp.x, -motion.x * speed * perp.y);
            //if (velocity.y > 0 && slopeAngle > maxSlopeAngle) velocity.y = 0;
        }

        if (isMoving && !isOnSlope && !isJumping && velocity.y > 0)
        {
           velocity.y = -1;
        }

        // jumping
        if (inputJump && IsGrounded && !isJumping)
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
