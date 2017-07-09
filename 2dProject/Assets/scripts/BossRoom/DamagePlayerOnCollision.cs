using UnityEngine;

public class DamagePlayerOnCollision : MonoBehaviour
{
    public Rigidbody2D rb2d;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerStats.PlayerStatsSingle.health -= 1;
            rb2d.AddForce(new Vector2(0f, -50f));
        }
    }
}
