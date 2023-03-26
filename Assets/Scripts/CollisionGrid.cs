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
        const float offset = 0.1f;

        data.Reset(entity, entity.pos);

        int x1 = Mathf.FloorToInt(entity.pos.x - entity.rw + 0.0f - offset);
        int x2 = Mathf.CeilToInt(entity.pos.x + entity.rw - 1.0f + offset);
        int y1 = Mathf.FloorToInt(entity.pos.y - entity.rh + 0.0f - offset);
        int y2 = Mathf.CeilToInt(entity.pos.y + entity.rh - 1.0f + offset);

        for (int x = x1; x <= x2; x++)
            for (int y = y1; y <= y2; y++)
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
        const float offset = 0.1f;

        int x1 = Mathf.FloorToInt(entity.pos.x - entity.rw + 0.0f - offset);
        int x2 = Mathf.CeilToInt(entity.pos.x + entity.rw - 1.0f + offset);
        int y1 = Mathf.FloorToInt(entity.pos.y - entity.rh + 0.0f - offset);
        int y2 = Mathf.CeilToInt(entity.pos.y + entity.rh - 1.0f + offset);

        for (int x = x1; x <= x2; x++)
            for (int y = y1; y <= y2; y++)
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

    [ContextMenu("Clear All Tiles")]
    void ClearAllTiles()
    {
        map.ClearAllTiles();
        map.CompressBounds();
    }
}
