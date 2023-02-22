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

    public static bool IsCollision(Entity entity, ref AlignRect align)
    {
        bool result = false;
        const float offset = 0.1f;

        align.Reset(entity, entity.pos);

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
