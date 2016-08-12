using UnityEngine;
using System.Collections;

public class CamraController : MonoBehaviour
{

    public GameObject player;

    private Vector3 offset;

    // Use this for initialization
    void Start()
    {
        offset = transform.position - player.transform.position;
        transform.Rotate(Vector3.zero);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
        transform.Rotate(Vector3.zero);
    }
}
