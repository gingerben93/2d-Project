﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellType { FIRE, HEAL }; //add more types here for items


public class Magic: MonoBehaviour
{

    public SpellType type;

    //item information
    public string spellName;
    public string description;

    //sprite
    public Sprite spriteNeutral;


    void Start()
    {
        //might need this later
    }

    public void Use()
    {
        switch (type)
        {
            case SpellType.FIRE:
 
            default:
                break;
        }
    }

    public string GetToolTip()
    {
        string stats = string.Empty;
        string color = string.Empty;
        string newLine = string.Empty;

        if (description != string.Empty)
        {
            newLine = "\n";
        }

        //description if wanted
        return string.Format("<color=" + color + "><size=16>{0}</size></color><size=14><i><color=lime>" + newLine + "{1}</color></i>{2}</size>", spellName, description, stats);
    }

}
