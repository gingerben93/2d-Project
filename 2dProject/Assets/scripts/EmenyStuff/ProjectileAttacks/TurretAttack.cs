using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttack : MonoBehaviour {
    
    bool startTimer = false;

    private float timer = 0;
    public float fireRate = 2f;
    private float distanceToPlayer;

    private Vector3 playersLastLocations;

    public Transform fireBallPrefab;

    public int attackType = 0;

    public Vector3 spawnLocationOffset;
	
	// Update is called once per frame
	void Update () {
        //for checking how far the player is
        distanceToPlayer = Vector3.Distance(transform.position, PlayerController.PlayerControllerSingle.transform.position);

        if (distanceToPlayer <= 30)
        {
            startTimer = true;
        }
        //timer for time until attack
        if (startTimer)
        {
            timer += Time.deltaTime;
        }
        //2 seconds timer for firing projectiles, smaller = faster larger = slower
        if (timer >= fireRate)
        {
            startTimer = false;
            timer = 0f;

            var fireball = Instantiate(fireBallPrefab);
            fireball.position = transform.position + spawnLocationOffset;
            fireball.name = "Fireball";
            //var temp = fireball.gameObject.AddComponent<ProjectileForward>();

            if(attackType == 0)
            {
                var temp = fireball.gameObject.AddComponent<ProjectileForward>();
                temp.direction = 1;
                temp.ProjectileMovement(transform.rotation);
            }
            else if (attackType == 1)
            {
                var temp = fireball.gameObject.AddComponent<EnemyProjectileAttack>();
                temp.fireBallPrefab = fireBallPrefab;
            }
            else if (attackType == 2)
            {
                var temp = fireball.gameObject.AddComponent<ProjectileAttackSplitInArc>();
                temp.fireBallPrefab = fireBallPrefab;
                temp.splitNumber = 4;
                temp.ProjectileMovement();
            }
            else if (attackType == 3)
            {
                var temp = fireball.gameObject.AddComponent<ProjectileAttackSplitTowardsPlayer>();
                temp.fireBallPrefab = fireBallPrefab;
                temp.splitNumber = 4;
                //temp.ProjectileMovement();
            }
        }
    }
}
