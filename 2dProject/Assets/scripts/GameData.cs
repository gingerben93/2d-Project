using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData : MonoBehaviour
{
    List<List<Vector2>> doorlocations = new List<List<Vector2>>();

    /*
    public void addMapSeed(string newMapSeed)
    {
        mapSeeds.Add(newMapSeed);
    }
    */

    // adds door to list
    public void AddDoorLocations(List<Vector2> newDoorLocations)
    {
        doorlocations.Add(newDoorLocations);
    }

    // gets doors from list
    public List<Vector2> GetDoorForMap(int mapIndex)
    {
        return doorlocations[mapIndex];
    }

    List<string> mapSeed = new List<string>();

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
            nextSeed = mapSeed[(mapSeed.IndexOf(seed) + 1) % mapSeed.Count];
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

}