using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites;
    public float animSpeed = 0.25f;
    float time;
    int frame;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        frame = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sprites.Length == 0) return;

        time += Time.deltaTime;
        if (time >= animSpeed)
        {
            time = 0;
            frame++;
            if (frame >= sprites.Length) frame = 0;
            spriteRenderer.sprite = sprites[frame];
        }
    }
}
