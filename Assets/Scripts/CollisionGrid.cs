using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    public static CollisionData IsCollision(Entity entity)
    {
        var data = new CollisionData();

        data.Reset(entity, entity.pos);

        var bounds = GetEntityBounds(entity);

        for (int x = bounds.x; x <= bounds.xMax; x++)
            for (int y = bounds.y; y <= bounds.yMax; y++)
            {
                var tile = instance.map.GetTile<CollisionTile>(new Vector3Int(x, y, 0));
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
                var tile = instance.map.GetTile<CollisionTile>(new Vector3Int(x, y, 0));
                if (tile == null) continue;

                if (tile.IsCollision(x, y, entity))
                    data.isStuck = true;

                switch (tile.type)
                {
                    case TileType.Spikes:
                        data.isSpike = true;
                        break;
                }
            }

        return data;
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
