using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour {

    // Update is called once per frame

    bool startTimer = false;
    private float timer = 0;

    private Transform player;
    private Transform BossLocation;
    private float distanceToPlayer;

    void Start()
    { 
        player = GameController.GameControllerSingle.transform;
        BossLocation = gameObject.GetComponentInParent<Transform>();
    }

	void Update ()
    {
        distanceToPlayer = Vector3.Distance(BossLocation.position, player.position);

        if (distanceToPlayer <= 10)
        {
            startTimer = true;
        }
        if (startTimer)
        {
            timer += Time.deltaTime;
        }
        if(timer >= 5)
        {
            startTimer = false;
            timer = 0;
            PullAttack();
        }
    }

    void PullAttack()
    {

    }

}
