﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/RoadTile")]
public class RoadTile : CollisionTile
{
    public Sprite[] sprites;

    static protected readonly int[] indices4 = new int[4] { 3, 2, 0, 1 };
    static protected readonly int[] indices16 = new int[16] { 15, 11, 12, 8, 3, 7, 0, 4, 14, 10, 13, 9, 2, 6, 1, 5 };

    public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
    {
        //Debug.Log(go);
        return true;
    }

    public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    {
        for (int yd = -1; yd <= 1; yd++)
            for (int xd = -1; xd <= 1; xd++)
            {
                Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
                if (IsCompatible(tilemap, position)) tilemap.RefreshTile(position);
            }

        //tilemap.RefreshTile(location);
    }

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        switch (sprites.Length)
        {
            case 1:
                tileData.sprite = sprites[0];
                tileData.color = Color.white;
                break;
            case 2:
                tileData.sprite = sprites[0];
                tileData.color = Color.white;
                break;
            case 4:
                Tiles4(location, tilemap, ref tileData);
                break;
            case 16:
                Tiles16(location, tilemap, ref tileData);
                break;
            default:
                tileData.sprite = null;
                tileData.color = Color.red;
                break;
        }
    }

    private void Tiles4(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        int mask = IsCompatible(tilemap, location + new Vector3Int(-1, 0, 0)) ? 1 : 0;
        mask += IsCompatible(tilemap, location + new Vector3Int(1, 0, 0)) ? 2 : 0;

        if (mask >= 0 && mask < sprites.Length)
        {
            tileData.sprite = sprites[indices4[mask]];
            tileData.color = Color.white;
        }
        else Debug.LogWarning("Not enough sprites in instance");
    }

    private void Tiles16(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        int mask = IsCompatible(tilemap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
        mask += IsCompatible(tilemap, location + new Vector3Int(1, 0, 0)) ? 2 : 0;
        mask += IsCompatible(tilemap, location + new Vector3Int(0, -1, 0)) ? 4 : 0;
        mask += IsCompatible(tilemap, location + new Vector3Int(-1, 0, 0)) ? 8 : 0;

        if (mask >= 0 && mask < sprites.Length)
        {
            tileData.sprite = sprites[indices16[mask]];
            tileData.color = Color.white;
        }
        else Debug.LogWarning("Not enough sprites in instance");
    }

    protected virtual bool IsCompatible(ITilemap tilemap, Vector3Int position)
    {
        var tile = tilemap.GetTile(position) as CollisionTile;
        if (tile == null) return false;
        if (tile == this) return true;
        if (tile.compatible) return true;
        //if (sprites.Length < 16) return false
        return false;
    }
}
