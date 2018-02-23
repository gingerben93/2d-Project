using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : EnemyStats
{
    public Transform HealthDrop;
    public Transform loot;
    public Transform loot2;
    public Transform loot3;

    void Start()
    {
        items.Add(HealthDrop);
        this.items.Add(loot);
        this.items.Add(loot2);
        this.items.Add(loot3);

        StartStats();
    }

    public override void StartStats()
    {
        dropRate = .5f;
        damagePlayerOnCollision = true;
        health = 3;
        experiencePoint = 4;
        invincible = false;
    }
}
