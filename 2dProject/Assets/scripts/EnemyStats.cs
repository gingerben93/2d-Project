using UnityEngine;
using System.Collections;

/// Handle hitpoints and damages
public class EnemyStats : MonoBehaviour
{
    public Transform loot;

    Shot shot;

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
            PlayerStats.playerStatistics.experiencePoints += experiencePoint;

            Instantiate(loot, transform.position, Quaternion.identity).transform.parent = (GameObject.Find("WorldItems")).transform; ;
            // Dead!
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerStats.playerStatistics.health -= 1;
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // Is this a shot?
        shot = otherCollider.gameObject.GetComponent<Shot>();
        if (shot != null)
        {
            // Avoid friendly fire
            if (shot.isEnemyShot != isEnemy)
            {
                Damage(shot.damage);

                // Destroy the shot
                Destroy(shot.gameObject); // Remember to always target the game object, otherwise you will just remove the script
            }
        }
    }
}
