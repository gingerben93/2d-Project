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
        Shoot();
    }

    void FixedUpdate()
    {
        if (rigidbodyComponent == null) rigidbodyComponent = GetComponent<Rigidbody2D>();

        // Apply movement to the rigidbody
        rigidbodyComponent.velocity = movement;
    }

    void Shoot()
    {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        heading = mousePos - GameController.GameControllerSingle.transform.position;
        distance = heading.magnitude;
        direction = heading / distance;

        movement = new Vector2(10 * direction.x, 10 * direction.y);
    }

}
