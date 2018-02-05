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

    public int skillPoints = 3;
    public int statPoints = 20;
    public int maxPoints = 20;

    public int armor = 0;
    public int dexterity = 0;
    public int intelligence = 0;
    public int strength = 0;
    public int vitality = 0;

    public int armorMin = 0;
    public int dexterityMin = 0;
    public int intelligenceMin = 0;
    public int strengthMin = 0;
    public int vitalityMin = 0;

    // for diaplaying exp and level
    private Text experiencePointsTxt;
    private Text levelTxt;

    private Text statPoinsTxt;
    private Text skillPoinsTxt;

    private Text armorTxt;
    private Text dexterityTxt;
    private Text intelligenceTxt;
    private Text strengthTxt;
    private Text vitalityTxt;

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

        //set skill point temporarely
        skillPoints = 3;

        //find button and set method
        GameObject.Find("ArmorUp").GetComponent<Button>().onClick.AddListener(delegate { IncArmor(); });
        GameObject.Find("ArmorDown").GetComponent<Button>().onClick.AddListener(delegate { DecArmor(); });
        GameObject.Find("DexterityUp").GetComponent<Button>().onClick.AddListener(delegate { IncDexterity(); });
        GameObject.Find("DexterityDown").GetComponent<Button>().onClick.AddListener(delegate { DecDexterity(); });
        GameObject.Find("IntelligenceUp").GetComponent<Button>().onClick.AddListener(delegate { IncIntelligence(); });
        GameObject.Find("IntelligenceDown").GetComponent<Button>().onClick.AddListener(delegate { DecIntelligence(); });
        GameObject.Find("StrengthUp").GetComponent<Button>().onClick.AddListener(delegate { IncStrength(); });
        GameObject.Find("StrengthDown").GetComponent<Button>().onClick.AddListener(delegate { DecStrength(); });
        GameObject.Find("VitalityUp").GetComponent<Button>().onClick.AddListener(delegate { IncVitality(); });
        GameObject.Find("VitalityDown").GetComponent<Button>().onClick.AddListener(delegate { DecVitality(); });

        //save skill points button
        GameObject.Find("SaveStatPoints").GetComponent<Button>().onClick.AddListener(delegate { SaveStatPoints(); });

        //find text box
        experiencePointsTxt = GameObject.Find("Experience").GetComponent<Text>();
        levelTxt = GameObject.Find("Level").GetComponent<Text>();

        statPoinsTxt = GameObject.Find("StatPoints").GetComponent<Text>();
        skillPoinsTxt = GameObject.Find("SkillPoints").GetComponent<Text>();

        armorTxt = GameObject.Find("ArmorText").GetComponent<Text>();
        dexterityTxt = GameObject.Find("DexterityText").GetComponent<Text>();
        intelligenceTxt = GameObject.Find("IntelligenceText").GetComponent<Text>();
        strengthTxt = GameObject.Find("StrengthText").GetComponent<Text>();
        vitalityTxt = GameObject.Find("VitalityText").GetComponent<Text>();
        

        //set text
        experiencePointsTxt.text = "EXP: " + experiencePoints;
        levelTxt.text = "Level: " + level;

        statPoinsTxt.text = "Unspent Stat Points: " + statPoints;
        skillPoinsTxt.text = "Unspent Skill Points: " + skillPoints;

        armorTxt.text = "Armor: " + armor;
        dexterityTxt.text = "Dexterity: " + strength;
        intelligenceTxt.text = "Intelligence: " + intelligence;
        strengthTxt.text = "Strength: " + strength;
        vitalityTxt.text = "Vitality: " + vitality;
        
        
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
            statPoints += 3;
            maxPoints = statPoints;

            //change text that needs to change
            levelTxt.text = "Level: " + level;
            statPoinsTxt.text = "Unspent Stat Points: " + statPoints;

            // change damage for currently equipt weapons
            //PlayerController.PlayerControllerSingle.weaponDamage += 1;
        }
    }

    public void IncArmor()
    {
        if (statPoints > 0)
        {
            UpdateStatText("Armor", 1);
        }
    }

    public void DecArmor()
    {
        if (armor > armorMin)
        {
            UpdateStatText("Armor", -1);
        }
    }

    public void IncDexterity()
    {
        if (statPoints > 0)
        {
            UpdateStatText("Dexterity", 1);
        }
    }

    public void DecDexterity()
    {
        if (dexterity > dexterityMin)
        {
            UpdateStatText("Dexterity", -1);
        }
    }
    public void IncIntelligence()
    {
        if (statPoints > 0)
        {
            UpdateStatText("Intelligence", 1);
        }
    }

    public void DecIntelligence()
    {
        if (intelligence > intelligenceMin)
        {
            UpdateStatText("Intelligence", -1);
        }
    }
    public void IncStrength()
    {
        if (statPoints > 0)
        {
            UpdateStatText("Strength", 1);
        }
    }

    public void DecStrength()
    {
        if (strength > strengthMin)
        {
            UpdateStatText("Strength", -1);
        }
    }
    public void IncVitality()
    {
        if (statPoints > 0)
        {
            UpdateStatText("Vitality", 1);
        }
    }

    public void DecVitality()
    {
        if (vitality > vitalityMin)
        {
            UpdateStatText("Vitality", -1);
        }
    }

    //not even using this right now
    public void IncSkillPoints()
    {
        skillPoints += 1;
        skillPoinsTxt.text = "Unspent Skill Points: " + skillPoints;
    }

    public void DecSkillPoints()
    {
        skillPoints -= 1;
        skillPoinsTxt.text = "Unspent Skill Points: " + skillPoints;
    }

    public void UpdateStatText(string stat, int point)
    {
        statPoints -= point;
        statPoinsTxt.text = "Unspent Stat Points: " + statPoints;
        switch (stat)
        {
            case "Armor":
                armor += point;
                armorTxt.text = "Armor: " + armor;
                break;
            case "Dexterity":
                dexterity += point;
                dexterityTxt.text = "Dexterity: " + dexterity;
                break;
            case "Intelligence":
                intelligence += point;
                intelligenceTxt.text = "Intelligence: " + intelligence;
                break;
            case "Strength":
                strength += point;
                strengthTxt.text = "Strength: " + strength;
                break;
            case "Vitality":
                vitality += point;
                vitalityTxt.text = "Vitality: " + vitality;
                break;
            default:
                break;
        }
    }

    public void SaveStatPoints()
    {
        armorMin = armor;
        dexterityMin = dexterity;
        intelligenceMin = intelligence;
        strengthMin = strength;
        vitalityMin = vitality;
        maxPoints = statPoints;
    }

    public void UpdateStatText(int arm, int dex, int intel, int str, int vit)
    {
        //so you can't get skill points from armor
        armorMin += arm;
        dexterityMin += dex;
        intelligenceMin += intel;
        strengthMin += str;
        vitalityMin += vit;

        //for text change
        armor += arm;
        dexterity += dex;
        intelligence += intel;
        strength += str;
        vitality += vit;

        //change text
        armorTxt.text = "Armor: " + armor;
        dexterityTxt.text = "Dexterity: " + dexterity;
        intelligenceTxt.text = "Intelligence: " + intelligence;
        strengthTxt.text = "Strength: " + strength;
        vitalityTxt.text = "Vitality: " + vitality;
    }
}
