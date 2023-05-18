using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public PlatformCoord coord;
    public bool solid;
    public Entity entity { get; private set; }

    Vector3 origin;
    bool back;

    void Awake()
    {
        entity = GetComponent<Entity>();
        origin = transform.position;
    }

    void FixedUpdate()
    {
        var pos = transform.position;
        float force = (back ? -1 : 1);
        pos.x += force * Time.deltaTime;
        if (Mathf.Abs(pos.x - origin.x) > 2) back = !back;
        transform.position = pos;
        entity.pos = transform.position;

        if (Player.IsPlayerOnPlatform(entity))
        {
            Player.AddImpulse(force, 0);
        }
    }
}
