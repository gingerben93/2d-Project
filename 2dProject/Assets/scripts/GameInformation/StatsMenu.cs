using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatsMenu : MonoBehaviour {

    public Text experiencePointsTxt;
    public Text levelTxt;

    // Use this for initialization
    void Start()
    {
        experiencePointsTxt.text = "EXP: " + PlayerStats.playerStatistics.experiencePoints;
        levelTxt.text = "Level: " + PlayerStats.playerStatistics.level;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        experiencePointsTxt.text = "EXP: " + PlayerStats.playerStatistics.experiencePoints;
        levelTxt.text = "Level: " + PlayerStats.playerStatistics.level;
    }
}
