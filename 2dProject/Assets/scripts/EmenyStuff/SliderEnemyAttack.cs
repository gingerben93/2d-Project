using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderEnemyAttack : MonoBehaviour {

    bool collide = false;
    public bool attacking = false;
    bool reverseAttack = false;

    //for attack collider
    BoxCollider2D tipCollider;
    SpriteRenderer tipImage;
    SliderEnemyMovment MovementScript;
    EnemyStats StatScript;

    //Bubble parts
    SpriteRenderer bubbleImage;
    PolygonCollider2D bubbleCollider;

    // Use this for initialization
    void Start ()
    {

        //collider parts
        tipCollider = transform.parent.Find("Collider").GetComponent<BoxCollider2D>();
        tipImage = transform.parent.Find("Collider").GetComponent<SpriteRenderer>();

        //for attacking
        MovementScript = transform.parent.GetComponent<SliderEnemyMovment>();

        //for making invincible
        StatScript = transform.parent.GetComponent<EnemyStats>();

        //bubble parts
        bubbleImage = transform.parent.Find("Bubble").gameObject.GetComponent<SpriteRenderer>();
        bubbleCollider = transform.parent.Find("Bubble").gameObject.GetComponent<PolygonCollider2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!collide && attacking)
        {
            transform.localPosition += Vector3.up * .2f;

            SizeObjects();
        }
        else if (reverseAttack)
        {
            if (Vector2.Distance(transform.parent.transform.position, transform.position) <= .4f)
            {
                //for stopping loop after object attack is close
                reverseAttack = false;

                transform.position = transform.parent.transform.position;
                transform.localPosition += new Vector3(0, .4f, 0);
                tipCollider.size = Vector2.one;
                tipCollider.transform.position = transform.parent.transform.position;
                tipImage.size = Vector2.one;

                attacking = false;
                MovementScript.attacking = false;
                collide = false;
            }
            else
            {
                transform.localPosition += Vector3.down * .2f;

                SizeObjects();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Bullet")
        {
            //make enemy killable
            StatScript.invincible = false;

            //reverse direction of attack
            reverseAttack = true;
            attacking = false;

            //bubble parts off
            bubbleImage.enabled = false;
            bubbleCollider.enabled = false;
        }
        if (attacking)
        {
            collide = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //take out of collide for some action
        if (collide)
        {
            //make enemy killable
            StatScript.invincible = false;

            //reverse direction of attack
            reverseAttack = true;
            attacking = false;

            //bubble parts off
            bubbleImage.enabled = false;
            bubbleCollider.enabled = false;
        }
        //reverseAttack = true;

    }

    public void StartAttack()
    {
        //collider parts
        tipImage.enabled = true;
        tipCollider.enabled = true;

        //bubble parts
        bubbleImage.enabled = true;
        bubbleCollider.enabled = true;

        //move slighty forward so it doesn't break if it collides right away (from a 0 distance move)
        transform.localPosition += Vector3.up * .4f;

        //don't need to get angle when you're object is a child because of local frame of reference
        //get angle
        //float angle = (Mathf.Abs(transform.parent.transform.position.y - transform.position.y) / Mathf.Abs(transform.parent.transform.position.x - transform.position.x));
        //if ((transform.parent.transform.position.y < transform.position.y && transform.parent.transform.position.x > transform.position.x) || (transform.position.y < transform.parent.transform.position.y && transform.position.x > transform.parent.transform.position.x))
        //{
        //    angle *= -1;
        //}
        //angle = Mathf.Rad2Deg * Mathf.Atan(angle);
        //col.transform.Rotate(0, 0, angle);

        //starts attack

        //so enemy can't die
        StatScript.invincible = true;

        //set to attacking mode
        attacking = true;
    }

    void SizeObjects()
    {
        //size and position of attack body
        tipCollider.size = new Vector2(1, Vector2.Distance(transform.parent.transform.position, transform.position));
        tipCollider.transform.position = (transform.parent.transform.position + transform.position) / 2;

        tipImage.size = new Vector2(1, Vector2.Distance(transform.parent.transform.position, transform.position));
    }

}
