using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpriteAnimation : ScriptableObject
{
    public Sprite[] sprites;
    public float animSpeed = 0.25f;
    public bool loop = true;
}
