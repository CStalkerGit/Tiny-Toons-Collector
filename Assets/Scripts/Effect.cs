using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    SpriteAnimator anim;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<SpriteAnimator>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        bool audioEnd = true;
        if (audioSource) audioEnd = !audioSource.isPlaying;
        bool animationEnd = true;
        if (anim) animationEnd = !anim.isPlaying;

        if (audioEnd && animationEnd) Destroy(gameObject);
    }
}
