using UnityEngine;
using System.Collections;

/// Handle hitpoints and damages
public class EnemyStats : MonoBehaviour
{
    /// Total hitpoints
    public int hp = 1;

    public bool isEnemy = true;

    /// Inflicts damage and check if the object should be destroyed
    public void Damage(int damageCount)
    {
        hp -= damageCount;

        if (hp <= 0)
        {
            // Dead!
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // Is this a shot?
        Shot shot = otherCollider.gameObject.GetComponent<Shot>();
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
