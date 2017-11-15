using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttack : MonoBehaviour {

    //variables
    float length;
    float speed;

    GameObject closeBar;
    GameObject farBar;
    SpriteRenderer closeImage;
    SpriteRenderer farImage;

    //for particle systems
    private ParticleSystem FarParticle;
    private ParticleSystem CloseParticle;
    ParticleSystem.ShapeModule shapeModule;

    // Use this for initialization
    void Start ()
    {
        closeBar = transform.Find("CloseBar").gameObject;
        closeImage = closeBar.transform.GetComponentInChildren<SpriteRenderer>();
        CloseParticle = closeBar.transform.GetComponentInChildren<ParticleSystem>();

        farBar = transform.Find("FarBar").gameObject;
        farImage = farBar.transform.GetComponentInChildren<SpriteRenderer>();
        FarParticle = farBar.transform.GetComponentInChildren<ParticleSystem>();

        //transform.position = new Vector2(Random.Range(-50, 50), Random.Range(-50, 50));

        //length = Vector2.Distance(PlayerController.PlayerControllerSingle.transform.position, transform.position);
        length = Random.Range(10, 50);
        speed = Random.Range(.1f, 1f);

        //2+ for offset of rotation point
        closeBar.transform.localPosition = new Vector3(0, 2 + length * .25f, 0);
        closeBar.GetComponent<BoxCollider2D>().size = new Vector2(1, length * .5f);
        closeImage.size = new Vector2(1, length / 2);
        shapeModule = CloseParticle.shape;
        shapeModule.radius = length / 4;

        farBar.transform.localPosition = new Vector3(0, length, 0);
        farBar.GetComponent<BoxCollider2D>().size = new Vector2(1, length * .5f);
        farImage.size = new Vector2(1, length / 2);
        shapeModule = FarParticle.shape;
        shapeModule.box = new Vector3(1, length / 2, 1);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Vector2.Distance(PlayerController.PlayerControllerSingle.transform.position, transform.parent.transform.position) <= 500f)
        {
            transform.eulerAngles = new Vector3(0, 0, (transform.eulerAngles.z - speed));
        }
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Debug.Log("hit player");
            PlayerController.PlayerControllerSingle.DamagePlayer(1);
        }
    }
}
