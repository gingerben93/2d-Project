using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData : MonoBehaviour
{
    public List<List<Vector2>> doorlocations = new List<List<Vector2>>();
    public List<Vector2> doorlocations2 = new List<Vector2>();
    public Dictionary<string, string> doorConnectionDictionary = new Dictionary<string, string>();
    public List<string> doorDicRefs = new List<string>();

    // adds door to list
    public void AddDoorLocations(List<Vector2> newDoorLocations)
    {
        foreach(Vector2 door in newDoorLocations){
            doorlocations2.Add(new Vector2(door.x, door.y));
        }
        doorlocations.Add(newDoorLocations);
    }

    // gets doors from list
    public List<Vector2> GetDoorForMap(int mapIndex)
    {
        return doorlocations[mapIndex];
    }

    public List<string> mapSeed = new List<string>();

    //gets index of map
    public int FindMapIndex(string map)
    {
        return mapSeed.IndexOf(map);
    }

    //checks if is seed
    public bool IsSeed(string seed)
    {
        if (mapSeed.Contains(seed))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // returns the next seeds in list
    public string GetSeed(string seed)
    {
        string nextSeed;
        if (mapSeed.Count > 0)
        {
            int index = mapSeed.IndexOf(seed);

            if (mapSeed.Count > index + 1)
            {
                nextSeed = mapSeed[index + 1];
            }
            else
            {
                nextSeed = mapSeed[0];
            }
        }
        else
        {
            return "0";
        }

        return nextSeed;
    }

    //adds seed to map
    public void AddSeed(string seed)
    {
        mapSeed.Add(seed);
    }

    public void CreatDoorConnections()
    {
        for (int listDoor = 0; listDoor < doorlocations.Count; listDoor++)
        {
            for(int doorNum = 0; doorNum < doorlocations[listDoor].Count; doorNum++)
            {
                doorConnectionDictionary.Add(mapSeed[listDoor] + doorNum.ToString(), "");
                doorDicRefs.Add(mapSeed[listDoor] + doorNum.ToString());
            }
        }
    }

    public void CreateDoorReferences()
    {
        //first pass to ensure connectivity between all rooms

        //connects all doors randomly
        while(doorDicRefs.Count > 0)
        {
            
            int doorIndex1 = Random.Range(0, doorDicRefs.Count - 1);
            string doorRef1 = doorDicRefs[doorIndex1];
            doorDicRefs.Remove(doorRef1);
            

            int doorIndex2 = Random.Range(0, doorDicRefs.Count - 1);
            string doorRef2 = doorDicRefs[doorIndex2];
            doorDicRefs.Remove(doorRef2);

            doorConnectionDictionary[doorRef2] = doorRef1;
            doorConnectionDictionary[doorRef1] = doorRef2;
           
        }
        
        //prints the dic of door info
        /*
        foreach (KeyValuePair<string, string> kvp in doorConnectionDictionary)
        {
            Debug.Log("Key =" + kvp.Key + "Value =" + kvp.Value);
        }
        */
        

    }

    public string GetDoorInfo(string oldDoor)
    {
        string newDoor;
        newDoor = doorConnectionDictionary[oldDoor];
        return newDoor;
    }
}