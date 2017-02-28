using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatsMenu : MonoBehaviour {

    Text txt;

    // Use this for initialization
    void Start()
    {
        txt = gameObject.GetComponent<Text>();
        txt.text = "EXP: " + PlayerStats.playerStatistics.experiencePoints;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        txt.text = "EXP: " + PlayerStats.playerStatistics.experiencePoints;
    }
}
