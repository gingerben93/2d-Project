using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

    public int damage = 1;

    /// Projectile damage player or enemies?
    public bool isEnemyShot = false;

    // Use this for initialization
    void Start () {
        // 2 - Limited time to live to avoid any leak
        Destroy(gameObject, 2);
    }
}
