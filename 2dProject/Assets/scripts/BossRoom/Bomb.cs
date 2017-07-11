using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public Rigidbody2D rb2d;
    public GameObject shield;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            rb2d.AddForce(new Vector2(0f, 1000f));
            Destroy(gameObject);
            shield.SetActive(false);
            BossScript.BossScriptSingle.shieldOn = false;
            GameController.GameControllerSingle.stun = false;
            if (GameObject.FindGameObjectWithTag("EnemyProjectile") != null)
            {
                Destroy(GameObject.FindGameObjectWithTag("EnemyProjectile"));
            }
        }

    }
}
