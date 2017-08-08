using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireballScroll : MonoBehaviour {

    public Sprite sprite;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.Find("Fire").GetComponent<Button>().interactable = true;
            GameObject.Find("Fire").GetComponent<Image>().sprite = sprite;

            Destroy(gameObject);
        }
    }
}
