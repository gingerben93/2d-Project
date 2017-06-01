using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public static PlayerStats PlayerStatsSingle;

    public int health;
    public int maxHealth;
    public int experiencePoints;
    public int level;

    public Dictionary<string, MapInformation> MapInfo = new Dictionary<string, MapInformation>();

    // Use this for initialization

    void Awake()
    {
        if (PlayerStatsSingle == null)
        {
            PlayerStatsSingle = this;
        }
        else if (PlayerStatsSingle != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (GameController.GameControllerSingle.dontLoadTheGame)
        {
            //Don't do anything
        }
        else
        {
            GameController.GameControllerSingle.LoadPlayerData();
        }
    }
}
