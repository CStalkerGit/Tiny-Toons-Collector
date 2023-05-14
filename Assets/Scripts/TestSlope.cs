using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class TestSlope : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var spr = GetComponent<SpriteRenderer>();
        var entity = GetComponent<Entity>();
        
        var data = CollisionGrid.IsCollision(entity);
        //data.Reset(entity, entity.transform.position);
        if (data.isCollision)
            spr.color = Color.red;
        else
            spr.color = Color.white;

        var v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        v.z = 0;
        transform.position = v;
        entity.pos = transform.position;
    }
}
