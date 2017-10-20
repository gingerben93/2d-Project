using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    public static PlayerStats PlayerStatsSingle;

    //Basic stats
    public int health;
    public int defense;
    public int maxHealth;
    public int experiencePoints;
    public int level;


    //Influenced Stats
    public int armor;
    public int strength;
    public int vitality;
    public int agility;
    public int intelligence;
    public int charisma;

    //tect box for each stat
    public Text damageText;
    public Text defenseText;
    public Text strengthText;
    public Text vitalityText;
    public Text agilityText;
    public Text intelligenceText;
    public Text charismaText;


    //Equipment stats
    private int eArmor;
    private int eStrength;
    private int eVitality;
    private int eAgility;
    private int eIntelligence;
    private int eCharisma;

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

        //assign text box's to change
        damageText = GameObject.Find("Damage").GetComponent<Text>();
        defenseText = GameObject.Find("Defense").GetComponent<Text>();
        strengthText = GameObject.Find("Strength").GetComponent<Text>();
        vitalityText = GameObject.Find("Vitality").GetComponent<Text>();
        agilityText = GameObject.Find("Agility").GetComponent<Text>();
        intelligenceText = GameObject.Find("Intelligence").GetComponent<Text>();
        charismaText = GameObject.Find("Charisma").GetComponent<Text>();


        //Assigning text to each text box
        damageText.text = "<color=black>Damage:</color>";
        defenseText.text = "<color=black>Defense:</color>";
        strengthText.text = "<color=black>Strength:</color>";
        vitalityText.text = "<color=black>Vitality:</color>";
        agilityText.text = "<color=black>Agility:</color>";
        intelligenceText.text = "<color=black>Intelligence:</color>";
        charismaText.text = "<color=black>Charisma:</color>";

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
            PlayerController.PlayerControllerSingle.weaponDamage += 1;
        }
    }

    public void EquipmentStats()
    {

    }

    public void UpdatePlayerStats()
    {
        
    } 
}
