using UnityEngine;

public class Blowdart : MonoBehaviour
{
    private GameObject shotPrefab;
    public float shootingRate = 0.25f;

    private float shootCooldown;

    void Start()
    {
        shotPrefab = Resources.Load("Prefabs/WeaponProjectiles/Bullet", typeof(GameObject)) as GameObject;
        shootCooldown = 0f;
    }

    void LateUpdate()
    {
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
    }

    public void Attack(/*bool isEnemy*/)
    {
        //Debug.Log(GameController.GameControllerSingle.damage);
        if (CanAttack)
        {
            shootCooldown = shootingRate;

            // Create a new shot
            GameObject shotTransform = Instantiate(shotPrefab, GameObject.Find("PlayerProjectiles").transform) as GameObject;
            // Assign position
            shotTransform.transform.position = transform.position;

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