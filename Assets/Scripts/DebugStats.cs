using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugStats : MonoBehaviour
{
    DebugStats instance;
    Text stats;

    public static Vector2 Velocity { get; set; }
    public static bool IsJumping { get; set; }
    public static bool IsGrounded { get; set; }
    public static bool IsOnSlope { get; set; }
    public static float Angle { get; set; }

    void Awake()
    {
        instance = this;
        stats = GetComponent<Text>();
    }

    void FixedUpdate()
    {
        stats.text = $"Velocity = {Velocity}\n"
                   + $"IsJumping = {IsJumping}\n"
                   + $"OnGround = {IsGrounded}\n"
                   + $"OnSlope = {IsOnSlope}\n"
                   + $"Angle = {Angle}\n";
    }
}
