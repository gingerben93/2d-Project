using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderEnemyAttack : MonoBehaviour {

    bool collide = false;
    public bool attacking = false;
    bool reverseAttack = false;

    //for attack collider
    BoxCollider2D col;
    SpriteRenderer SpriteRender;
    SliderEnemyMovment MovementScript;
    EnemyStats StatScript;

    // Use this for initialization
    void Start ()
    {
        col = transform.parent.Find("Collider").GetComponent<BoxCollider2D>();
        SpriteRender = transform.parent.Find("Collider").GetComponent<SpriteRenderer>();
        MovementScript = transform.parent.GetComponent<SliderEnemyMovment>();
        StatScript = transform.parent.GetComponent<EnemyStats>();
        //SpriteRender.enabled = false;
        //col.enabled = false;

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
                reverseAttack = false;

                transform.position = transform.parent.transform.position;
                transform.localPosition += new Vector3(0, .4f, 0);
                col.size = Vector2.one;
                col.transform.position = transform.parent.transform.position;
                SpriteRender.size = Vector2.one;

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

            reverseAttack = true;
            attacking = false;
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
            reverseAttack = true;
        }
        //reverseAttack = true;

    }

    public void StartAttack()
    {
        SpriteRender.enabled = true;
        col.enabled = true;

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
        col.size = new Vector2(1, Vector2.Distance(transform.parent.transform.position, transform.position));
        col.transform.position = (transform.parent.transform.position + transform.position) / 2;

        SpriteRender.size = new Vector2(1, Vector2.Distance(transform.parent.transform.position, transform.position));
    }

}
