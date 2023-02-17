using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionAlignment
{
    Top,
    Bottom,
    Left,
    Right
}

public class Entity : MonoBehaviour
{
    public float rw = 0.5f;
    public float rh = 0.5f;
    public bool useFlatforms;

    [System.NonSerialized]
    public Vector3 prev_pos;
    [System.NonSerialized]
    public Vector3 pos;

    [System.NonSerialized]
    public Vector3 velocity;

    private bool isDeleted = false;

    void Awake()
    {

    }

    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(rw * 2, rh * 2, 0.01f));
    }

    public bool IsDeleted()
    {
        return isDeleted;
    }

    public void Delete()
    {
        if (isDeleted == false)
        {
            isDeleted = true;
            //Engine.ptr.KillEntity(this);
        }
    }

    public bool IsCollision(Entity entity)
    {
        if (Mathf.Abs(pos.x - entity.pos.x) > (rw + entity.rw)) return false;
        if (Mathf.Abs(pos.y - entity.pos.y) > (rh + entity.rh)) return false;
        return true;
    }

    public bool IsCollision(Vector2 position, float rw, float rh)
    {
        if (Mathf.Abs(pos.x - position.x) > (rw + this.rw)) return false;
        if (Mathf.Abs(pos.y - position.y) > (rh + this.rh)) return false;
        return true;
    }

    public bool IsCollision(float x, float y, float r)
    {
        if (Mathf.Abs(pos.x - x) > (rw + r)) return false;
        if (Mathf.Abs(pos.y - y) > (rh + r)) return false;
        return true;
    }

    public float PrevBottomCoord()
    {
        return prev_pos.y - rh;
    }

#if UNITY_EDITOR
    [ContextMenu("Align")]
    public void Align()
    {
        Vector3 pos = transform.position;
        pos.Set(Mathf.Round(pos.x), Mathf.Round(pos.y), 0);
        transform.position = pos;
    }
#endif
}
