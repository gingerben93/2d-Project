using UnityEngine;
using System.Collections;
using System;

public class DoorCollider : MonoBehaviour {

    public int numVal { get; set; }
    private string oldSeed;
    private string oldDoor;
    private string dicRef;

    private string newSeed;
    private string newDoor;
    private string newDicRef;

    public void OnTriggerEnter2D(Collider2D node)
    {
        //for door colliding in DrawPlayerMap
        DrawPlayerMap changeLocalMap = FindObjectOfType<DrawPlayerMap>();
        GameController gameCon = FindObjectOfType<GameController>();
        DoorPrefabInfo info = node.GetComponent<DoorPrefabInfo>();
        if (node.gameObject.tag == "Door")
        {
            oldSeed = info.seedReference;
            oldDoor = info.doorReference.ToString();
            dicRef = oldSeed + oldDoor;

            GameData data = FindObjectOfType<GameData>();
            newDicRef = data.GetDoorInfo(dicRef);

            //set new door and map seed info
            newSeed = newDicRef.Substring(0, newDicRef.Length - 1);
            newDoor = newDicRef.Remove(newDicRef.Length - 2, newDicRef.Length - 1);

            //Debug.Log("newSeed = " + newSeed + "newDoor = " + newDoor);

            numVal = Int32.Parse(newDoor);

            //for drawing lines
            GameObject.FindObjectOfType<DrawPlayerMap>().currentDoor = Int32.Parse(oldDoor);
            GameObject.FindObjectOfType<DrawPlayerMap>().nextDoor = Int32.Parse(newDoor);
            GameObject.FindObjectOfType<DrawPlayerMap>().currentMap = Int32.Parse(oldSeed);
            GameObject.FindObjectOfType<DrawPlayerMap>().nextMap = Int32.Parse(newSeed);

            //pass seed and door info to gameController.
            gameCon.mapSeed = newSeed;
            gameCon.doorRef = numVal;
            gameCon.touchingDoor = true;
            changeLocalMap.touchingDoor = true;
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
        //for door colliding in DrawPlayerMap
        DrawPlayerMap changeLocalMap = FindObjectOfType<DrawPlayerMap>();
        GameController gameCon = FindObjectOfType<GameController>();
        if (node.gameObject.tag == "Door")
        {
            changeLocalMap.touchingDoor = false;
            gameCon.touchingDoor = false;
        }
    }

}
