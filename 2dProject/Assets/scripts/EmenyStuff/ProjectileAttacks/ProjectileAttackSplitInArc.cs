using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackSplitInArc : MonoBehaviour {

    //split time
    private float splitCoolDown = .5f;

    //number of times to split
    public int splitNumber = 0;

    //object speed
    float speed = 6f;

    public Vector3 GoalHeading;

    //prefab
    public Transform fireBallPrefab;

    void Start()
    {
        //if (transform.position.z == 0f)
        //{
        //    ProjectileMovement();
        //}
        //Last for 3 seconds 
        Destroy(gameObject, 1f);
        //ProjectileMovement();
    }

    void Update()
    {
        //Move projectile
        transform.position += transform.right * speed * Time.deltaTime;

        if (splitNumber > 0)
        {
            //if want split before destroy
            if (splitCoolDown > 0.0f)
            {
                splitCoolDown -= Time.deltaTime;
            }
            else
            {
                splitCoolDown = 2f;

                var fireball = Instantiate(fireBallPrefab);
                fireball.position = transform.position;
                fireball.rotation = transform.rotation;
                fireball.name = "Fireball";
                var temp = fireball.gameObject.AddComponent<ProjectileAttackSplitInArc>();
                temp.fireBallPrefab = fireBallPrefab;
                temp.splitNumber = splitNumber - 1;
                temp.transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 30);

                var fireball2 = Instantiate(fireBallPrefab);
                fireball2.position = transform.position;
                fireball2.rotation = transform.rotation;
                fireball2.name = "Fireball";
                var temp2 = fireball2.gameObject.AddComponent<ProjectileAttackSplitInArc>();
                temp2.fireBallPrefab = fireBallPrefab;
                temp2.splitNumber = splitNumber - 1;
                temp2.transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - 30);
            }
        }
    }

    public void ProjectileMovement()
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
