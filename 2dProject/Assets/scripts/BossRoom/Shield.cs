using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {


    public Rigidbody2D rb2d;
    public bool notGrabbed = true;


    public static Shield ShieldSingle;

    void Awake()
    {
        if (ShieldSingle == null)
        {
            ShieldSingle = this;
        }
        else if (ShieldSingle != this)
        {
            Destroy(gameObject);
        }

        BossScript.BossScriptSingle.shieldOn = true;
    }

    void Update()
    {
        //Cheks to see if Player is grabbed, is so then wait 2 seconds before reactivating shield and player collision
        if (!notGrabbed)
        {
            StartCoroutine(Grabbed());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (notGrabbed)
            {
                GameController.GameControllerSingle.stun = true;
                Debug.Log(GameController.GameControllerSingle.stun);
                Debug.Log(collision.gameObject.tag);
                // Calculate Angle Between the collision point and the player
                var dir = collision.transform.position - transform.position;
                // We then get the opposite (-Vector3) and normalize it
                dir.Normalize();
                // And finally we add force in the direction of dir and multiply it by force. 
                // This will push back the player
                if (dir.x < 0.1)
                {
                    rb2d.AddForce(new Vector2(-500f, 0));
                }
                else
                {
                    rb2d.AddForce(new Vector2(dir.x * 500f, 0));
                }
                StartCoroutine(StopPlayerControls());
            }

        }

        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject);
        }
    }

    IEnumerator StopPlayerControls()
    {
        yield return new WaitForSeconds(2);
        GameController.GameControllerSingle.stun = false;
    }

    IEnumerator Grabbed()
    {
        yield return new WaitForSeconds(1);
        notGrabbed = true;
    }
}
