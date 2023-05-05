using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class EntitySway : MonoBehaviour
{
    Entity entity;
    Vector3 origin;
    float sin;

    void Awake()
    {
        entity = GetComponent<Entity>();
        origin = transform.position;
        sin = Random.Range(-Mathf.PI, Mathf.PI);
    }

    void FixedUpdate()
    {
        var pos = origin;
        pos.y += Mathf.Sin(sin) * 0.2f;
        sin += Time.deltaTime * 3;
        if (sin > Mathf.PI) sin -= Mathf.PI * 2;
        transform.position = pos;
        entity.pos = pos;
    }
}
