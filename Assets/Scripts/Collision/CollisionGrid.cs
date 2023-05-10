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

    void Start()
    {
        // spawn entities from SpawnTile
        foreach (var pos in map.cellBounds.allPositionsWithin)
        {
            var tile = map.GetTile<SpawnTile>(pos);
            if (tile)
            {
                Instantiate(tile.prefab, new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0), Quaternion.identity);
                map.SetTile(pos, null);
            }
        }
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
                var pos = new Vector3Int(x, y, 0);
                var tile = ptr.map.GetTile<CollisionTile>(pos);
                if (tile == null) continue;

                data.position = pos;

                if (tile.IsCollision(x, y, entity))
                {
                    data.isStuck = true;
                }

                if (entity.IsCollision(x + 0.5f, y + 0.5f, 0.5f))
                {
                    data.trigger = tile.trigger;

                    switch (tile.type)
                    {
                        case TileType.Spikes:
                            if (entity.BottomCoord < y + 0.5f) data.isSpike = true;
                            break;
                        case TileType.Exit:
                            data.trigger = TriggerType.Exit;
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
        if (tile)
        {
            return tile.type == TileType.Background;
        }
        else 
            return true;
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

    public static bool FindNearestTeleport(Vector3Int tilePos, bool searchDown, out Vector3 result)
    {
        Vector3Int position = tilePos;
        TriggerType search = searchDown ? TriggerType.TeleportUp : TriggerType.TeleportDown;
        result = tilePos;

        for (int i = 0; i < 20; i++)
        {
            position.y += searchDown ? -1 : 1;
            var tile = ptr.map.GetTile<CollisionTile>(position);
            if (tile && tile.trigger == search)
            {
                result = position + new Vector3(0.5f, searchDown ? -0.5f : 1.5f, 0);
                return true;
            }
        }

        return false;
    }

    [ContextMenu("Clear All Tiles")]
    void ClearAllTiles()
    {
        map.ClearAllTiles();
        map.CompressBounds();
    }
}
