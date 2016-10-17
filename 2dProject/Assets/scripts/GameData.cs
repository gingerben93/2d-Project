using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData : MonoBehaviour
{
    public List<List<Vector2>> doorlocations = new List<List<Vector2>>();
    public List<Vector2> doorlocations2 = new List<Vector2>();
    public Dictionary<string, string> doorConnectionDictionary = new Dictionary<string, string>();
    public List<List<string>> doorDicRefs = new List<List<string>>();
    int[] numDoorCountPerMap = new int[6];

    public List<string> mapSeed = new List<string>();

    /*
    int NumMaps = GameObject.Find("MapMarker").GetComponent<MapGenerator>().numMaps;
    int[] numDoorCountPerMap;

    void Start()
    {
        numDoorCountPerMap = new int[NumMaps];
    }
    */

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
        // fills a dictionary with all with map seeds concatenated to door index;
        for (int listDoor = 0; listDoor < doorlocations.Count; listDoor++)
        {
            List<string> tempList = new List<string>();
            int doorNum;
            for (doorNum = 0; doorNum < doorlocations[listDoor].Count; doorNum++)
            {
                doorConnectionDictionary.Add(mapSeed[listDoor] + doorNum.ToString(), "");
                tempList.Add(mapSeed[listDoor] + doorNum.ToString());
            }

            numDoorCountPerMap[listDoor] = doorNum;
            doorDicRefs.Add(tempList);
        }
    }

    public void EnsureConnectivityOfMaps()
    {
        // connects a door in map 1 to map 2, map 2 to map 3, etc
        for (int listDoor = 0; listDoor < doorlocations.Count - 1; listDoor++)
        {
            int doorIndex1 = Random.Range(0, doorDicRefs[listDoor].Count - 1);
            int doorIndex2 = Random.Range(0, doorDicRefs[listDoor + 1].Count - 1);

            numDoorCountPerMap[listDoor] -= 1;
            numDoorCountPerMap[listDoor + 1] -= 1;

            string doorRef1 = doorDicRefs[listDoor][doorIndex1];
            string doorRef2 = doorDicRefs[listDoor + 1][doorIndex2];

            //set dictionary connections and remove those doors
            doorConnectionDictionary[doorRef2] = doorRef1;
            doorConnectionDictionary[doorRef1] = doorRef2;

            doorDicRefs[listDoor].Remove(doorRef1);
            doorDicRefs[listDoor + 1].Remove(doorRef2);

            doorDicRefs[listDoor].TrimExcess();
            doorDicRefs[listDoor + 1].TrimExcess();

        }
        /*
        foreach (KeyValuePair<string, string> kvp in doorConnectionDictionary)
        {
            Debug.Log("Key =" + kvp.Key + "Value =" + kvp.Value);
        }
        */
    }

    public void ConnectDoors()
    {
        int maxDoorCountAllRooms = 0;
        List<int> possibleDoorChoices = new List<int>();

        //connects all doors, looks for highest door count room and connects to next highest door count room
        while (doorDicRefs.Count > 0)
        {
            possibleDoorChoices.Clear();
            possibleDoorChoices.TrimExcess();
            maxDoorCountAllRooms = 0;

            //first door info get and set
            for (int doorNumIndex = 0; doorNumIndex < numDoorCountPerMap.Length; doorNumIndex++)
            {
                if (maxDoorCountAllRooms < numDoorCountPerMap[doorNumIndex])
                {
                    maxDoorCountAllRooms = numDoorCountPerMap[doorNumIndex];
                    possibleDoorChoices.Clear();
                    possibleDoorChoices.TrimExcess();
                    possibleDoorChoices.Add(doorNumIndex);
                }
                else if(maxDoorCountAllRooms == numDoorCountPerMap[doorNumIndex])
                {
                    possibleDoorChoices.Add(doorNumIndex);
                }
            }

            //get right map; decrease door count; get random door; get dictionary key;
            int mapIndex1 = possibleDoorChoices[Random.Range(0, possibleDoorChoices.Count - 1)];
            numDoorCountPerMap[mapIndex1] -= 1;
            int doorIndex1 = Random.Range(0, doorDicRefs[mapIndex1].Count - 1);
            string doorRef1 = doorDicRefs[mapIndex1][doorIndex1];

            //begin second door info get and set
            int secondMaxDoorCountAllRooms = maxDoorCountAllRooms;

            possibleDoorChoices.Clear();
            possibleDoorChoices.TrimExcess();

            while(possibleDoorChoices.Count == 0)
            {
                for (int doorNumIndex = 0; doorNumIndex < numDoorCountPerMap.Length; doorNumIndex++)
                {
                    if (mapIndex1 == doorNumIndex)
                    {
                        //do nothing, this is room of first door info
                    }
                    else if (secondMaxDoorCountAllRooms == numDoorCountPerMap[doorNumIndex])
                    {
                        possibleDoorChoices.Add(doorNumIndex);
                    }
                }
                secondMaxDoorCountAllRooms -= 1;
            }

            //get right map; if map is sam as first get another; decrease door count; get random door; get dictionary key;
            int mapIndex2 = possibleDoorChoices[Random.Range(0, possibleDoorChoices.Count - 1)];
            while (mapIndex2 == mapIndex1)
            {
                mapIndex2 = possibleDoorChoices[Random.Range(0, possibleDoorChoices.Count - 1)];
            }
            numDoorCountPerMap[mapIndex2] -= 1;
            int doorIndex2 = Random.Range(0, doorDicRefs[mapIndex2].Count - 1);
            string doorRef2 = doorDicRefs[mapIndex2][doorIndex2];


            //set dictionary connections and remove those doors
            doorConnectionDictionary[doorRef2] = doorRef1;
            doorConnectionDictionary[doorRef1] = doorRef2;

            doorDicRefs[mapIndex1].Remove(doorRef1);
            doorDicRefs[mapIndex2].Remove(doorRef2);

            doorDicRefs[mapIndex1].TrimExcess();
            doorDicRefs[mapIndex2].TrimExcess();

            //check if any doors are left to connect
            int allMapsHaveNoDoorsLeft = 0;
            for(int x = 0; x < doorDicRefs.Count; x++)
            {
                if (doorDicRefs[x].Count == 0)
                {
                    allMapsHaveNoDoorsLeft += 1;
                }
            }
            if(allMapsHaveNoDoorsLeft == doorDicRefs.Count)
            {
                break;
            }
        }

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