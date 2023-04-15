using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public Transform exit;
    public CameraRest camera1;
    public CameraRest camera2;

    int tx, ty;

    // Start is called before the first frame update
    void Start()
    {
        tx = Mathf.FloorToInt(transform.position.x);
        ty = Mathf.FloorToInt(transform.position.y);
    }

    void FixedUpdate()
    {
        int px = Mathf.FloorToInt(Player.LastPosition.x);
        int py = Mathf.FloorToInt(Player.LastPosition.y);

        if (tx == px && ty == py)
        {
            //Debug.Log("trigger!");
            if (Player.pressedDown && exit)
            {
                Player.Teleport(exit.transform.position);

                if (camera1)
                {
                    CameraControl.RestrictRect(camera1, camera2);
                }
            }
        }
    }
}
