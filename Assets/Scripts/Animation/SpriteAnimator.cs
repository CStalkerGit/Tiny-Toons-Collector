using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour
{
    [SerializeField]
    private SpriteAnimation anim;
    public bool playOnAwake = true;

    // components
    SpriteRenderer spriteRenderer;

    // properies
    public bool isPlaying { get; private set; }

    // private
    float time;
    int frame;

    void Awake()
    {
        // components
        spriteRenderer = GetComponent<SpriteRenderer>();

        time = 0;
        frame = 0;
        isPlaying = false;

        if (playOnAwake) Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isPlaying) return;

        time += Time.deltaTime;
        if (time >= anim.animSpeed)
        {
            time = 0;
            frame++;
            if (frame >= anim.sprites.Length) frame = 0;
            spriteRenderer.sprite = anim.sprites[frame];
            if (anim.loop == false && frame >= anim.sprites.Length - 1) isPlaying = false;
        }
    }

    public void Play()
    {
        if (!anim) return;
        spriteRenderer.sprite = anim.sprites[0];
        frame = 0;
        time = 0;
        isPlaying = anim.sprites.Length > 1 ? true : false;
    }

    public void SetAnimation(SpriteAnimation a)
    {
        if (anim != a)
        {
            anim = a;
            Play();
        }
    }

    public void SetSprite(Sprite spr)
    {
        isPlaying = false;
        anim = null;
        if (spriteRenderer.sprite != spr) spriteRenderer.sprite = spr;
    }
}
