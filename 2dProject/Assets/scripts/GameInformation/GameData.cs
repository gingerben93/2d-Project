using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData : MonoBehaviour
{
    //used only in gamedata class
    private List<List<string>> doorDicRefs = new List<List<string>>();
    private List<int> numDoorCountPerMap = new List<int>();

    //used outside and inside of gamedata class
    public Dictionary<string, string> doorConnectionDictionary = new Dictionary<string, string>();
    public List<string> mapSeed = new List<string>();

    //for the size of the map sets for making the world map
    public List<Vector2> mapSets = new List<Vector2>();

    public static GameData GameDataSingle;

    void Awake()
    {
        if (GameDataSingle == null)
        {
            DontDestroyOnLoad(gameObject);
            GameDataSingle = this;
        }
        else if (GameDataSingle != this)
        {
            Destroy(gameObject);
        }
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

    public List<Vector2> createNumberOfDoors(int numDoors, MapInformation map1, int listDoor)
    {
        List<Vector2> drawDoors = new List<Vector2>();

        for (int x = 0; x < numDoors; x++)
        {
            Vector2 doorXY;
            Vector2 tempDoor;
            int tempDoorIndex = Random.Range(0, map1.possibleDoorLocationsX.Count);
            doorXY = new Vector2(map1.possibleDoorLocationsX[tempDoorIndex], map1.possibleDoorLocationsY[tempDoorIndex]);

            float xPos = -map1.width / 2 + doorXY.x * map1.squareSize + map1.squareSize / 2;
            float yPos = -map1.height / 2 + doorXY.y * map1.squareSize + map1.squareSize;

            //for removing doors left and right of current door
            if (tempDoorIndex < map1.possibleDoorLocationsX.Count - 1)
            {
                tempDoor = new Vector2(map1.possibleDoorLocationsX[tempDoorIndex + 1], map1.possibleDoorLocationsY[tempDoorIndex + 1]);
                if (Vector2.Distance(tempDoor, doorXY) <= 1)
                {
                    //Debug.Log(map1.index + "REMOVE1 Vector2.Distance(tempDoor, doorXY) = " + Vector2.Distance(tempDoor, doorXY) + "tempDoor.x = " + tempDoor.x + " tempDoor.y = " + tempDoor.y + "tempDoorIndex1 = " + tempDoorIndex + " X = " + x);
                    map1.possibleDoorLocationsX.RemoveAt(tempDoorIndex + 1);
                    map1.possibleDoorLocationsY.RemoveAt(tempDoorIndex + 1);
                }
            }
            if (tempDoorIndex > 0)
            {
                tempDoor = new Vector2(map1.possibleDoorLocationsX[tempDoorIndex - 1], map1.possibleDoorLocationsY[tempDoorIndex - 1]);
                if (Vector2.Distance(tempDoor, doorXY) <= 1)
                {
                    //Debug.Log(map1.index + "REMOVE2 Vector2.Distance(tempDoor, doorXY) = " + Vector2.Distance(tempDoor, doorXY) + "tempDoor.x = " + tempDoor.x + " tempDoor.y = " + tempDoor.y + "tempDoorIndex2 = " + tempDoorIndex + " X = " + x);
                    map1.possibleDoorLocationsX.RemoveAt(tempDoorIndex - 1);
                    map1.possibleDoorLocationsY.RemoveAt(tempDoorIndex - 1);
                    tempDoorIndex -= 1;
                }
            }

            //Debug.Log("tempDoorIndex At end = " + tempDoorIndex);
            map1.possibleDoorLocationsX.RemoveAt(tempDoorIndex);
            map1.possibleDoorLocationsY.RemoveAt(tempDoorIndex);

            doorXY = new Vector2(xPos, yPos);
            drawDoors.Add(doorXY);

            //Debug.Log(map1.index + "After MapGenerator.MapGeneratorSingle.MapInfo[mapSeed[listDoor]].Count = " + MapGenerator.MapGeneratorSingle.MapInfo[mapSeed[listDoor]].possibleDoorLocationsX.Count + " tempDoorIndex = " + tempDoorIndex + " X = " + x);
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
            MapInformation map1 = MapGenerator.MapGeneratorSingle.MapInfo[mapSeed[listDoor]];

            ListOfDoors = createNumberOfDoors(numDoors, map1 , listDoor);

            //if ((MapGenerator.MapGeneratorSingle.MapInfo[mapSeed[listDoor]]).doorLocationsX != null)
            //{
            //    Debug.Log("BEFORE MapGenerator.MapGeneratorSingle.MapInfo[mapSeed[listDoor]].Count = " + MapGenerator.MapGeneratorSingle.MapInfo[mapSeed[listDoor]].doorLocationsX.Count);
            //}

            List<float> tempHolderX = new List<float>();
            List<float> tempHolderY = new List<float>();
            foreach (Vector2 XYCoord in ListOfDoors)
            {
                tempHolderX.Add(XYCoord.x);
                tempHolderY.Add(XYCoord.y);
            }
            //all door locations
            map1.doorLocationsX = tempHolderX;
            map1.doorLocationsY = tempHolderY;

            //Debug.Log("AFTER MapGenerator.MapGeneratorSingle.MapInfo[mapSeed[listDoor]].Count = " + MapGenerator.MapGeneratorSingle.MapInfo[mapSeed[listDoor]].doorLocationsX.Count);
            //Debug.Log("map1.doorLocationsX.Count = " + map1.doorLocationsX.Count);
        }
    }

    public void EnsureConnectivityOfMaps(int startMap, int endMap)
    {
        // fills a dictionary with all with map seeds concatenated to door index;
        for (int listDoor = startMap; listDoor < endMap + 1; listDoor++)
        {
            //Debug.Log("Times Run 2 = " + listDoor);
            List<string> tempList = new List<string>();
            MapInformation map1 = MapGenerator.MapGeneratorSingle.MapInfo[mapSeed[startMap]];
            int doorNum;
            for (doorNum = 0; doorNum < map1.doorLocationsX.Count; doorNum++)
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
            int doorIndex1 = Random.Range(0, doorDicRefs[listDoor].Count);
            int doorIndex2 = Random.Range(0, doorDicRefs[listDoor + 1].Count);

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
            int mapIndex1 = possibleDoorChoices[Random.Range(0, possibleDoorChoices.Count)];

            numDoorCountPerMap[mapIndex1] -= 1;
            int doorIndex1 = Random.Range(0, doorDicRefs[mapIndex1].Count);
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
            int mapIndex2 = possibleDoorChoices[Random.Range(0, possibleDoorChoices.Count)];
            while (mapIndex2 == mapIndex1)
            {
                mapIndex2 = possibleDoorChoices[Random.Range(0, possibleDoorChoices.Count)];
            }
            numDoorCountPerMap[mapIndex2] -= 1;
            int doorIndex2 = Random.Range(0, doorDicRefs[mapIndex2].Count);
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
        //Debug.Log("map1 = " + mapOne + " Map2 = " + mapTwo);
        MapInformation map1 = MapGenerator.MapGeneratorSingle.MapInfo[mapSeed[mapOne]];
        MapInformation map2 = MapGenerator.MapGeneratorSingle.MapInfo[mapSeed[mapTwo]];

        int tempDoorIndex1 = Random.Range(0, map1.possibleDoorLocationsX.Count);
        Vector2 tempDoor;
        Vector2 door1 = new Vector2(map1.possibleDoorLocationsX[tempDoorIndex1], map1.possibleDoorLocationsY[tempDoorIndex1]);

        float xPosMapOne = -map1.width / 2 + door1.x * map1.squareSize + map1.squareSize / 2;
        float yPosMapOne = -map1.height / 2 + door1.y * map1.squareSize + map1.squareSize;



        //for removing doors left and right of current door
        if (tempDoorIndex1 < map1.possibleDoorLocationsX.Count - 1)
        {
            tempDoor = new Vector2(map1.possibleDoorLocationsX[tempDoorIndex1 + 1], map1.possibleDoorLocationsY[tempDoorIndex1 + 1]);
            if (Vector2.Distance(tempDoor, door1) <= 1)
            {
                map1.possibleDoorLocationsX.Remove(tempDoorIndex1 + 1);
                map1.possibleDoorLocationsY.Remove(tempDoorIndex1 + 1);
            }
        }
        if (tempDoorIndex1 > 0)
        {
            tempDoor = new Vector2(map1.possibleDoorLocationsX[tempDoorIndex1 - 1], map1.possibleDoorLocationsY[tempDoorIndex1 - 1]);
            if (Vector2.Distance(tempDoor, door1) <= 1)
            {
                map1.possibleDoorLocationsX.Remove(tempDoorIndex1 - 1);
                map1.possibleDoorLocationsY.Remove(tempDoorIndex1 - 1);
                //when remove left element you have to shrik the index location beacuse list resizes.
                tempDoorIndex1--;
            }
        }

        int tempDoorIndex2 = Random.Range(0, map2.possibleDoorLocationsX.Count);
        Vector2 door2 = new Vector2(map2.possibleDoorLocationsX[tempDoorIndex2], map2.possibleDoorLocationsY[tempDoorIndex2]);

        float xPosMapTwo = -map2.width / 2 + door2.x * map2.squareSize + map2.squareSize / 2;
        float yPosMapTwo = -map2.height / 2 + door2.y * map2.squareSize + map2.squareSize;

        //for removing doors left and right of current door
        if (tempDoorIndex2 < map2.possibleDoorLocationsX.Count - 1)
        {
            tempDoor = new Vector2(map2.possibleDoorLocationsX[tempDoorIndex2 + 1], map2.possibleDoorLocationsY[tempDoorIndex2 + 1]);
            if (Vector2.Distance(tempDoor, door2) <= 1)
            {
                map2.possibleDoorLocationsX.Remove(tempDoorIndex2 + 1);
                map2.possibleDoorLocationsY.Remove(tempDoorIndex2 + 1);
            }
        }
        if (tempDoorIndex2 > 0)
        {
            tempDoor = new Vector2(map2.possibleDoorLocationsX[tempDoorIndex2 - 1], map2.possibleDoorLocationsY[tempDoorIndex2 - 1]);
            if (Vector2.Distance(tempDoor, door2) <= 1)
            {
                map2.possibleDoorLocationsX.Remove(tempDoorIndex2 - 1);
                map2.possibleDoorLocationsY.Remove(tempDoorIndex2 - 1);
                //when remove left element you have to shrik the index location beacuse list resizes.
                tempDoorIndex2 -= 1;
            }
        }

        map1.possibleDoorLocationsX.RemoveAt(tempDoorIndex1);
        map1.possibleDoorLocationsY.RemoveAt(tempDoorIndex1);
        map2.possibleDoorLocationsX.RemoveAt(tempDoorIndex2);
        map2.possibleDoorLocationsY.RemoveAt(tempDoorIndex2);

        door1 = new Vector2(xPosMapOne, yPosMapOne);
        door2 = new Vector2(xPosMapTwo, yPosMapTwo);

        map1.doorLocationsX.Add(xPosMapOne);
        map1.doorLocationsY.Add(yPosMapOne);
        map2.doorLocationsX.Add(xPosMapTwo);
        map2.doorLocationsY.Add(yPosMapTwo);

        doorConnectionDictionary.Add(mapSeed[mapOne] + "," + (map1.doorLocationsX.Count - 1).ToString(), mapSeed[mapTwo] + "," + (map2.doorLocationsX.Count - 1).ToString());
        doorConnectionDictionary.Add(mapSeed[mapTwo] + "," + (map2.doorLocationsX.Count - 1).ToString(), mapSeed[mapOne] + "," + (map1.doorLocationsX.Count - 1).ToString());

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