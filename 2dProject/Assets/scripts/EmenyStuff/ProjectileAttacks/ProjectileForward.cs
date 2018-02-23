using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileForward : MonoBehaviour {

    //object speed
    float speed = 6f;
    public float direction = 1;

    // Use this for initialization
    void Start ()
    {
        if(direction == -1)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        //Last for 3 seconds 
        Destroy(gameObject, 3.0f);
        //ProjectileMovement();
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += direction*transform.right * speed * Time.deltaTime;
    }
    public void ProjectileMovement(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Destroy(gameObject);
    }
}
