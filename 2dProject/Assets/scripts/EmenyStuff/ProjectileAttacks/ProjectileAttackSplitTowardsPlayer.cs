using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackSplitTowardsPlayer : MonoBehaviour {

    //split time
    private float splitCoolDown = 2f;

    //number of times to split
    public int splitNumber = 0;

    //object speed
    float speed = 6f;

    public Transform fireBallPrefab;

    void Start()
    {
        //Last for 3 seconds 
        Destroy(gameObject, 3.0f);
        ProjectileMovement();
    }

    void Update()
    {
        //Move projectile towards forward
        transform.position += transform.right * speed * Time.deltaTime;

        if (splitNumber > 0)
        {
            //if want split before destory
            if (splitCoolDown > 0.0f)
            {
                splitCoolDown -= Time.deltaTime;
            }
            else
            {
                splitCoolDown = 2f;
                var fireball = Instantiate(fireBallPrefab, transform.parent.transform);
                fireball.position = transform.position;
                fireball.name = "Fireball";
                var temp = fireball.gameObject.AddComponent<ProjectileAttackSplitTowardsPlayer>();
                temp.fireBallPrefab = fireBallPrefab;
                temp.splitNumber = splitNumber - 1;
            }
        }
    }

    void ProjectileMovement()
    {
        var dir = PlayerController.PlayerControllerSingle.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Destroy(gameObject);
    }
}
