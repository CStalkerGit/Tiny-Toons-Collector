using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    public SpawnPoint exit;
    public bool userInput = true;

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
            if (Player.pressedDown || !userInput)
            {
                if (exit)
                {
                    Player.Teleport(exit.transform.position);
                    //CameraControl.Set(camera1, camera2);
                }
            }
        }
    }
}
