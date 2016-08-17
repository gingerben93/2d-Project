using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData : MonoBehaviour {

    List<string> mapSeed = new List<string>();

    public bool isSeed(string seed)
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

    public string getSeed(string seed)
    {
        string nextSeed;
        if(mapSeed.Count > 0) {
            nextSeed = mapSeed[(mapSeed.IndexOf(seed) + 1) % mapSeed.Count];
        }
        else
        {
            return "0";
        }

        return nextSeed;
    }

    public void AddSeed(string seed)
    {
        mapSeed.Add(seed);
    }

}
