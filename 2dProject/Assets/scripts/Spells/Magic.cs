﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum SpellType { FIRE, ICE, ZAP, HEAL, UNKNOWN }; //add more types here for items


public class Magic : MonoBehaviour
{

    public SpellType type;

    //item information
    public string spellName;
    public string description;

    //sprite
    public Sprite spriteNeutral;

    //spell information
    private GameObject spellPrefab;
    private GameObject spellTransform;
    public float spellRate = 0.25f;

    private float spellCooldown;

    void Start()
    {
        //might need this later
    }

    public void Cast()
    {
        switch (type)
        {
            case SpellType.FIRE:
                Debug.Log("PewPewFireball");
                spellPrefab = Resources.Load("Prefabs/Spells/Fireball", typeof(GameObject)) as GameObject;
                spellCooldown = 0f;

                // Create a new spell
                spellTransform = Instantiate(spellPrefab, GameObject.Find("PlayerProjectiles").transform) as GameObject;
     
                break;
            case SpellType.HEAL:
                Debug.Log("HEALHEALHEAL");
                spellPrefab = Resources.Load("Prefabs/Spells/Heal", typeof(GameObject)) as GameObject;
                spellCooldown = 0f;

                // Create a new spell
                spellTransform = Instantiate(spellPrefab, GameObject.Find("PlayerProjectiles").transform) as GameObject;

                break;
            case SpellType.ICE:
                Debug.Log("Frozen Solid");
                break;
            case SpellType.ZAP:
                Debug.Log("Zap ZAP Zap");
                break;
            case SpellType.UNKNOWN:
                Debug.Log("Spell Aint Nuffin");
                break;
            default:
                Debug.Log("Default case");
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