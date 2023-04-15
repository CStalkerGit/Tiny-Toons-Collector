using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class AnimatedTile : CollisionTile
{
    public Sprite[] sprites;

    public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
    {
        if (sprites != null && sprites.Length > 0)
            tileData.sprite = sprites[0];
    }

    public override bool GetTileAnimationData(Vector3Int location, ITilemap tileMap, ref TileAnimationData data)
    {
        if (sprites != null && sprites.Length > 1)
        {
            data.animatedSprites = sprites; 
            data.animationSpeed = 6;
            data.animationStartTime = 0; // Random.Range(0, 2);
            return true;
        }
        return false;
    }
}
