﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodHands : Weapon {

    GodHandsFist godHandsFist;
    GodHandsFistPlayer godHandsFistPlayer;

    public Transform fistPrefab;
    public GameObject fistPlayerPrefab;

    public Transform PlayerFistLocation;

    public Vector3 targetLocation;

    public bool GodhandsCanAttack = true;

    Transform fist;

    void Start()
    {
        fistPlayerPrefab = Resources.Load("Prefabs/WeaponProjectiles/FistPlayer", typeof(GameObject)) as GameObject;
    }

    public override void Attack()
    {
        if (GodhandsCanAttack)
        {
            GodhandsCanAttack = false;

            PlayerFistLocation = GameObject.Find("PlayerProjectiles").transform;
            GameObject weaponTransform = Instantiate(fistPlayerPrefab, GameObject.Find("PlayerProjectiles").transform) as GameObject;

            //set child to have reference to parent
            weaponTransform.GetComponent<GodHandsFistPlayer>().parent = this;

            weaponTransform.name = "Fist";

            godHandsFistPlayer = GameObject.Find("PlayerProjectiles").transform.GetComponentInChildren<GodHandsFistPlayer>();

            godHandsFistPlayer.pullAttackOn = true;
            godHandsFistPlayer.newTargetLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            godHandsFistPlayer.gotoPosition = PlayerController.PlayerControllerSingle.transform.position;
            godHandsFistPlayer.userTransform = weaponTransform.GetComponent<Transform>();
            godHandsFistPlayer.userTransform.position = PlayerController.PlayerControllerSingle.transform.position;
        }
    }

    public void AttackBoss(Vector3 targetLocation)
    {
        fist = Instantiate(fistPrefab, transform);
        fist.name = "Fist";

        godHandsFist = transform.GetComponentInChildren<GodHandsFist>();

        godHandsFist.tag = "EnemyProjectile";

        godHandsFist.pullAttackOn = true;
        godHandsFist.newTargetLocation = targetLocation;

        godHandsFist.gotoPosition = transform.position;
        godHandsFist.userTransform = fist.GetComponent<Transform>();
        godHandsFist.userTransform.position = transform.position;
    }
}
