using UnityEngine;

public class DamagePlayerOnCollision : MonoBehaviour
{
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GameObject.Find("Hero").GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerStats.PlayerStatsSingle.health -= 1;
            rb2d.AddForce(new Vector2(0f, -500f));
        }
    }
}
