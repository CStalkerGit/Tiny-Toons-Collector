using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform follow;
    public int x1;
    public int x2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!follow) return;

        Vector2 pos = follow.position;

        pos.x = Mathf.Clamp(pos.x, x1 + 5, x2 - 4);

        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }
}
