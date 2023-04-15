using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform follow;
    public CameraRest cam1;
    public CameraRest cam2;

    int x1, y1, x2, y2;

    static CameraControl ptr;

    void Awake()
    {
        ptr = this;
        Set(cam1, cam2);
    }

    // Update is called once per frame
    void Update()
    {
        if (!follow) return;

        Vector2 pos = follow.position;

        pos.x = Mathf.Clamp(pos.x, x1, x2);
        pos.y = Mathf.Clamp(pos.y, y1, y2);

        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }

    public static void Set(CameraRest cam1, CameraRest cam2)
    {
        if (!cam1) return;

        ptr.x1 = cam1.X + 5;
        ptr.x2 = (cam2 ? cam2.X : cam1.X) - 4;
        if (ptr.x2 < ptr.x1) ptr.x2 = ptr.x1;

        ptr.y1 = cam1.Y + 4;
        ptr.y2 = (cam2 ? cam2.Y : cam1.Y) - 3;
        if (ptr.y2 < ptr.y1) ptr.y2 = ptr.y1;

        Camera.main.backgroundColor = cam1.backgroundColor;
    }
}
