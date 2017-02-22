using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public static PlayerStats playerStatistics;

    public int health;
    public int experiencePoints;

    public Dictionary<string, MapInformation> MapInfo = new Dictionary<string, MapInformation>();

    // Use this for initialization

    void Awake()
    {
        if (playerStatistics == null)
        {
            playerStatistics = this;
        }
        else if (playerStatistics != this)
        {
            Destroy(gameObject);
        }
        //GameObject temp = (GameObject)Resources.Load("player/GameData1", typeof(GameObject));
        //MapInfo = temp.GetComponent<PlayerStats>().MapInfo;
    }
}
