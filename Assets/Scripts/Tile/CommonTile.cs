using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu]
public class CommonTile : CollisionTile
{
    public Sprite sprite;

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = sprite;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CommonTile))]
public class ExampleEditor : UnityEditor.Editor
{
    private CommonTile tile => (target as CommonTile);

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        //Debug.Log("RenderStaticPreview");
        //CommonTile tile = AssetDatabase.LoadAssetAtPath<CommonTile>(assetPath);
        if (!tile || !tile.sprite) return null;

        Texture2D spritePreview = AssetPreview.GetAssetPreview(tile.sprite); // Get sprite texture
        if (!spritePreview) return null;

        //Color[] pixels = spritePreview.GetPixels();
        //for (int i = 0; i < pixels.Length; i++)
        //    pixels[i] = pixels[i] * tile.color; // Tint
        //spritePreview.SetPixels(pixels);
        //spritePreview.Apply();

        Texture2D preview = new Texture2D(width, height);
        EditorUtility.CopySerialized(spritePreview, preview); // Returning the original texture causes an editor crash  
        return preview;
    }
}
#endif

