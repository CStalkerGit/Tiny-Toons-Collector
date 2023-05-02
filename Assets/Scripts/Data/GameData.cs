using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu]
public class GameData : ScriptableObject
{
    public Effect poof;
    public Effect pickup;
    public Effect hit;
    public Effect down;

    public AudioClip clipDefeat;
    public AudioClip clipVictory;

    public PopUp popUp;
}
