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
    DoorPrefabInfo info;

    void Start()
    {
        gameData = FindObjectOfType<GameData>();
    }

    public void OnTriggerEnter2D(Collider2D node)
    {
        //for door colliding in DrawPlayerMap
        info = node.GetComponent<DoorPrefabInfo>();

        if (node.gameObject.tag == "Door")
        {
            //Debug.Log("oldSeed = " + info.seedReference + " oldDoor = " + info.doorReference.ToString());
            oldSeed = info.seedReference;
            oldDoor = info.doorReference.ToString();
            dicRef = oldSeed + "," + oldDoor;

            //gameData = FindObjectOfType<GameData>();
            newDicRef = gameData.GetDoorInfo(dicRef);

            //Debug.Log("newDicRef = " + newDicRef);

            //splits the dic ref apart into the map seed and the door index value
            values = newDicRef.Split(',');

            //Debug.Log("newSeed = " + values[0] + " newDoor = " + values[1]);
            newSeed = values[0];
            newDoor = values[1];

            numVal = Int32.Parse(newDoor);
            //numVal = gameData.FindMapIndex(newDoor);

            //for drawing lines
            DrawPlayerMap.DrawPlayerMapSingle.currentDoor = Int32.Parse(oldDoor);
            DrawPlayerMap.DrawPlayerMapSingle.nextDoor = Int32.Parse(newDoor);
            DrawPlayerMap.DrawPlayerMapSingle.currentMap = oldSeed;
            DrawPlayerMap.DrawPlayerMapSingle.nextMap = newSeed;

            //pass seed and door info to gameController.
            GameController.GameControllerSingle.mapSeed = newSeed;
            //GameController.GameControllerSingle.doorRef = numVal;
            PlayerController.PlayerControllerSingle.touchingDoor = true;
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
        if (node.gameObject.tag == "Door")
        {
            PlayerController.PlayerControllerSingle.touchingDoor = false;
        }
    }

}
