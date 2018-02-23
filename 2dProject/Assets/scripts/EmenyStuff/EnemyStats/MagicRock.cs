using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicRock : EnemyStats
{
    public Transform MagicEssence; 

    void Start ()
    {
        //Essence items
        items.Add(MagicEssence);

        StartStats();
    }

    public override void StartStats()
    {
        dropRate = 1f;
        damagePlayerOnCollision = false;
        health = 5;
        experiencePoint = 0;
        invincible = false;
    }
}
