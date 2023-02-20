using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class PlayerInput : MonoBehaviour
{
    Actor actor;

    void Awake()
    {
        actor = GetComponent<Actor>();
    }

    void Update()
    {
        if (Input.GetButton("Jump"))
            actor.Jump();
        else
            actor.StopJumping();


        actor.Move(Input.GetAxisRaw("Horizontal"));
        if (Input.GetKeyDown(KeyCode.F)) actor.AddForce(new Vector3(10, 0, 0));
    }
}
