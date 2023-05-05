﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Digits points;
    public Image[] imgHealth;
    public Sprite[] sprHealth;

    int playerHealth = -1;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (points) points.Set(Data.points);

        if (playerHealth != Data.health)
        {
            playerHealth = Data.health;
            for (int i = 0; i < imgHealth.Length; i++)
            {
                var hp = (i + 1) * 2;
                if (hp > Data.maxHealth)
                    imgHealth[i].sprite = sprHealth[3]; // no heart
                else if (hp <= playerHealth)
                    imgHealth[i].sprite = sprHealth[0]; // full heart
                else if (hp - 1 == playerHealth)
                    imgHealth[i].sprite = sprHealth[1]; // half heart
                else
                    imgHealth[i].sprite = sprHealth[2]; // empty heart
            }
        }
    }
}
