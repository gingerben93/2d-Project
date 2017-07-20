using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortSword : MonoBehaviour {

    private GameObject SwordPrefab;
    public float weaponAttackRate = 0.25f;

    private float weaponCooldown;

    void Start()
    {
        SwordPrefab = Resources.Load("Prefabs/WeaponProjectiles/Melee", typeof(GameObject)) as GameObject;
        weaponCooldown = 0f;
        //rotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (weaponCooldown > 0)
        {
            weaponCooldown -= Time.deltaTime;
        }
    }

    public void Attack(/*bool isEnemy*/)
    {
        if (CanAttack)
        {
            weaponCooldown = weaponAttackRate;

            // Create a new shot
            GameObject WeaponTransform = Instantiate(SwordPrefab, GameObject.Find("PlayerProjectiles").transform) as GameObject;
            // Assign position
            //shotTransform.position = transform.position
            if (GameController.GameControllerSingle.facingRight == true)
            {
                WeaponTransform.transform.position = transform.position + transform.right;
            }
            else if (GameController.GameControllerSingle.facingRight == false)
            {
                WeaponTransform.transform.Rotate(0, 180, 0);
                WeaponTransform.transform.position = transform.position - transform.right;
            }
        }
    }


    public bool CanAttack
    {
        get
        {
            return weaponCooldown <= 0f;
        }
    }
}
