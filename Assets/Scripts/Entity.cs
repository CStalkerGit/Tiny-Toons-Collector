using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    void Awake()
    {
        pos = transform.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(rw * 2, rh * 2, 0.01f));
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

    public bool IsCollision(TileType type, float x, float y, bool toRight)
    {
        if (!IsCollision(x + 0.5f, y + 0.5f, 0.5f)) return false;
        switch (type)
        {
            case TileType.FullBlock:
            //case TileType.Spikes:
                return true;
            case TileType.Platform:
                return PrevBottomCoord > y + 1;
        }

        var p = toRight ? BottomLeft : BottomRight;
        p.x -= x;
        p.y -= y;

        switch (type)
        {           
            case TileType.SlopeP4:
                return toRight ? (p.x <= 1 - p.y) : (p.x >= p.y);
            case TileType.SlopeP8half:
                if (p.y > 0.5f) return false;
                return toRight ? (p.x <= 1 - p.y * 2) : (p.x >= p.y * 2);
            case TileType.SlopeP8full:
                if (p.y <= 0.5f) return true;
                return toRight ? (p.x <= 1 - (p.y * 2 - 1)) : (p.x >= p.y * 2 - 1);
            default:
                return false;
        }
    }

    public float TopCoord => prev_pos.y + rh;
    public float PrevBottomCoord => prev_pos.y - rh;

    // получает точку необходимую для правильно рассчета коллизии с наклонной поверхностью
    // это ближайшая точка нижней части entity к наклонной поверхности
    public Vector2 BottomLeft => new Vector2(pos.x - rw, pos.y - rh); 
    public Vector2 BottomRight => new Vector2(pos.x + rw, pos.y - rh);

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
