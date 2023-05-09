using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform follow;

    int x1, y1, x2, y2;

    static CameraControl ptr;

    void Awake()
    {
        ptr = this;
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

    public static void Set(CameraPosition cam)
    {
        if (!cam)
        {
            ptr.x1 = -9999;
            ptr.x2 = 9999;
            ptr.y1 = -9999;
            ptr.y2 = 9999;
            return;
        }

        ptr.x1 = cam.X1 + 5;
        ptr.x2 = cam.X2 - 4;
        if (ptr.x2 < ptr.x1) ptr.x2 = ptr.x1;

        ptr.y1 = cam.Y1 + 3;
        ptr.y2 = cam.Y2 - 3;
        if (ptr.y2 < ptr.y1) ptr.y2 = ptr.y1;

        Camera.main.backgroundColor = cam.GetBackgroundColor();
    }

    public static void Find(Vector3 pos)
    {
        var cameras = FindObjectsOfType<CameraPosition>();
        int cx1 = Mathf.FloorToInt(pos.x);
        int cx2 = Mathf.CeilToInt(pos.x);
        int cy1 = Mathf.FloorToInt(pos.y);
        int cy2 = Mathf.CeilToInt(pos.y);

        foreach (var cam in cameras)
        {
            if (cx1 >= cam.X1 && cx2 <= cam.X2)
                if (cy1 >= cam.Y1 && cy2 <= cam.Y2)
                {
                    Set(cam);
                    break;
                }
        }
    }
}
