using UnityEngine;
using System.Collections;

/// Handle hitpoints and damages
public class BossScript : MonoBehaviour
{
    public Transform loot;
    public Transform loot2;
    public Transform loot3;

    Shot shot;
    ShotM shotM;

    /// Total hitpoints
    public int hp { get; set; }
    public bool isEnemy { get; set; }
    private int experiencePoint;

    

    void Start()
    {
        // set emeny information
        experiencePoint = 5;

        hp = 1;
        isEnemy = true;

        //for boss attack
        playerTransform = GameController.GameControllerSingle.transform;
        bossAttack = GetComponentInChildren<GodHands>();

}

    GodHands bossAttack;

    bool startTimer = false;

    private float timer = 0;
    private float distanceToPlayer;

    private Vector3 playersLastLocations;

    private Transform playerTransform;

    

    void Update()
    {
        //Debug.Log(BossLocation.position);
        distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= 10)
        {
            playersLastLocations = playerTransform.position;
            startTimer = true;
        }
        if (startTimer)
        {
            timer += Time.deltaTime;
        }
        if (timer >= 5)
        {
            startTimer = false;
            timer = 0;

            playersLastLocations = playerTransform.position;
            bossAttack.Attack(playersLastLocations);
            
        }
    }

    /// Inflicts damage and check if the object should be destroyed
    public void Damage(int damageCount)
    {
        hp -= damageCount;

        if (hp <= 0)
        {
            //set exp
            PlayerStats.PlayerStatsSingle.experiencePoints += experiencePoint;

            //Boss1 is now dead
            GameController.GameControllerSingle.Boss1 = true;


            //Instantiate(loot, transform.position, Quaternion.identity).transform.parent = (GameObject.Find("WorldItems")).transform;
            //Instantiate(loot2, transform.position, Quaternion.identity).transform.parent = (GameObject.Find("WorldItems")).transform;
            //Instantiate(loot3, transform.position, Quaternion.identity).transform.parent = (GameObject.Find("WorldItems")).transform;
            // Dead!
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerStats.PlayerStatsSingle.health -= 1;
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // Is this a shot?
        shot = otherCollider.gameObject.GetComponent<Shot>();
        shotM = otherCollider.gameObject.GetComponent<ShotM>();
        if (shot != null)
        {
            // Avoid friendly fire
            if (shot.isEnemyShot != isEnemy)
            {
                Damage(GameController.GameControllerSingle.damage);

                // Destroy the shot
                Destroy(shot.gameObject); // Remember to always target the game object, otherwise you will just remove the script
            }
        }
        else if (shotM != null)
        {
            // Avoid friendly fire
            if (shotM.isEnemyShot != isEnemy)
            {
                Damage(GameController.GameControllerSingle.damage);

                // Destroy the shot
                Destroy(shotM.gameObject); // Remember to always target the game object, otherwise you will just remove the script
            }
        }
    }
}
