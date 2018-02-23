using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : EnemyStats
{

    public Transform HealthDrop;
    public Transform loot; // 100% drop rate
    public Transform loot2; // 5% drop rate
    public Transform loot3; // 5% drop rate

    void Start()
    {
        //turret items
        items.Add(HealthDrop);
        items.Add(loot);
        items.Add(loot2);
        items.Add(loot3);

        StartStats();
    }

    public override void StartStats()
    {
        dropRate = .5f;
        damagePlayerOnCollision = true;
        health = 4;
        experiencePoint = 1;
        invincible = false;
    }

    //getting name is for seeing which part of the game object was hit if it was children
    public override void Damage(int damageCount, string NameObject, float InvicTime)
    {
        //Debug.Log("NameObject = " + NameObject);
        //Debug.Log("damageCount = " + damageCount);
        //only want to damage slider if it get hit in turret; maybe just more damage, add that later
        if (NameObject == "Image")
        {
            if (!invincible)
            {
                health -= damageCount;

                if (health <= 0)
                {
                    DeathPhase();
                }
                else
                {
                    if (InvicTime > 0)
                    {
                        StartCoroutine(MakeInvincible(InvicTime));
                    }
                }
            }
        }
    }
}
