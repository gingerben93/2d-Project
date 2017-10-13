using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCrystal : MonoBehaviour {

    EnemyStats Stats;
	// Use this for initialization
	void Start ()
    {
        Stats = transform.parent.GetComponent<EnemyStats>();
	}

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        //if bullet, do bullet stuff
        if (otherCollider.tag == "Bullet")
        {
            Destroy(otherCollider.gameObject);
            Stats.Damage(PlayerController.PlayerControllerSingle.weaponDamage);
        }
    }

}
