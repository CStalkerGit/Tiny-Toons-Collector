using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlackScreen : MonoBehaviour
{
    public Tilemap map;
    public Camera cam;
    public Tile blackTile;
    public float speed;

    static BlackScreen ptr = null;

    bool fading;
    Vector2Int fadingPos;
    int radius;
    float timer;

    void Awake()
    {
        ptr = this;
        Clear();
    }

    void FixedUpdate()
    {
        if (!fading) return;

        timer -= Time.deltaTime;

        if (timer > 0) return;

        timer = speed;

        foreach (var pos in map.cellBounds.allPositionsWithin)
        {
            var dist = Mathf.Abs((pos.x - fadingPos.x)) + Mathf.Abs((pos.y - fadingPos.y));
            if (dist >= radius)
                map.SetTile(pos, blackTile);
        }

        radius--;
        if (radius < 0) fading = false;
    }

    public void Clear()
    {
        // var tile = map.GetTile<Tile>(pos);
        foreach (var pos in map.cellBounds.allPositionsWithin)
            map.SetTile(pos, null);
    }

    public void FadeIn(Vector3 pos)
    {
        fading = true;
        fadingPos = new Vector2Int(Mathf.RoundToInt((pos.x - cam.transform.position.x) * 2 - 0.5f), Mathf.RoundToInt((pos.y - cam.transform.position.y) * 2 - 0.5f));
        radius = 18;
        timer = speed;
    }

    public static void ClearTiles() => ptr?.Clear();
    public static void FadeInEffect(Vector3 pos) => ptr?.FadeIn(pos);
    public static bool Finished => ptr?.radius < 0;
}
