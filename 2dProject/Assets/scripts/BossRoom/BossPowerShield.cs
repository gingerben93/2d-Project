using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPowerShield : MonoBehaviour {

    public bool notGrabbed = true;
    public Rigidbody2D boss;

    public static BossPowerShield BossPowerShieldSingle;

    void Awake()
    {
        if (BossPowerShieldSingle == null)
        {
            BossPowerShieldSingle = this;
        }
        else if (BossPowerShieldSingle != this)
        {
            Destroy(gameObject);
        }

        
    }
    
    void Start()
    {
        boss = GameObject.Find("Boss").GetComponent<Rigidbody2D>();
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
                PlayerController.PlayerControllerSingle.freezePlayer = true;
                Debug.Log(collision.gameObject.tag);
                // Calculate Angle Between the collision point and the player
                var dir = collision.transform.position - transform.position;
                // We then get the opposite (-Vector3) and normalize it
                dir.Normalize();
                // And finally we add force in the direction of dir and multiply it by force. 
                // This will push back the player
                Debug.Log(dir.x);
                if(dir.x < 0.01f && dir.y >= 0.0f)
                {
                    collision.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                    collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(-500f, 500f));
                }
                else if (dir.x < 0.01f && dir.y < 0.0f)
                {
                    collision.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                    collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(-500f, 500f));
                }
                else
                {
                    collision.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                    collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir.x * 2000f, 500f));

                }
                BossScript.BossScriptSingle.charge = true;
                boss.GetComponent<Rigidbody2D>().velocity = new Vector3(0, transform.GetComponent<Rigidbody2D>().velocity.y, 0);
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
        PlayerController.PlayerControllerSingle.freezePlayer = false;
    }

    IEnumerator Grabbed()
    {
        yield return new WaitForSeconds(1);
        notGrabbed = true;
    }
}
