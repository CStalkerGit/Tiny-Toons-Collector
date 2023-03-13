using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class HUD : MonoBehaviour
{
    public Digits carrots;

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
