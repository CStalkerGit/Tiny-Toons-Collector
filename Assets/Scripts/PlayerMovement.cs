using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerMovement : MonoBehaviour
{
    public float runSpeed;

    float horizontalMove = 0f;

    Controller2D controller;

    void Awake()
    {
        controller = GetComponent<Controller2D>();
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        //if (Input.GetButtonDown("Jump")) jump = true;
        controller.Move(horizontalMove * runSpeed);
    }
}
