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
            GatherQuest temp;
            if (temp = QuestController.QuestControllerSingle.transform.GetComponent<GatherQuest>())
            {
                temp.GatherQuestCounter += 1;
                temp.UpdateGatherQuest("ManaPotion");
            }
            
            Destroy(gameObject);
        }
    }
}
