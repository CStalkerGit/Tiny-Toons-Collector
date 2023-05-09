using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/AssistTile")]
public class AssistTile : CollisionTile
{
    public Sprite defSprite;
    public Sprite[] sideSlopes; // 2
    public Sprite[] plainGround; // 3
    public Sprite[] groundToSlope; // 4
    public Sprite[] groundToWall; // 3
    public Sprite[] slopeToWall; // 3

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        tileData.color = Color.white;

        bool upIsSlope = IsSlope(tilemap, location + new Vector3Int(0, 1, 0));
        bool upIsFullBlock = IsFullBlock(tilemap, location + new Vector3Int(0, 1, 0));
        bool leftIsSlope = IsSlope(tilemap, location + new Vector3Int(-1, 0, 0));
        bool rightIsSlope = IsSlope(tilemap, location + new Vector3Int(1, 0, 0));

        tileData.sprite = defSprite;

        if (upIsSlope)
        {
            if (leftIsSlope)
                tileData.sprite = sideSlopes[0];
            else if (rightIsSlope)
                tileData.sprite = sideSlopes[1];
            else
            {
                int offset = 2;
                if (IsFullBlock(tilemap, location + new Vector3Int(0, -1, 0))) offset = 0;
                var tile = tilemap.GetTile(location + new Vector3Int(0, 1, 0)) as CollisionTile;
                tileData.sprite = groundToSlope[(tile.orientToRight ? 1 : 0) + offset];
            }
        }
        else if (upIsFullBlock)
        {
            bool leftCorner = !IsFullBlock(tilemap, location + new Vector3Int(-1, 1, 0));
            bool rightCorner = !IsFullBlock(tilemap, location + new Vector3Int(1, 1, 0));
            int index = 0;
            if (leftCorner)
            {
                if (rightCorner) index = 2; else index = 0;
            }
            else index = 1;
            if (leftIsSlope || rightIsSlope)
                tileData.sprite = slopeToWall[index];
            else
                tileData.sprite = groundToWall[index];
        }
        else
        {
            if (leftIsSlope)
            {
                if (rightIsSlope) tileData.sprite = plainGround[2]; else tileData.sprite = plainGround[0];
            }
            else tileData.sprite = plainGround[1];
        }          
    }

    protected bool IsSlope(ITilemap tilemap, Vector3Int position)
    {
        var tile = tilemap.GetTile(position) as CollisionTile;
        if (tile == null) return false;
        return tile.IsSlope();
    }

    protected bool IsFullBlock(ITilemap tilemap, Vector3Int position)
    {
        var tile = tilemap.GetTile(position) as CollisionTile;
        if (tile == null) return false;
        return tile.type == TileType.FullBlock;
    }
}
