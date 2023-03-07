using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Background,
    FullBlock,
    Slope45,
    Slope225half,
    Slope225full,
    Platform,
    Spikes
}

[CreateAssetMenu]
public class CollisionTile : TileBase
{
    public TileType type;
    public bool orientToRight;
    public Sprite sprite;

    //public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
    //{
    //    //Debug.Log(go);
    //    return true;
    //}

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = sprite;
    }

    public bool IsCollision(int x, int y, Entity entity)
    {
        if (!entity.IsCollision(x + 0.5f, y + 0.5f, 0.5f)) return false;

        switch (type)
        {
            case TileType.FullBlock:
                return true; // entity.IsCollision(x + 0.5f, y + 0.5f, 0.5f);
            case TileType.Slope45:
                return entity.IsCollisionSlope45(x, y, orientToRight);
            case TileType.Slope225half:
                return entity.IsCollisionSlope225half(x, y, orientToRight);
            case TileType.Slope225full:
                return entity.IsCollisionSlope225full(x, y, orientToRight);
            default:
                return false;
        }
    }

    public void GetAlignRect(ref CollisionData rect, int tileX, int tileY, Entity entity)
    {
        Vector2 point;
        switch (type)
        {
            case TileType.FullBlock:
                rect.Inflate(tileX, tileY);
                break;
            case TileType.Slope45:
                point = entity.GetSlopeCollisionPoint(tileX, tileY, orientToRight);
                if (orientToRight)
                    rect.InflateLocal(tileX, tileY, 1 - point.x, 0, 1 - point.y, 0f);
                else
                    rect.InflateLocal(tileX, tileY, point.x, 0, 1f, point.y);
                rect.isSlope = true;
                break;
            case TileType.Slope225half:
                point = entity.GetSlopeCollisionPoint(tileX, tileY, orientToRight);
                if (orientToRight)
                    rect.InflateLocal(tileX, tileY, Mathf.Min(0.5f - point.x / 2, 0.5f), 0, 1 - point.y * 2, 0f);
                else
                    rect.InflateLocal(tileX, tileY, Mathf.Min(point.x / 2, 0.5f), 0, 1f, point.y * 2);
                rect.isSlope = true;
                break;
            case TileType.Slope225full:
                point = entity.GetSlopeCollisionPoint(tileX, tileY, orientToRight);
                if (orientToRight)
                    rect.InflateLocal(tileX, tileY, 1 - point.x / 2, 0, 1 - (point.y - 0.5f) * 2, 0f);
                else
                    rect.InflateLocal(tileX, tileY, point.x / 2 + 0.5f, 0, 1f, (point.y - 0.5f) * 2);
                rect.isSlope = true;
                break;
        }
    }

    public bool CanWalk(Entity entity)
    {
        switch (type)
        {
            case TileType.FullBlock:
                return true;
            case TileType.Platform:
                return entity.useFlatforms;
            default:
                return false;
        }
    }

    public bool Solid(Entity entity)
    {
        switch (type)
        {
            case TileType.FullBlock:
                return true;
            default:
                return false;
        }
    }
}
