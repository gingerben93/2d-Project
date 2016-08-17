using UnityEngine;
using System.Collections;
using System.Collections.Generic;

<<<<<<< HEAD
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
=======
public class GameData : MonoBehaviour
{

    List<float> mapSeed = new List<float>();

    public float getSeed(float seed, int doorNum)
    {
        float nextSeed;
        if (doorNum == 1)
        {
            nextSeed = mapSeed.IndexOf(seed) + 1;
        }
        else
        {
            nextSeed = mapSeed.IndexOf(seed) - 1;
>>>>>>> origin/master
        }

        return nextSeed;
    }

<<<<<<< HEAD
    public void AddSeed(string seed)
=======
    public void AddSeed(float seed)
>>>>>>> origin/master
    {
        mapSeed.Add(seed);
    }

<<<<<<< HEAD
}
=======
}
>>>>>>> origin/master
