using UnityEngine;

public class Blowdart : Weapon
{
    public GameObject shotPrefab;

    public float shootingRate = 0.25f;
    private float shootCooldown;

    void Start()
    {
        //shotPrefab = Resources.Load("Prefabs/WeaponProjectiles/Bullet", typeof(GameObject)) as GameObject;
        shootCooldown = 0f;
    }

    void LateUpdate()
    {
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
    }

    public override void Attack()
    {
        //Debug.Log(GameController.GameControllerSingle.damage);
        if (CanAttack)
        {
            shootCooldown = shootingRate;

            // Create a new shot
            GameObject shotTransform = Instantiate(shotPrefab, GameObject.Find("PlayerProjectiles").transform) as GameObject;

            // Assign bullet information; location and damage
            shotTransform.transform.position = transform.position;
            shotTransform.GetComponent<DamageOnCollision>().damage = PlayerStats.PlayerStatsSingle.strength;


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