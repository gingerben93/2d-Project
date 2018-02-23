using UnityEngine;

public class Blowdart : Weapon
{
    public GameObject shotPrefab;

    public float shootingRate = 0.25f;
    private float shootCooldown;

    void Start()
    {
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
        }
    }

    void OnDestroy()
    {
        PlayerController.PlayerControllerSingle.playerAttack -= Attack;
    }

    public bool CanAttack
    {
        get
        {
            return shootCooldown <= 0f;
        }
    }
}