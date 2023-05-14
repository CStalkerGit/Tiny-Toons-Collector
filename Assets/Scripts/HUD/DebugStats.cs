using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugStats : MonoBehaviour
{
    public Text text;
    public EntityPhysics entity;
    public static string debug;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        text.text = $"speed = {entity.velocity}\n" +
            $"coord = {entity.transform.position}\n" +
            //debug;
            $"OnGround = {entity.OnGround}, OnSlope = {entity.OnSlope}\n";// +
            //$"BlockedX = {entity.BlockedX}, BlockedY = {entity.BlockedY}";
    }
}
