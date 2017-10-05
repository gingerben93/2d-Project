using UnityEngine;
using System.Collections;

/// Handle hitpoints and damages
public class BossScript : MonoBehaviour
{
    public Transform loot;
    public Transform loot2;
    public Transform loot3;

    //Drop bomb when shield is activate
    public Transform bomb;

    Shot shot;
    ShotM shotM;

    /// Total hitpoints
    public bool isEnemy { get; set; }
    private int experiencePoint;

    //shield bools
    public GameObject shield;
    public MoveTowardPlayer movement;
    private bool shield1 = true;
    private bool shield2 = true;
    private bool shield3 = true;

    //Accessing Healthbar Script on boss
    public Healthbar bossHealth;

    GodHands bossAttack;

    bool startTimer = false;

    private float timer = 0;
    private float distanceToPlayer;

    private Vector3 playersLastLocations;

    public bool shieldOn = false;
    public bool charge = true;
    public bool bombSpawn = false;

    public static BossScript BossScriptSingle;

    void Awake()
    {
        if (BossScriptSingle == null)
        {
            DontDestroyOnLoad(gameObject);
            BossScriptSingle = this;
        }
        else if (BossScriptSingle != this)
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        // set emeny information
        experiencePoint = 5;

        bossHealth = GetComponent<Healthbar>();

        bossHealth.maxHealth = 20;
        bossHealth.currentHealth = 20;


        isEnemy = true;

        //for boss attack
        bossAttack = GetComponentInChildren<GodHands>();

        //for movement scipt
        movement = gameObject.GetComponent<MoveTowardPlayer>();
    }



    void Update()
    {
        //for checking how far the player is
        distanceToPlayer = Vector3.Distance(transform.position, PlayerController.PlayerControllerSingle.transform.position);
        if(shieldOn == false) { 
            if (distanceToPlayer <= 10)
            {
                playersLastLocations = PlayerController.PlayerControllerSingle.transform.position;
                startTimer = true;
            }
            //timer for time until attack
            if (startTimer)
            {
                timer += Time.deltaTime;
            }
            if (timer >= 3)
            {
                startTimer = false;
                timer = 0;

                playersLastLocations = PlayerController.PlayerControllerSingle.transform.position;
                bossAttack.AttackBoss(playersLastLocations);

            }
        }
        else //shieldon is true
        {
            if (charge == true)
            {
                StartCoroutine(Charge());
            }
        }
    }

