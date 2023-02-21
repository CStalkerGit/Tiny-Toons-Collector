using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct AlignRect
{
    public float top, bottom, right, left;
    public void Reset(Entity entity, Vector3 newPosition)
    {
        top = newPosition.y - entity.rh;
        bottom = newPosition.y + entity.rh;
        right = newPosition.x - entity.rw;
        left = newPosition.x + entity.rw;
    }
    public void Inflate(float tileX, float tileY)
    {
        if (top < tileY + 1) top = tileY + 1;
        if (bottom > tileY) bottom = tileY;
        if (right < tileX + 1f) right = tileX + 1f;
        if (left > tileX) left = tileX;
    }
    // увеличиваем ограничивающий прямоугольник, при этом не превышая максимальных границ тайла
    public void Inflate(float tileX, float tileY, float _top, float _bottom, float _right, float _left)
    {
        if (top < _top) top = _top < tileY + 1 ? _top : tileY + 1;
        if (bottom > _bottom) bottom = _bottom > tileY ? _bottom : tileY;
        if (right < _right) right = _right < tileX + 1f ? _right : tileX + 1f;
        if (left > _left) left = _left > tileX ? _left : tileX;
    }
    public void InflateLocal(float tileX, float tileY, float localTop, float localBottom, float localRight, float localLeft)
    {
        localTop += tileY;
        localBottom += tileY;
        localRight += tileX;
        localLeft += tileX;
        if (top < localTop) top = Mathf.Min(localTop, tileY + 1); // newTop < tileY + 1 ? newTop : tileY + 1;
        if (bottom > localBottom) bottom = Mathf.Max(localBottom, tileY);
        if (right < localRight) right = Mathf.Min(localRight, tileX + 1f);
        if (left > localLeft) left = Mathf.Max(localLeft, tileX);
    }
}

public class CollisionGrid : MonoBehaviour
{
    public Tilemap map;
    public static float Gravity => 18f;
    public static float MinStep => 0.01f;

    static CollisionGrid instance = null;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance) Debug.LogWarning("CollisionGrid pointer is not null");
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        instance = null;
    }

    public static bool IsCollision(Entity entity, ref AlignRect align)
    {
        bool result = false;
        const float offset = 0.1f;

        int x1 = Mathf.FloorToInt(entity.pos.x - entity.rw + 0.0f - offset);
        int x2 = Mathf.CeilToInt(entity.pos.x + entity.rw - 1.0f + offset);
        int y1 = Mathf.FloorToInt(entity.pos.y - entity.rh + 0.0f - offset);
        int y2 = Mathf.CeilToInt(entity.pos.y + entity.rh - 1.0f + offset);

        for (int x = x1; x <= x2; x++)
            for (int y = y1; y <= y2; y++)
            {
                CustomTile tile = instance.map.GetTile<CustomTile>(new Vector3Int(x, y, 0));
                if (tile == null) continue;
                if (tile.IsCollision(x, y, entity))
                {
                    tile.GetAlignRect(ref align, x, y, entity);
                    result = true;
                }
            }

        return result;
    }

    public static void ProcessCollision(Entity entity)
    {

    }
}
