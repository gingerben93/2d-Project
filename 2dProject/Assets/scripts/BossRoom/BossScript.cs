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

        hp = 4;
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

    public bool shieldOn = false;




    void Update()
    {
        //for checking how far the player is
        distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= 10)
        {
            playersLastLocations = playerTransform.position;
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

            playersLastLocations = playerTransform.position;
            bossAttack.AttackBoss(playersLastLocations);
            
        }
    }

    /// Inflicts damage and check if the object should be destroyed
    public void Damage(int damageCount)
    {
        //Cant be damage when shield is up
        if (shieldOn)
        {

        }
        else
        {
            hp -= damageCount;

            if (hp <= 0)
            {
                //set exp
                PlayerStats.PlayerStatsSingle.experiencePoints += experiencePoint;

                //Boss1 is now dead
                GameController.GameControllerSingle.Boss1 = true;
                MainQuest8_0 mainQuest = GameController.GameControllerSingle.transform.GetComponent<MainQuest8_0>();
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
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        //if bullet, do bullet stuff
        if (otherCollider.tag == "Bullet" && transform.tag == "Enemy")
        {
            Debug.Log("HIT by Bullet");
            Destroy(otherCollider.gameObject);
            Damage(GameController.GameControllerSingle.damage);
        }
    }



}
