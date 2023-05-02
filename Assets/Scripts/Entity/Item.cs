using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Entity))]
[DisallowMultipleComponent]
public class Item : MonoBehaviour
{
    public int Points;

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
        if (Player.IsCollision(entity))
        {
            if (OnCollision())
            {
                Stats.points += Points;
                Game.Pickup(transform.position);
                Destroy(gameObject);
            }
        }

        var pos = origin;
        pos.y += Mathf.Sin(sin) * 0.2f;
        sin += Time.deltaTime * 3;
        if (sin > Mathf.PI) sin -= Mathf.PI * 2;
        transform.position = pos;
        entity.pos = pos;
    }

    public bool IsCollision(Entity actor) => entity.IsCollision(actor);

    protected virtual bool OnCollision()
    {
        return true;
    }
}
