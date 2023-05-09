using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Tiles/SpawnTile")]
public class SpawnTile : TileBase
{
    public Sprite sprite;
    public GameObject prefab;

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = sprite;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SpawnTile))]
public class SpawnTileEditor : UnityEditor.Editor
{
    private SpawnTile tile => (target as SpawnTile);

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        if (!tile || !tile.sprite) return null;

        Texture2D spritePreview = AssetPreview.GetAssetPreview(tile.sprite); // Get sprite texture
        if (!spritePreview) return null;

        Texture2D preview = new Texture2D(width, height);
        EditorUtility.CopySerialized(spritePreview, preview); // Returning the original texture causes an editor crash  
        return preview;
    }
}
#endif
