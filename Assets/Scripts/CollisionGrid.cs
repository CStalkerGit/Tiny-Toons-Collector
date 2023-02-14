using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionGrid : MonoBehaviour
{
    public Tilemap map;
    public float sv_gravity = 1.0f;

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

    public static float Gravity => instance.sv_gravity;

    public static bool IsCollision(Entity entity)
    {
        bool result = false;
        const float offset = 0.1f;

        int x1 = Mathf.FloorToInt(entity.transform.position.x - entity.rw + 0.0f - offset);
        int x2 = Mathf.CeilToInt(entity.transform.position.x + entity.rw - 1.0f + offset);
        int y1 = Mathf.FloorToInt(entity.transform.position.y - entity.rh + 0.0f - offset);
        int y2 = Mathf.CeilToInt(entity.transform.position.y + entity.rh - 1.0f + offset);

        for (int x = x1; x <= x2; x++)
            for (int y = y1; y <= y2; y++)
            {
                CustomTile tile = instance.map.GetTile<CustomTile>(new Vector3Int(x, y, 0));
                if (tile == null) continue;
                if (tile.IsCollision(x, y, entity))
                {
                    result = true;
                }
            }

        return result;
    }
}
