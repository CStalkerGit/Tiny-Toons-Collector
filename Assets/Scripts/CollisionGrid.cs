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

    public static bool IsCollision(Entity entity, ref CollisionRect data)
    {
        bool result = false;
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
                    tile.GetAlignRect(ref data, x, y, entity);
                    result = true;
                }
            }

        return result;
    }

    public static void ProcessActorCollisions(Actor actor)
    {
        var targets = FindObjectsOfType<Entity>();
        foreach (var target in targets)
        {
            if (actor.entity.IsCollision(target))
            {
                if (actor.entity.PrevBottomCoord > target.TopCoord)
                {
                    //if (target.Stomp()) actor.physics.velocity.y = 5;
                }
            }
        }
    }

    [ContextMenu("Clear All Tiles")]
    void ClearAllTiles()
    {
        map.ClearAllTiles();
        map.CompressBounds();
    }
}
