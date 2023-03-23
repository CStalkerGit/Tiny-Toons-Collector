using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Actor actor;
    public Digits carrots;
    public Image[] imgHealth;
    public Sprite[] sprHealth;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (carrots) carrots.Set(Stats.carrots);
    }
}