    /// Inflicts damage and check if the object should be destroyed
    public void Damage(int damageCount)
    {
        //Cant be damage when shield is up
        if (shieldOn == false)
        {
            bossHealth.currentHealth -= damageCount;


            //For multiple similar phases of an enemy fight (aka same thing happens multiple times)
            //if (bossHealth.currentHealth / bossHealth.maxHealth <= HEALTHTHREASHHOLD && shield1)
            //{
            //    HEALTHTHREASHHOLD -= .25F
            //}

            //Activate shields when certain thresholds are met.
            if (bossHealth.currentHealth / bossHealth.maxHealth <= .75f && shield1)
            {
                //Make it so this statement cant happen again
                shield1 = false;

                //Set vars for charge and shield is on
                charge = true;
                shieldOn = true;
                
                shield.SetActive(true);
                movement.enabled = false;
                bossHealth.currentHealth = bossHealth.maxHealth * .75f;

                //Instantiate(bomb, new Vector3(12.0f, 0, 0), Quaternion.identity);
            }
            else if(bossHealth.currentHealth / bossHealth.maxHealth <= .5f && shield2)
            {
                //Make it so this statement cant happen again
                shield2 = false;

                //Set vars for charge and shield is on
                charge = true;
                shieldOn = true;
                
                shield.SetActive(true);
                movement.enabled = false;
                bossHealth.currentHealth = bossHealth.maxHealth * .50f;
                //Instantiate(bomb, new Vector3(12.0f, 0, 0), Quaternion.identity);
            }
            else if(bossHealth.currentHealth / bossHealth.maxHealth <= .25f && shield3)
            {
                //Make it so this statement cant happen again
                shield3 = false;

                charge = true;
                shieldOn = true;
                
                shield.SetActive(true);
                movement.enabled = false;
                bossHealth.currentHealth = bossHealth.maxHealth * .25f;
                //Instantiate(bomb, new Vector3(12.0f, 0, 0), Quaternion.identity);
            }
            else if (bossHealth.currentHealth <= 0)
            {
                //set exp
                PlayerStats.PlayerStatsSingle.GainExperiencePoints(experiencePoint);
                //PlayerStats.PlayerStatsSingle.experiencePoints += experiencePoint;

                //Boss1 is now dead
                MainQuest8_0 mainQuest = PlayerController.PlayerControllerSingle.transform.GetComponent<MainQuest8_0>();
                mainQuest.bossDead = true;

                mainQuest.BossSprite = GameObject.Find("Boss").GetComponent<SpriteRenderer>().sprite;
                DialogManager.DialogManagerSingle.TalkingCharacter.sprite = mainQuest.BossSprite;

                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerStats.PlayerStatsSingle.health -= 1;
        }
        if (collision.gameObject.name == "LeftWall")
        {
            transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, transform.GetComponent<Rigidbody2D>().velocity.y, 0);
            transform.GetComponent<Rigidbody2D>().AddForce(new Vector3(5f, 0f, 0));

            //Spawn bomb on other side of the room is charging
            if (bombSpawn == false && shieldOn == true)
            {
                Instantiate(bomb, new Vector3(12.0f, 0, 0), Quaternion.identity);
            }
        }
        if (collision.gameObject.name == "RightWall")
        {
            transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, transform.GetComponent<Rigidbody2D>().velocity.y, 0);
            transform.GetComponent<Rigidbody2D>().AddForce(new Vector3(-5f, 0f, 0));

            //Spawn bomb on other side of the room
            if (bombSpawn == false && shieldOn == true)
            {
                Instantiate(bomb, new Vector3(-12.0f, 0, 0), Quaternion.identity);
            }
        }


    }


    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        //if bullet, do bullet stuff
        if (otherCollider.tag == "Bullet" && transform.tag == "Enemy")
        {
            Debug.Log("HIT by Bullet");
            Destroy(otherCollider.gameObject);
            Damage(PlayerController.PlayerControllerSingle.weaponDamage);
        }
    }


    //For charging left and right
    IEnumerator Charge()
    {
        //Keeps Charging periodic 
        charge = false;
        yield return new WaitForSeconds(3);

        //PLayer to the left of boss
        if (PlayerController.PlayerControllerSingle.transform.position.x > transform.position.x)
        {
            transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, transform.GetComponent<Rigidbody2D>().velocity.y, 0);
            transform.GetComponent<Rigidbody2D>().AddForce(new Vector3(2000f, 0, 0));
            //StartCoroutine(ChargingLeft());
        }
        //player to the right of boss
        else
        {
            transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, transform.GetComponent<Rigidbody2D>().velocity.y, 0);
            transform.GetComponent<Rigidbody2D>().AddForce(new Vector3(-2000f, 0, 0));
            //StartCoroutine(ChargingRight());
        }
      
        charge = true;

    }

    ////For charging left and right
    //IEnumerator ChargingLeft()
    //{
    //    charge = true;
    //    yield return new WaitForSeconds(3);
    //    transform.GetComponent<Rigidbody2D>().AddForce(new Vector3(2000f, 0, 0));

    //}

    //IEnumerator ChargingRight()
    //{
    //    charge = true;
    //    yield return new WaitForSeconds(3);
    //    transform.GetComponent<Rigidbody2D>().AddForce(new Vector3(-2000f, 0, 0));
        
    //}

}
