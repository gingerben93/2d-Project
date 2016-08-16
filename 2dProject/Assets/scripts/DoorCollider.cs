using UnityEngine;
using System.Collections;

public class DoorCollider : MonoBehaviour {

    public void OnCollisionEnter2D(Collision2D node)
    {
        if (node.gameObject.tag == "Door")
        {
            Destroy(node.gameObject);
        }
    }
}
