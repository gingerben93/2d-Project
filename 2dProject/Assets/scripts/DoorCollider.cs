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
    private string[] values;

    GameData gameData;
    DrawPlayerMap changeLocalMap;
    GameController gameCon;
    DoorPrefabInfo info;

    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        changeLocalMap = FindObjectOfType<DrawPlayerMap>();
        gameCon = FindObjectOfType<GameController>();
        
    }

    public void OnTriggerEnter2D(Collider2D node)
    {
        //for door colliding in DrawPlayerMap
        info = node.GetComponent<DoorPrefabInfo>();

        if (node.gameObject.tag == "Door")
        {
            oldSeed = info.seedReference;
            oldDoor = info.doorReference.ToString();
            dicRef = oldSeed + "," + oldDoor;

            //gameData = FindObjectOfType<GameData>();
            newDicRef = gameData.GetDoorInfo(dicRef);

            //Debug.Log("newDicRef = " + newDicRef);

            //splits the dic ref apart into the map seed and the door index value
            values = newDicRef.Split(',');

            //Debug.Log("newSeed = " + values[0] + "newDoor = " + values[1]);
            newSeed = values[0];
            newDoor = values[1];

            numVal = Int32.Parse(newDoor);
            //numVal = gameData.FindMapIndex(newDoor);

            //for drawing lines
            changeLocalMap.currentDoor = Int32.Parse(oldDoor);
            changeLocalMap.nextDoor = Int32.Parse(newDoor);
            changeLocalMap.currentMap = oldSeed;
            changeLocalMap.nextMap = newSeed;

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
        changeLocalMap = FindObjectOfType<DrawPlayerMap>();
        gameCon = FindObjectOfType<GameController>();

        if (node.gameObject.tag == "Door")
        {
            changeLocalMap.touchingDoor = false;
            gameCon.touchingDoor = false;
        }
    }

}
