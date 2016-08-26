using UnityEngine;
using System.Collections;
using System;

public class DoorCollider : MonoBehaviour {

    

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

            Debug.Log("oldSeed = " + oldSeed + "oldDoor = " + oldDoor);

            GameData data = FindObjectOfType<GameData>();
            newDicRef = data.GetDoorInfo(dicRef);

            Debug.Log("newDicRef = " + newDicRef);

            newSeed = newDicRef.Substring(0, newDicRef.Length - 1);
            //MyString.Remove(5, 10))
            newDoor = newDicRef.Remove(newDicRef.Length - 2, newDicRef.Length - 1);

            Debug.Log("newSeed = " + newSeed + "newDoor = " + newDoor);

            int numVal = Int32.Parse(newDoor);

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
