using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform follow;
    public Transform cam1;
    public Transform cam2;

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

    public static void RestrictRect(int x1, int y1)
    {
        ptr.x1 = x1;
        //ptr.x2 = x2;
        ptr.y1 = y1;
        //ptr.y2 = y2;
    }

    public static void RestrictRect(Vector3 pos)
    {
        int x1 = Mathf.FloorToInt(pos.x);
        int y1 = Mathf.FloorToInt(pos.y);
        RestrictRect(x1, y1);
    }
}
