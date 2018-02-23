using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : EnemyStats {

    public Transform HealthDrop;
    public Transform loot; // 100% drop rate
    public Transform loot2; // 5% drop rate
    public Transform loot3; // 5% drop rate
    public Transform loot4; // 5% drop rate
    public Transform loot5; // 5% drop rate
    public Transform loot6; // 5% drop rate
    public Transform loot7; // 5% drop rate
    public Transform loot8; // 5% drop rate

    void Start ()
    {
        //turret items
        items.Add(HealthDrop);
        this.items.Add(loot);
        this.items.Add(loot2);
        this.items.Add(loot3);
        this.items.Add(loot4);
        this.items.Add(loot5);
        this.items.Add(loot6);
        this.items.Add(loot7);
        this.items.Add(loot8);

        StartStats();
    }

    public override void StartStats()
    {
        dropRate = .5f;
        damagePlayerOnCollision = false;
        health = 4;
        experiencePoint = 1;
        invincible = false;
    }

    //getting name is for seeing which part of the game object was hit if it was children
    public override void Damage(int damageCount, string NameObject, float InvicTime)
    {
        //Debug.Log("NameObject = " + NameObject);
        //Debug.Log("damageCount = " + damageCount);
        //only want to damage turret if it get hit in turret; maybe just more damage, add that later
        if (NameObject == "CrystalCollider")
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
                    if(InvicTime > 0)
                    {
                        StartCoroutine(MakeInvincible(InvicTime));
                    }
                }
            }
        }
    }

}
