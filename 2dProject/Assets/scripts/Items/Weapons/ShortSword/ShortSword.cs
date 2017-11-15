using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortSword : MonoBehaviour {

    public float weaponAttackRate = 0.25f;
    private float weaponCooldown;

    SpriteRenderer Image;
    BoxCollider2D ObjectCollider;

    void Start()
    {
        //set damage
        transform.GetComponent<DamageOnCollision>().damage = 1;
        transform.GetComponent<DamageOnCollision>().onCollide = onCollide;

        Image = gameObject.GetComponentInChildren<SpriteRenderer>();
        ObjectCollider = gameObject.GetComponent<BoxCollider2D>();
        weaponCooldown = 0f;
    }

    void Update()
    {
        if (weaponCooldown > 0)
        {
            weaponCooldown -= Time.deltaTime;

            if (PlayerController.PlayerControllerSingle.facingRight)
            {
                transform.position = PlayerController.PlayerControllerSingle.transform.position + PlayerController.PlayerControllerSingle.transform.right;
            }
            else
            {
                transform.position = PlayerController.PlayerControllerSingle.transform.position - PlayerController.PlayerControllerSingle.transform.right;
            }
        }
        else
        {
            Image.enabled = false;
            ObjectCollider.enabled = false;
        }
    }

    public void Attack()
    {
        if (CanAttack)
        {
            weaponCooldown = weaponAttackRate;
            Image.enabled = true;
            ObjectCollider.enabled = true;
        }
    }

    public bool CanAttack
    {
        get
        {
            return weaponCooldown <= 0f;
        }
    }

    void onCollide()
    {
        Image.enabled = false;
        ObjectCollider.enabled = false;
        weaponCooldown = weaponAttackRate;
    }

    //void OnTriggerEnter2D(Collider2D otherCollider)
    //{
    //    //if bullet, do bullet stuff
    //    if (otherCollider.tag == "Enemy")
    //    {
    //        EnemyStats Enemy;
    //        //might needs to also look in children of gameobjects fi this ever fails
    //        if (Enemy = otherCollider.gameObject.GetComponent<EnemyStats>())
    //        {

    //        }
    //        else
    //        {
    //            Enemy = otherCollider.gameObject.transform.parent.GetComponent<EnemyStats>();
    //        }

    //        Enemy.Damage(PlayerController.PlayerControllerSingle.weaponDamage);
    //    }
    //}

}
