using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Background,
    FullBlock,
    SlopeP4,
    SlopeP8half,
    SlopeP8full,
    Platform,
    Spikes,
    HalfBlock,
    HalfBlockCeil
}

public class CollisionTile : TileBase
{
    public TileType type;
    public bool orientToRight;
    public bool compatible;

    public bool IsCollision(int x, int y, Entity entity)
    {
        return entity.IsCollision(type, x, y, orientToRight);
    }

    public void GetCollisionRect(ref CollisionData rect, int tileX, int tileY, Entity entity)
    {
        switch (type)
        {
            case TileType.FullBlock:
                rect.UnionTile(tileX, tileY);
                return;
        }

        Vector2 point = orientToRight ? entity.BottomLeft : entity.BottomRight;
        point -= new Vector2(tileX, tileY);

        switch (type)
        {
            case TileType.FullBlock:
            case TileType.Platform:
                rect.UnionTile(tileX, tileY);
                break;
            case TileType.HalfBlock:
                rect.UnionTile(tileX, tileY, 0.5f, 0, 1, 0);
                break;
            case TileType.HalfBlockCeil:
                rect.UnionTile(tileX, tileY, 1, 0.5f, 1, 0);
                break;
            case TileType.SlopeP4:
                if (orientToRight)
                    rect.UnionTile(tileX, tileY, 1 - point.x, 0, 1 - point.y, 0f);
                else
                    rect.UnionTile(tileX, tileY, point.x, 0, 1f, point.y);
                rect.isSlope = true;
                break;
            case TileType.SlopeP8half:
                if (orientToRight)
                    rect.UnionTile(tileX, tileY, Mathf.Min(0.5f - point.x / 2, 0.5f), 0, 1 - point.y * 2, 0f);
                else
                    rect.UnionTile(tileX, tileY, Mathf.Min(point.x / 2, 0.5f), 0, 1f, point.y * 2);
                rect.isSlope = true;
                break;
            case TileType.SlopeP8full:
                if (orientToRight)
                    rect.UnionTile(tileX, tileY, 1 - point.x / 2, 0, 1 - (point.y - 0.5f) * 2, 0f);
                else
                    rect.UnionTile(tileX, tileY, point.x / 2 + 0.5f, 0, 1f, (point.y - 0.5f) * 2);
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

    public bool IsSlope()
    {
        switch (type)
        {
            case TileType.SlopeP4:
            case TileType.SlopeP8full:
            case TileType.SlopeP8half:
                return true;
            default:
                return false;
        }
    }
}


