using UnityEngine;
using System.Collections;

public class CamraController : MonoBehaviour
{

    private Transform player;

    private Vector3 offset;

    public static CamraController PLayerCamra;

    void Awake()
    {
        if (PLayerCamra == null)
        {
            DontDestroyOnLoad(gameObject);
            PLayerCamra = this;
        }
        else if (PLayerCamra != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        player = PlayerController.PlayerControllerSingle.transform;
        offset = transform.position - player.transform.position;
        transform.Rotate(Vector3.zero);
        transform.position = PlayerController.PlayerControllerSingle.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
        transform.Rotate(Vector3.zero);
    }
}
