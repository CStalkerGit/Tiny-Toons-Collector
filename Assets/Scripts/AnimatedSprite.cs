using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites;
    public float animSpeed = 0.25f;
    public bool loop = true;
    public bool playOnAwake = true;

    public bool isPlaying { get; private set; }

   float time;
    int frame;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        time = 0;
        frame = 0;
        isPlaying = false;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (playOnAwake) Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isPlaying) return; 

        time += Time.deltaTime;
        if (time >= animSpeed)
        {
            time = 0;
            frame++;
            if (frame >= sprites.Length) frame = 0;
            spriteRenderer.sprite = sprites[frame];
            if (loop == false && frame >= sprites.Length - 1) isPlaying = false;
        }
    }

    public void Play()
    {
        if (sprites.Length < 2) return;
        frame = 0;
        time = 0;
        isPlaying = true;
    }
}
