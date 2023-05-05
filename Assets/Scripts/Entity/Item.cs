using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Entity))]
[DisallowMultipleComponent]
public class Item : MonoBehaviour
{
    public int Points;

    Entity entity;

    void Awake()
    {
        entity = GetComponent<Entity>();
    }

    void FixedUpdate()
    {
        if (Player.IsCollision(entity))
        {
            if (OnCollision())
            {
                Data.points += Points;
                Game.SpawnPopUp(Points, transform.position);
                Game.Pickup(transform.position);
                Destroy(gameObject);
            }
        }
    }

    public bool IsCollision(Entity actor) => entity.IsCollision(actor);

    protected virtual bool OnCollision()
    {
        return true;
    }
}
