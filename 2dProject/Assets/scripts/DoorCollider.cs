using UnityEngine;
using System.Collections;
using System;

public class DoorCollider : MonoBehaviour {


    public int numVal;
    public void OnTriggerEnter2D(Collider2D node)
    {
        GameController gameCon = FindObjectOfType<GameController>();
        DoorPrefabInfo info = node.GetComponent<DoorPrefabInfo>();
        if (node.gameObject.tag == "Door")
        {
            string oldSeed = info.seedReference;
            string oldDoor = info.doorReference.ToString();
            string dicRef = oldSeed + oldDoor;

            string newDicRef;
            string newSeed;
            string newDoor;

            //Debug.Log("oldSeed = " + oldSeed + "oldDoor = " + oldDoor);

            GameData data = FindObjectOfType<GameData>();
            newDicRef = data.GetDoorInfo(dicRef);

            //Debug.Log("newDicRef = " + newDicRef);

            //set new door and map seed info
            newSeed = newDicRef.Substring(0, newDicRef.Length - 1);
            newDoor = newDicRef.Remove(newDicRef.Length - 2, newDicRef.Length - 1);

            //Debug.Log("newSeed = " + newSeed + "newDoor = " + newDoor);

            numVal = Int32.Parse(newDoor);

            //pass seed and door info to gameController.
            gameCon.mapSeed = newSeed;
            gameCon.doorRef = numVal;
            gameCon.touchingDoor = true; 
        }
    }

    /*
    public void OnTriggerStay2D(Collider2D node)
    {
        GameController gameCon = FindObjectOfType<GameController>();
        if (node.gameObject.tag == "Door")
        {
            gameCon.touchingDoor = true;
        }
    }
    */

    public void OnTriggerExit2D(Collider2D node)
    {
        GameController gameCon = FindObjectOfType<GameController>();
        if (node.gameObject.tag == "Door")
        {
            gameCon.touchingDoor = false;
        }
    }

}
