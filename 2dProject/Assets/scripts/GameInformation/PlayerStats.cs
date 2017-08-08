using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public static PlayerStats PlayerStatsSingle;

    public int health;
    public int maxHealth;
    public int experiencePoints;
    public int level;

    public int baseWeaponDamage;
    public int baseSpellDamage;

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

    //for gain exp
    public void GainExperiencePoints(int exp)
    {
        experiencePoints += exp;
        if (experiencePoints >= level * 15)
        {
            // text container
            StartCoroutine(GameController.GameControllerSingle.ShowMessage("LEVEL UP NERD!", 2));

            //level character up
            level += 1;
            maxHealth += 2;
            health = maxHealth;
            baseWeaponDamage += 1;
            baseSpellDamage += 1;

            // change damage for currently equipt weapons
            GameController.GameControllerSingle.weaponDamage += 1;
        }
    }
}
