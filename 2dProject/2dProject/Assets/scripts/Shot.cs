using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // 2 - Limited time to live to avoid any leak
        Destroy(gameObject, 2);
    }
}
