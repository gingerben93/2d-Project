using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour {

    // Use this for initialization
    void Start() {
        if (PlayerStats.PlayerStatsSingle.health < PlayerStats.PlayerStatsSingle.maxHealth) { 
            PlayerStats.PlayerStatsSingle.health += 1;
        }

        Destroy(gameObject, 1);
    }

    void Update()
    {
        transform.position = new Vector3(GameObject.Find("Hero").transform.position.x, GameObject.Find("Hero").transform.position.y + 1, 0);
    }
}
