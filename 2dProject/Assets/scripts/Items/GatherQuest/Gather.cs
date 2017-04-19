using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gather : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            QuestController.QuestControllerSingle.GatherQuestCounter += 1;
            QuestController.QuestControllerSingle.UpdateGatherQuest("ManaPotion");
            Destroy(gameObject);
        }
    }
}
