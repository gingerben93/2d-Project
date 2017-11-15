using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour {

    public int damage { get; set; }

    //for player attack
    public delegate void OnCollide();
    public OnCollide onCollide;

    //// Use this for initialization
    //void Start ()
    //   {

    //}

    //// Update is called once per frame
    //void Update ()
    //   {

    //}

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        //if bullet, do bullet stuff
        if (otherCollider.tag == "Enemy")
        {
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
                Enemy.Damage(damage);
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
