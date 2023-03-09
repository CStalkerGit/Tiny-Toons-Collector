using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class RoadTileEx : RoadTile
{
    public Sprite[] spritesSlopes;

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        var tileUp = tilemap.GetTile(location + new Vector3Int(0, 1, 0)) as CollisionTile;
        if (tileUp && tileUp.IsSlope())
        {
            if (CheckSlope(location, tilemap, -1, 0))
                tileData.sprite = spritesSlopes[0];
            else if (CheckSlope(location, tilemap, 1, 0))
                tileData.sprite = spritesSlopes[1];
            else
                tileData.sprite = spritesSlopes[tileUp.orientToRight ? 3 : 2];
        }
        else
        {
            if (CheckSlope(location, tilemap, -1, 0))
                tileData.sprite = spritesSlopes[4];
            else if (CheckSlope(location, tilemap, 1, 0))
                tileData.sprite = spritesSlopes[5];
            else
            {
                base.GetTileData(location, tilemap, ref tileData);
            }
        }
    }

    bool CheckSlope(Vector3Int location, ITilemap tilemap, int x, int y)
    {
        var tile = tilemap.GetTile(location + new Vector3Int(x, y, 0)) as CollisionTile;
        if (!tile) return false;
        return tile.IsSlope();
    }

    //protected override bool IsCompatible(ITilemap tilemap, Vector3Int position)
    //{
    //    var tile = tilemap.GetTile(position) as CollisionTile;
    //    if (tile == null) return false;
    //    if (tile == this) return true;
    //    if (tile.IsSlope()) return true;
    //    return false;
    //}
}
