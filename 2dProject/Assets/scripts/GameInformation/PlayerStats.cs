using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    public static PlayerStats PlayerStatsSingle;

    public int health;
    public int maxHealth;
    public int experiencePoints;
    public int level;

    public int baseWeaponDamage;
    public int baseSpellDamage;

    // for diaplaying exp and level
    private Text experiencePointsTxt;
    private Text levelTxt;

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

        experiencePointsTxt = GameObject.Find("Experience").GetComponent<Text>();
        levelTxt = GameObject.Find("Level").GetComponent<Text>();
        experiencePointsTxt.text = "EXP: " + experiencePoints;
        levelTxt.text = "Level: " + level;
    }

    //for gain exp
    public void GainExperiencePoints(int exp)
    {
        experiencePoints += exp;
        //change exp text
        experiencePointsTxt.text = "EXP: " + experiencePoints;

        //player level up
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

            //change text that needs to change
            levelTxt.text = "Level: " + level;

            // change damage for currently equipt weapons
            PlayerController.PlayerControllerSingle.weaponDamage += 1;
        }
    }
}
