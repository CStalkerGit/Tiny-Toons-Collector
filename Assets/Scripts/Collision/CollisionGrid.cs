using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionGrid : MonoBehaviour
{
    public Tilemap map;
    public static float Gravity => 18f;
    public static float MinStep => 0.01f;

    static CollisionGrid ptr = null;

    // Start is called before the first frame update
    void Awake()
    {
        if (ptr) Debug.LogWarning("CollisionGrid pointer is not null");
        ptr = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        ptr = null;
    }

    public static CollisionData IsCollision(Entity entity)
    {
        var data = new CollisionData();

        data.Reset(entity, entity.pos);

        var bounds = GetEntityBounds(entity);

        for (int x = bounds.x; x <= bounds.xMax; x++)
            for (int y = bounds.y; y <= bounds.yMax; y++)
            {
                var tile = ptr.map.GetTile<CollisionTile>(new Vector3Int(x, y, 0));
                if (tile == null) continue;
                if (tile.IsCollision(x, y, entity))
                {
                    tile.GetCollisionRect(ref data, x, y, entity);
                    data.isCollision = true;
                }
            }

        return data;
    }

    public static SpecialCollision IsSpecialCollision(Entity entity)
    {
        SpecialCollision data = new SpecialCollision();

        var bounds = GetEntityBounds(entity);

        for (int x = bounds.x; x <= bounds.xMax; x++)
            for (int y = bounds.y; y <= bounds.yMax; y++)
            {
                var tile = ptr.map.GetTile<CollisionTile>(new Vector3Int(x, y, 0));
                if (tile == null) continue;

                if (tile.IsCollision(x, y, entity))
                {
                    data.isStuck = true;

                    switch (tile.type)
                    {
                        case TileType.Spikes:
                            data.isSpike = true;
                            break;
                    }
                }
            }

        return data;
    }

    public static bool IsCollision(Entity entity, float offsetX, float offsetY)
    {
        var lastPos = entity.pos;
        entity.pos.x += offsetX;
        entity.pos.y += offsetY;

        var bounds = GetEntityBounds(entity);

        for (int x = bounds.x; x <= bounds.xMax; x++)
            for (int y = bounds.y; y <= bounds.yMax; y++)
            {
                var tile = ptr.map.GetTile<CollisionTile>(new Vector3Int(x, y, 0));
                if (tile == null) continue;
                if (tile.IsCollision(x, y, entity))
                {
                    entity.pos = lastPos;
                    return true;
                }
            }

        entity.pos = lastPos;
        return false;
    }

    public static bool IsPitAhead(Entity entity, bool toRight)
    {
        int x = Mathf.FloorToInt(entity.pos.x + (toRight? .5f : -.5f));
        int y = Mathf.FloorToInt(entity.pos.y);
        var tile = ptr.map.GetTile<CollisionTile>(new Vector3Int(x, y, 0));
        if (tile) return false;

        y = Mathf.FloorToInt(entity.pos.y - 1f);
        tile = ptr.map.GetTile<CollisionTile>(new Vector3Int(x, y, 0));

        return tile == null; 
    }

    static BoundsInt GetEntityBounds(Entity entity)
    {
        const float offset = 0.1f;
   
        int x1 = Mathf.FloorToInt(entity.pos.x - entity.rw + 0.0f - offset);
        int x2 = Mathf.CeilToInt(entity.pos.x + entity.rw - 1.0f + offset);
        int y1 = Mathf.FloorToInt(entity.pos.y - entity.rh + 0.0f - offset);
        int y2 = Mathf.CeilToInt(entity.pos.y + entity.rh - 1.0f + offset);

        BoundsInt bounds = new BoundsInt();
        bounds.SetMinMax(new Vector3Int(x1, y1, 0), new Vector3Int(x2, y2, 0));
        return bounds;
    }

    [ContextMenu("Clear All Tiles")]
    void ClearAllTiles()
    {
        map.ClearAllTiles();
        map.CompressBounds();
    }
}
