using UnityEngine;
using System.Collections;

public class ShotMove : MonoBehaviour
{
    public GameObject player;

    private Vector3 mousePos;
    private Vector3 heading;
    private float distance;
    private Vector3 direction;

    private Vector2 movement;
    private Rigidbody2D rigidbodyComponent;
    // Update is called once per frame

    void Start()
    {
        transform.GetComponent<DamageOnCollision>().onCollide = onCollide;
        Shoot();
    }

    void FixedUpdate()
    {
        if (rigidbodyComponent == null) rigidbodyComponent = GetComponent<Rigidbody2D>();

        // Apply movement to the rigidbody
        rigidbodyComponent.velocity = movement;
    }

    void onCollide()
    {
        Destroy(gameObject);
    }

    void Shoot()
    {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        heading = mousePos - PlayerController.PlayerControllerSingle.transform.position;
        distance = heading.magnitude;
        direction = heading / distance;

        movement = new Vector2(10 * direction.x, 10 * direction.y);
    }

    //void OnTriggerEnter2D(Collider2D otherCollider)
    //{
    //    //if bullet, do bullet stuff
    //    if (otherCollider.tag == "Enemy")
    //    {
    //        EnemyStats Enemy;
    //        //might needs to also look in children of gameobjects fi this ever fails
    //        if(Enemy = otherCollider.gameObject.GetComponent<EnemyStats>())
    //        {

    //        }
    //        else
    //        {
    //            Enemy = otherCollider.gameObject.transform.parent.GetComponent<EnemyStats>();
    //        }

    //        if (Enemy)
    //        {
    //            Enemy.Damage(PlayerController.PlayerControllerSingle.weaponDamage);
    //        }
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}
}
