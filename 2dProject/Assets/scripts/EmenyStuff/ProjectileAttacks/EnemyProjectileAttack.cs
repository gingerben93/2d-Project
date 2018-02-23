using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileAttack : MonoBehaviour {

    //prefab
    public Transform fireBallPrefab;

    //object speed
    float speed = 6f;

    void Start()
    {
        //Last for 3 seconds 
        Destroy(gameObject, 3.0f);
        ProjectileMovement();
    }

    void Update()
    {
        //Move projectile towards where the player was
        //transform.Translate(-transform.right * .1f);

        //gameObjectRB.velocity = movement;
        //transform.position = Vector2.MoveTowards(transform.position, PlayerController.PlayerControllerSingle.transform.position*2, .3f);

        transform.position += transform.right * speed * Time.deltaTime;
    }

    void ProjectileMovement()
    {
        //point toward player
        var dir = PlayerController.PlayerControllerSingle.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Destroy(gameObject);
    }
}
