using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        }

        return nextSeed;
    }

    public void AddSeed(float seed)
    {
        mapSeed.Add(seed);
    }

}