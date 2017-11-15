using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        var difference = PlayerController.PlayerControllerSingle.transform.position - transform.position;
        var grapBodyAngle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        var bodyRoatation = Quaternion.AngleAxis(grapBodyAngle, Vector3.forward);
        transform.rotation = bodyRoatation;
    }
}
