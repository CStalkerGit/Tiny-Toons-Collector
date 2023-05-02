using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    public int health;

    protected override bool OnCollision()
    {
        Player.RestoreHealth(health);
        return true;
    }
}
