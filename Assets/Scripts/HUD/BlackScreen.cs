using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlackScreen : MonoBehaviour
{
    public Tilemap map;
    public Camera cam;
    public Tile blackTile;

    const float defSpeed = 0.05f;
    float speed = 1;

    static BlackScreen ptr = null;

    const int maxRadius = 18;

    bool inProcess;
    bool fadeIn;
    Vector2Int fadePos;
    int radius;
    float timer;

    void Awake()
    {
        ptr = this;
        map.gameObject.SetActive(true);
        //FadeOut();
    }

    void Update()
    {
        if (!inProcess) return;

        timer -= Time.unscaledDeltaTime;

        if (timer > 0) return;

        timer = speed;

        foreach (var pos in map.cellBounds.allPositionsWithin)
        {
            var dist = Mathf.Abs((pos.x - fadePos.x)) + Mathf.Abs((pos.y - fadePos.y));
            if (fadeIn)
            {
                if (dist >= radius) map.SetTile(pos, blackTile);
            }
            else
            {
                if (dist <= radius) map.SetTile(pos, null);
            }
        }

        if (fadeIn)
        {
            radius--;
            if (radius < 0) inProcess = false;
        }
        else
        {
            radius++;
            if (radius > maxRadius)
            {
                inProcess = false;
                Clear(false);
            }
        }
    }

    public void Clear(bool black)
    {
        foreach (var pos in map.cellBounds.allPositionsWithin)
            map.SetTile(pos, black ? blackTile : null);
    }

    void Fade(Vector3 pos, bool fadeIn, bool slow)
    {
        Clear(!fadeIn);
        inProcess = true;
        this.fadeIn = fadeIn;
        fadePos = new Vector2Int(Mathf.RoundToInt((pos.x - cam.transform.position.x) * 2 - 0.5f), Mathf.RoundToInt((pos.y - cam.transform.position.y) * 2 - 0.5f));
        if (!fadeIn) fadePos.Set(0, 0);
        radius = fadeIn ? maxRadius : 0;
        speed = slow ? defSpeed * 2 : defSpeed;
        timer = speed;
    }

    public static void ClearTiles() => ptr?.Clear(false);
    public static void FadeIn(Vector3 pos, bool slow) => ptr?.Fade(pos, true, slow);
    public static void FadeOut() => ptr?.Fade(Vector3.zero, false, false);
    public static bool InProcess => ptr ? ptr.inProcess : false;  
}
