using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData : MonoBehaviour
{
    //saves door locations
    public List<List<Vector2>> possibleDoorLocations = new List<List<Vector2>>();
    public List<List<Vector2>> doorlocations = new List<List<Vector2>>();

    //useless; for vizualzation
    public List<Vector2> doorlocations2 = new List<Vector2>();

    //saves the width and height of each map
    public List<int[,]> MapWidthHeight = new List<int[,]>();

    public int squareSize { get; set; }


    public Dictionary<string, string> doorConnectionDictionary = new Dictionary<string, string>();
    public List<List<string>> doorDicRefs = new List<List<string>>();
    //int[] numDoorCountPerMap = new int[6];
    List<int> numDoorCountPerMap = new List<int>();

    public List<string> mapSeed = new List<string>();

    //for enemy locations
    public List<List<Vector2>> enemylocations = new List<List<Vector2>>();

    //for the size of the map sets for making the world map
    public List<Vector2> mapSets = new List<Vector2>();


    // adds door to list
    public void AddDoorLocations(List<Vector2> newDoorLocations)
    {
        foreach(Vector2 door in newDoorLocations){
            doorlocations2.Add(new Vector2(door.x, door.y));
        }
        possibleDoorLocations.Add(newDoorLocations);
    }


    // add enemy location
    public void AddEnemyLocations(List<Vector2> newEnemyLocations)
    {
        enemylocations.Add(newEnemyLocations);
    }

    // gets doors from list
    public List<Vector2> GetDoorForMap(int mapIndex)
    {
        return doorlocations[mapIndex];
    }

    //for enemy list
    public List<Vector2> GetEnemyForMap(int mapIndex)
    {
        return enemylocations[mapIndex];
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

    public List<Vector2> createNumberOfDoors(int currentMap, int numDoors)
    {
        List<Vector2> drawDoors = new List<Vector2>();

        //Debug.Log("create doors for map = " + currentMap);

        for (int x = 0; x < numDoors; x++)
        {
            squareSize = 1;
            //for size of map
            int nodeCountX = MapWidthHeight[currentMap].GetLength(0);
            int nodeCountY = MapWidthHeight[currentMap].GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            Vector2 doorXY;

            doorXY = possibleDoorLocations[currentMap][Random.Range(0, possibleDoorLocations.Count)];
            possibleDoorLocations[currentMap].Remove(doorXY);

            float xPos = -mapWidth / 2 + doorXY.x * squareSize + squareSize / 2;
            float yPos = -mapHeight / 2 + doorXY.y * squareSize + squareSize;

            doorXY = new Vector2(xPos, yPos);
            //Debug.Log("create door doorXY = " + doorXY + " on map" + currentMap);
            drawDoors.Add(doorXY);

        }

        return drawDoors;
    }

    public void CreatDoorConnections(int startMap, int endMap, int numDoors)
    {
        //this is the list with the number of doors per map in the current set of maps. is a temp variable
        numDoorCountPerMap = new List<int>();
        numDoorCountPerMap.Clear();
        numDoorCountPerMap.TrimExcess();

        //create the number of doors
        doorDicRefs = new List<List<string>>();


        for (int listDoor = startMap; listDoor < endMap + 1; listDoor++)
        {
            List<Vector2> ListOfDoors = new List<Vector2>();
            ListOfDoors = createNumberOfDoors(listDoor, numDoors);
            //all door locations
            doorlocations.Add(ListOfDoors);
        }

        //int test = 0;
        //foreach (List<Vector2> doors in doorlocations)
        //{
        //    Debug.Log("Times Run  test = " + test);
        //    foreach (Vector2 door in doors)
        //    {
        //        Debug.Log("List<Vector2> door = " + door);
        //    }
        //    test += 1;
        //}
    }

    public void EnsureConnectivityOfMaps(int startMap, int endMap)
    {
        // fills a dictionary with all with map seeds concatenated to door index;
        for (int listDoor = startMap; listDoor < endMap + 1; listDoor++)
        {
            //Debug.Log("Times Run 2 = " + listDoor);
            List<string> tempList = new List<string>();
            int doorNum;
            for (doorNum = 0; doorNum < doorlocations[listDoor - startMap].Count; doorNum++)
            {
                doorConnectionDictionary.Add(mapSeed[listDoor] + "," + doorNum.ToString(), "");
                tempList.Add(mapSeed[listDoor] + "," + doorNum.ToString());
            }

            numDoorCountPerMap.Add(doorNum);
            //numDoorCountPerMap[listDoor] = doorNum;
            doorDicRefs.Add(tempList);
        }

        // connects a door in map 1 to map 2, map 2 to map 3, etc
        for (int listDoor = startMap - startMap; listDoor < endMap - startMap; listDoor++)
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

        //foreach (KeyValuePair<string, string> kvp in doorConnectionDictionary)
        //{
        //    Debug.Log("Key =" + kvp.Key + "Value =" + kvp.Value);
        //}

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
            //Debug.Log("numDoorCountPerMap.Length = " + numDoorCountPerMap.Count);
            for (int doorNumIndex = 0; doorNumIndex < numDoorCountPerMap.Count; doorNumIndex++)
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
                for (int doorNumIndex = 0; doorNumIndex < numDoorCountPerMap.Count; doorNumIndex++)
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

        
        //foreach (KeyValuePair<string, string> kvp in doorConnectionDictionary)
        //{
        //    Debug.Log("Key =" + kvp.Key + "Value =" + kvp.Value);
        //}
        

    }

    //connects two maps with one door
    public void ConnectSetOfRooms(int mapOne, int mapTwo)
    {
        Vector2 door1 = possibleDoorLocations[mapOne][Random.Range(0, possibleDoorLocations[mapOne].Count)];
        Vector2 door2 = possibleDoorLocations[mapTwo][Random.Range(0, possibleDoorLocations[mapTwo].Count)];
        possibleDoorLocations[mapOne].Remove(door1);
        possibleDoorLocations[mapTwo].Remove(door2);

        squareSize = 1;
        //for size of map
        int nodeCountXMapOne = MapWidthHeight[mapOne].GetLength(0);
        int nodeCountYMapOne = MapWidthHeight[mapOne].GetLength(1);
        float mapWidthMapOne = nodeCountXMapOne * squareSize;
        float mapHeightMapOne = nodeCountYMapOne * squareSize;

        int nodeCountXMapTwo = MapWidthHeight[mapTwo].GetLength(0);
        int nodeCountYMapTwo = MapWidthHeight[mapTwo].GetLength(1);
        float mapWidthMapTwo = nodeCountXMapTwo * squareSize;
        float mapHeightMapTwo = nodeCountYMapTwo * squareSize;

        float xPosMapOne = -mapWidthMapOne / 2 + door1.x * squareSize + squareSize / 2;
        float yPosMapOne = -mapHeightMapOne / 2 + door1.y * squareSize + squareSize;

        float xPosMapTwo = -mapWidthMapTwo / 2 + door2.x * squareSize + squareSize / 2;
        float yPosMapTwo = -mapHeightMapTwo / 2 + door2.y * squareSize + squareSize;

        door1 = new Vector2(xPosMapOne, yPosMapOne);
        door2 = new Vector2(xPosMapTwo, yPosMapTwo);

        doorlocations[mapOne].Add(door1);
        doorlocations[mapTwo].Add(door2);
        

        doorConnectionDictionary.Add(mapSeed[mapOne] + "," + (doorlocations[mapOne].Count - 1).ToString(), mapSeed[mapTwo] + "," + (doorlocations[mapTwo].Count - 1).ToString());
        doorConnectionDictionary.Add(mapSeed[mapTwo] + "," + (doorlocations[mapTwo].Count - 1).ToString(), mapSeed[mapOne] + "," + (doorlocations[mapOne].Count - 1).ToString());

        //foreach (KeyValuePair<string, string> kvp in doorConnectionDictionary)
        //{
        //    Debug.Log("Key =" + kvp.Key + "Value =" + kvp.Value);
        //}
    }


    public string GetDoorInfo(string DicRef)
    {
        string newDoor;
        newDoor = doorConnectionDictionary[DicRef];
        return newDoor;
    }
}