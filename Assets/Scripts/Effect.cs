using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    AnimatedSprite animatedSprite;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        animatedSprite = GetComponent<AnimatedSprite>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        bool audioEnd = true;
        if (audioSource) audioEnd = !audioSource.isPlaying;
        bool animationEnd = true;
        if (animatedSprite) animationEnd = !animatedSprite.isPlaying;

        if (audioEnd && animationEnd) Destroy(gameObject);
    }
}
