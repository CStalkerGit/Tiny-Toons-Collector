using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Entity))]
public class Item : MonoBehaviour
{
    Entity entity;

    void Awake()
    {
        entity = GetComponent<Entity>();
    }

    void FixedUpdate()
    {
        if (Player.IsCollision(entity))
        {
            Stats.carrots++;
            Destroy(gameObject);
        }
    }

    public bool IsCollision(Entity actor) => entity.IsCollision(actor);
}
