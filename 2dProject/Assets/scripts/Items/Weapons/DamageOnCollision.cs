using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour {

    public int damage;

    //set this on the game object; or from a different script like the weapon that is attacking
    public float invincibleTimer = 0;

    //stuff can only get hit once by anything
    //public List<int> hasHit = new List<int>();

    //for player attack
    public delegate void OnCollide();
    public OnCollide onCollide;

    // coudl try some sort of cooldown system for hitting stuff
    //void Update()
    //{
    //    if(collideCoolDown > 0f)
    //    {
    //        collideCoolDown -= Time.deltaTime;
    //    }
    //}


    //recursive function check object then parent until stats found; explodes with they don't got stats
    EnemyStats FindEnemyStat(GameObject obj)
    {
        EnemyStats STATS;
        if (STATS = obj.GetComponent<EnemyStats>())
        {
            Debug.Log(obj.name);
            return STATS;
        }
        else
        {
            return FindEnemyStat(obj.transform.parent.gameObject);
        }
    }

    // a lot cheaper to make this enter
    void OnTriggerEnter2D(Collider2D otherCollider)
    //void OnTriggerStay2D(Collider2D otherCollider)
    {
        //if change to OnTriggerStay2D this make sure things only get hit once add if statement
        //hasHit.Add(otherCollider.GetInstanceID());

        Debug.Log(otherCollider.name);
        Debug.Log(otherCollider.tag);

        if (otherCollider.tag == "Enemy")
        {

            //find enemy stats if it has it
            EnemyStats Enemy = FindEnemyStat(otherCollider.gameObject);

            if (Enemy)
            {
                Enemy.Damage(damage, otherCollider.name, invincibleTimer);
            }
            else
            {
                Debug.Log("NO STATS FOUND");
            }

            if (onCollide != null)
            {
                onCollide();
            }
        }
        else
        {
            if (onCollide != null)
            {
                onCollide();
            }
        }
    }
}
