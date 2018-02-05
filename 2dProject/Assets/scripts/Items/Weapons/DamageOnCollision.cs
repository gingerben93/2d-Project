using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour {

    public int damage;

    //for player attack
    public delegate void OnCollide();
    public OnCollide onCollide;

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        //if bullet, do bullet stuff
        if (otherCollider.tag == "Enemy")
        {

            //Debug.Log("otherCollider = " + otherCollider.name);
            EnemyStats Enemy;
            //might needs to also look in children of gameobjects fi this ever fails
            if (Enemy = otherCollider.gameObject.GetComponent<EnemyStats>())
            {

            }
            else
            {
                Enemy = otherCollider.gameObject.transform.parent.GetComponent<EnemyStats>();
            }

            if (Enemy)
            {
                Enemy.Damage(damage, otherCollider.name);
            }
            onCollide();
            //Destroy(gameObject);
        }
        else
        {
            onCollide();
            //Destroy(gameObject);
        }
    }
}
