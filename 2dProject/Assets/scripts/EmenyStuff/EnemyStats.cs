using UnityEngine;
using System.Collections;

/// Handle hitpoints and damages
public class EnemyStats : MonoBehaviour
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
    }

    /// Inflicts damage and check if the object should be destroyed
    public void Damage(int damageCount)
    {
        hp -= damageCount;

        if (hp <= 0)
        {
            //set exp
            PlayerStats.PlayerStatsSingle.experiencePoints += experiencePoint;

            //Increase Kill Counter
            if (QuestController.QuestControllerSingle.killQuestList.ContainsKey("Enemy"))
            {
                QuestController.QuestControllerSingle.KillQuestCounter += 1;
                QuestController.QuestControllerSingle.UpdateKillQuest("Enemy");
            }

            Instantiate(loot, transform.position, Quaternion.identity).transform.parent = (GameObject.Find("WorldItems")).transform;
            Instantiate(loot2, transform.position, Quaternion.identity).transform.parent = (GameObject.Find("WorldItems")).transform;
            Instantiate(loot3, transform.position, Quaternion.identity).transform.parent = (GameObject.Find("WorldItems")).transform;
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
