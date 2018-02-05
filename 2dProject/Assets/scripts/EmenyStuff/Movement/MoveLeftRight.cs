using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftRight : MonoBehaviour
{

    private Rigidbody2D RigBod;

    private float Speed = 5f;

    // Use this for initialization
    void Start()
    {
        RigBod = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        RigBod.velocity = new Vector2(Speed, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.layer);
        if (collision.gameObject.layer == 8)
        {
            Speed *= -1;
        }
    }
}
