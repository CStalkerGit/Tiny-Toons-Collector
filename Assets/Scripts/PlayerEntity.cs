using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    Entity entity;

    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var items = FindObjectsOfType<Item>();
        foreach(var item in items)
        {
            if (item.IsCollision(entity))
                Destroy(item.gameObject, 0.2f); //item.Delete();
        }
    }
}
