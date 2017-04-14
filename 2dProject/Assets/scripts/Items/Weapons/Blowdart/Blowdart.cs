using UnityEngine;

public class Blowdart : MonoBehaviour
{
    public Transform shotPrefab;
    public float shootingRate = 0.25f;

    private float shootCooldown;
    private Transform holdForBullets;

    void Start()
    {
        shootCooldown = 0f;
        //rotation = transform.rotation;
        holdForBullets = GameObject.Find("PlayerProjectiles").transform;
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
        Debug.Log(GameController.GameControllerSingle.damage);
        if (CanAttack)
        {
            shootCooldown = shootingRate;

            // Create a new shot
            var shotTransform = Instantiate(shotPrefab) as Transform;
            shotTransform.transform.SetParent(holdForBullets);
            // Assign position
            shotTransform.position = transform.position;

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