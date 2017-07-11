using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour {

    public float maxHealth;
    public float currentHealth;
    public GameObject healthBar;
	
    void Update()
    {
        DecreseHealth();
    }

    void DecreseHealth()
    {
        float calcHealth = currentHealth / maxHealth;
        SetHealthBar(calcHealth);
    }

    void SetHealthBar(float health)
    {
        healthBar.transform.localScale = new Vector3(health, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }
}
