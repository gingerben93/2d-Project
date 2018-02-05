using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : EnemyStats
{

    public Transform loot; // 100% drop rate
    public Transform loot2; // 5% drop rate
    public Transform loot3; // 5% drop rate

    void Start()
    {
        this.items.Add(loot);
        this.items.Add(loot2);
        this.items.Add(loot3);

        health = 3;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
