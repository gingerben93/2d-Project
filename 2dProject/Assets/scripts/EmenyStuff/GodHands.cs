using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodHands : MonoBehaviour {

    GodHandsFist godHandsFist;

    public Transform fistPrefab;

    Transform fist;

    public void Attack(Vector3 targetLocation)
    {
        fist = Instantiate(fistPrefab, transform);
        fist.name = "Fist";

        godHandsFist = transform.GetComponentInChildren<GodHandsFist>();

        godHandsFist.pullAttackOn = true;
        godHandsFist.newTargetLocation = targetLocation;

        //as child of boss

        godHandsFist.gotoPosition = transform.position;
        godHandsFist.userTransform = fist.GetComponent<Transform>();
        godHandsFist.userTransform.position = transform.position;

        Debug.Log(transform.position + "transform.position");
        Debug.Log(targetLocation + "targetLocation");
        Debug.Log(godHandsFist.userTransform.position + "godHandsFist.userTransform.position");
    }
}
