using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttack : MonoBehaviour {

    EnemyProjectileAttack enemyProjectile;

    bool startTimer = false;

    private float timer = 0;
    private float distanceToPlayer;

    private Vector3 playersLastLocations;

    public Transform playerTransform;
    public Transform fireBallPrefab;

    // Use this for initialization
    void Start () {
        //for turret attack
        playerTransform = GameController.GameControllerSingle.transform;

    }
	
	// Update is called once per frame
	void Update () {
        //for checking how far the player is
        distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

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
        if (timer >= 2f)
        {
            startTimer = false;
            timer = 0f;

            var fireball = Instantiate(fireBallPrefab, transform);
            fireball.position = transform.position;
            fireball.name = "Fireball";
        }
    }
}
