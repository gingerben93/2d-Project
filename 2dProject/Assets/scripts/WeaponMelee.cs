using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : MonoBehaviour {

    public Transform shotPrefab;
    public float shootingRate = 0.25f;

    private float shootCooldown;
    private Transform holdForBullets;

    void Start()
    {
        shootCooldown = 0f;
        //rotation = transform.rotation;
        holdForBullets = GameObject.Find("PlayerProjectiles").transform;
    }

    void LateUpdate()
    {
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
    }

    public void Attack(bool isEnemy)
    {
        if (CanAttack)
        {
            shootCooldown = shootingRate;

            // Create a new shot
            var shotTransform = Instantiate(shotPrefab) as Transform;
            shotTransform.transform.SetParent(holdForBullets);
            // Assign position
            //shotTransform.position = transform.position
            if (GameController.GameControllerSingle.facingRight == true)
            {
                shotTransform.position = transform.position + transform.right;
            }
            else if (GameController.GameControllerSingle.facingRight == false)
            {
                shotTransform.transform.Rotate(0, 180, 0);
                shotTransform.position = transform.position - transform.right;
            }
            /* // Make the weapon shot always towards it
             ShotMove move = shotTransform.gameObject.GetComponent<ShotMove>();
             if (move != null)
             {
                 move.direction = this.transform.right; // towards in 2D space is the right of the sprite
             }*/
        }
    }


    public bool CanAttack
    {
        get
        {
            return shootCooldown <= 0f;
        }
    }
}
