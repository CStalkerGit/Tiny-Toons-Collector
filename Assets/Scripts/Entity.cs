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

    public bool IsCollisionSlope45(float x, float y, bool toRight)
    {
        var point = GetSlopeCollisionPoint(x, y, toRight);
        if (toRight)
            return point.x <= 1 - point.y;
        else
            return point.x >= point.y;
    }

    public bool IsCollisionSlope225half(float x, float y, bool toRight)
    {
        var point = GetSlopeCollisionPoint(x, y, toRight);

        if (point.y > 0.5f) return false;

        if (toRight)
            return point.x <= 1 - point.y * 2;
        else
            return point.x >= point.y * 2;
    }

    public bool IsCollisionSlope225full(float x, float y, bool toRight)
    {
        var point = GetSlopeCollisionPoint(x, y, toRight);

        if (point.y <= 0.5f) return true;

        if (toRight)
            return point.x <= 1 - (point.y * 2 - 1);
        else
            return point.x >= point.y * 2 - 1;
    }

    public float PrevBottomCoord()
    {
        return prev_pos.y - rh;
    }

    // получает точку необходимую для правильно рассчета коллизии с наклонной поверхностью
    // это ближайшая точка к прямой этой поверхности
    public Vector2 GetSlopeCollisionPoint(float x, float y, bool bottomLeft)
    {
        if (bottomLeft)
            return new Vector2(pos.x - rw - x, pos.y - rh - y);
        else
            return new Vector2(pos.x + rw - x, pos.y - rh - y);
    }

    public Vector2 GetSlopeCollisionPoint(bool bottomLeft)
    {
        if (bottomLeft)
            return new Vector2(pos.x - rw, pos.y - rh);
        else
            return new Vector2(pos.x + rw, pos.y - rh);
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
