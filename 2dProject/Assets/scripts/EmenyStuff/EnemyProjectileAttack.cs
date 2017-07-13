using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileAttack : MonoBehaviour {

    //Where the projectile is going towards
    private Vector3 heading;
    private float distance;
    private Vector3 direction;

    //Finalized movement of projectile
    private Vector2 movement;

    //Where the hero is located
    public Transform playerTransform;


    void Start()
    {
        //for turret attack
        playerTransform = GameController.GameControllerSingle.transform;

        //Last for 3 seconds 
        Destroy(gameObject, 3.0f);
        ProjectileMovement();
    }

    void Update()
    {
        //Move projectile towards where the player was
        gameObject.GetComponent<Rigidbody2D>().velocity = movement;
    }

    void ProjectileMovement()
    {
        //Calculate where to fire the projectile
        heading = playerTransform.position - transform.position;
        distance = heading.magnitude;
        direction = heading / distance;

        //Where it needs to go! Change the multiplies to change speed
        movement = new Vector2(10 * direction.x, 10 * direction.y);
    }

}
