using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugStats : MonoBehaviour
{
    public Text text;
    public EntityPhysics entity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        text.text = $"speed = {entity.velocity.x} {entity.velocity.y}\n" +
            $"coord = {entity.transform.position.x} {entity.transform.position.y}";
    }
}
