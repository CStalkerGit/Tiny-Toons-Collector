using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class Item : MonoBehaviour
{
    Entity entity;

    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity>();
    }

    public bool IsCollision(Entity actor) => entity.IsCollision(actor);
    public void Delete() => entity.Delete();
}
