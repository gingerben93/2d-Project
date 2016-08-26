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
                doorConnectionDictionary.Add(mapSeed[listDoor] + doorNum.ToString(), "test");
                doorDicRefs.Add(mapSeed[listDoor] + doorNum.ToString());


            }
        }

        /*
        foreach (KeyValuePair<string, string> kvp in dictionary)
        {
            //Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            Debug.Log("Key =" + kvp.Key + "Value =" + kvp.Value);
        }
        */
    }

    public void CreateDoorReferences()
    {
        //foreach(string doorRef1 in doorDicRefs)
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
        
        foreach (KeyValuePair<string, string> kvp in doorConnectionDictionary)
        {
            //Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            Debug.Log("Key =" + kvp.Key + "Value =" + kvp.Value);
        }
        

    }

    public string GetDoorInfo(string oldDoor)
    {
        string newDoor;
        newDoor = doorConnectionDictionary[oldDoor];
        return newDoor;
    }

    public void ConnectDoors()
    {
        // create temp copies, not references to
        List<List<Vector2>> tempDoorlocations = new List<List<Vector2>>(doorlocations);
        List<string> tempMapSeed = new List<string>(mapSeed);

        while (tempDoorlocations.Count > 0)
        {
            int mapSeed1 = Random.Range(0, tempDoorlocations.Count - 1);
            //List<Vector2> doorsList1 = tempDoorlocations[mapSeed1];

            int mapSeed2 = Random.Range(0, tempDoorlocations.Count - 1);
            //List<Vector2> doorsList2 = tempDoorlocations[mapSeed2];

            int doorNum1;
            string map1;
            string door1;
            Vector2 door1Vec;

            int doorNum2;
            string map2;
            string door2;
            Vector2 door2Vec;

            //get one random door info
            /*
            foreach (KeyValuePair<string, string> kvp in doorConnectionDictionary)
            {
                //Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                Debug.Log("Key =" + kvp.Key + "Value =" + kvp.Value);
            }
            */
            if (tempDoorlocations[mapSeed1].Count > 0)
            {
                doorNum1 = Random.Range(0, tempDoorlocations[mapSeed1].Count - 1);

                Debug.Log("mapSeed1 =" + mapSeed1.ToString());
                map1 = tempMapSeed[mapSeed1];
                door1 = doorNum1.ToString();
                door1Vec = tempDoorlocations[mapSeed1][doorNum1];
                tempDoorlocations[mapSeed1].Remove(door1Vec);
            }
            else
            {
                map1 = null;
                door1 = null;
                Debug.Log("doorsList1 Count = " + tempDoorlocations[mapSeed1].Count.ToString());
                tempDoorlocations.Remove(tempDoorlocations[mapSeed1]);
                continue;
            }



            //get second door info
            if (tempDoorlocations[mapSeed2].Count > 0)
            {
                doorNum2 = Random.Range(0, tempDoorlocations[mapSeed2].Count - 1);

                Debug.Log("mapSeed2 =" + mapSeed2.ToString());
                map2 = tempMapSeed[mapSeed2];
                door2 = doorNum2.ToString();
                door2Vec = tempDoorlocations[mapSeed2][doorNum2];
                tempDoorlocations[mapSeed2].Remove(door2Vec);
            }
            else
            {
                map2 = null;
                door2 = null;
                Debug.Log("doorsList2 Count = " + tempDoorlocations[mapSeed2].Count.ToString());
                tempDoorlocations.Remove(tempDoorlocations[mapSeed2]);
                continue;
            }

            Debug.Log("map1 = " + map1 + "door1 = " + door1 + "\nmap2 = " + map2 + "door2 = " + door2);
            if (map1 != null && door1 != null && map2 != null && door2 != null)
            {
                //doorsList1.RemoveAt(doorNum1);

                if (tempDoorlocations[mapSeed1].Count == 0)
                {
                    tempMapSeed.Remove(map1);
                }

                //doorsList2.RemoveAt(doorNum2);

                if (tempDoorlocations[mapSeed2].Count == 0)
                {
                    tempMapSeed.Remove(map2);
                }

                doorConnectionDictionary[map1 + door1] = map2 + door2;

            }
            else
            {
                Debug.Log("add doors back");
                tempDoorlocations[mapSeed1].Insert(doorNum1, door1Vec);
                tempDoorlocations[mapSeed2].Insert(doorNum2, door2Vec);
            }
            
        }
    }

    

    public void GetDoorInformation(string currentDoor)
    {

    }

    //make a dictionry with this info, use map + door element as unique key, make the  value all the important information
    /*
    public void ConnectDoors(string currentMap, int currentDoor)
    {
        List<DoorConnections> allDoorInfo = new List<DoorConnections>();
        List<List<Vector2>> tempDoorlocations = doorlocations;
        List<string> tempMapSeed = mapSeed;
        while (tempDoorlocations.Count > 0)
        {
            DoorConnections doorInfo = new DoorConnections();
            int mapRange = Random.Range(0, tempDoorlocations.Count);
            List<Vector2> doors = tempDoorlocations[mapRange];
            if (doors.Count > 0)
            {
                int door = Random.Range(0, doors.Count);
                doors.RemoveAt(door);
                string map = tempMapSeed[mapRange];
                tempMapSeed.Remove(map);

                doorInfo.startMap = currentMap;
                doorInfo.goalMap = map;
                doorInfo.startDoor = currentDoor;
                doorInfo.goalDoor = door;
            }
            else
            {
                tempDoorlocations.Remove(doors);
                continue;
            }
            allDoorInfo.Add(doorInfo);
        }
    }

    public class DoorConnections
    {
        public string startMap;
        public string goalMap;
        public int startDoor;
        public int goalDoor;

        void checkConnected()
        {

        }
    }
    */
}