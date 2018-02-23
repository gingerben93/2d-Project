using UnityEngine;
using System.Collections;

public class ShotMove : MonoBehaviour
{
    private Vector3 mousePos;
    public float speed = 8f;

    void Start()
    {
        speed = Random.Range(6f, 10f);
        Destroy(gameObject, 2);
        transform.GetComponent<DamageOnCollision>().onCollide = onCollide;
        Shoot();
    }

    void FixedUpdate()
    {
        // Apply movement to the rigidbody
        transform.position += transform.right * speed * Time.deltaTime;
    }

    void onCollide()
    {
        Destroy(gameObject);
    }

    void Shoot()
    {
        //mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos += new Vector3(Random.Range(-.2f, .2f), Random.Range(-.2f, .2f), 0);
        var dir = mousePos - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
