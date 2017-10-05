using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

    // weapon prefab
    public Transform shotPrefab;
    public float shootingRate = 0.25f;

    private float shootCooldown;

    // Use this for initialization
    void Start ()
    {
        shootCooldown = 0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
    }
}
