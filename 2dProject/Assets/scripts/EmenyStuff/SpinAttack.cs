using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttack : MonoBehaviour {

    float distanceToPlayer;

    GameObject closeBar;
    GameObject farBar;
    SpriteRenderer closeImage;
    SpriteRenderer farImage;

    // Use this for initialization
    void Start ()
    {
        closeBar = transform.Find("CloseBar").gameObject;
        farBar = transform.Find("FarBar").gameObject;
        closeImage = closeBar.transform.GetComponentInChildren<SpriteRenderer>();
        farImage = farBar.transform.GetComponentInChildren<SpriteRenderer>();

        distanceToPlayer = Vector2.Distance(PlayerController.PlayerControllerSingle.transform.position, transform.position);

        closeBar.transform.localPosition = new Vector3(0, distanceToPlayer * .25f, 0);
        closeBar.GetComponent<BoxCollider2D>().size = new Vector2(1, distanceToPlayer * .5f);
        closeImage.size = new Vector2(1, distanceToPlayer / 2);

        farBar.transform.localPosition = new Vector3(0, distanceToPlayer, 0);
        farBar.GetComponent<BoxCollider2D>().size = new Vector2(1, distanceToPlayer * .5f);
        farImage.size = new Vector2(1, distanceToPlayer / 2);

    }
	
	// Update is called once per frame
	void Update ()
    {


        if (Vector2.Distance(PlayerController.PlayerControllerSingle.transform.position, transform.position) <= 500f)
        {
            transform.eulerAngles = new Vector3(0, 0, (transform.eulerAngles.z - 1));
        }
	}
}
