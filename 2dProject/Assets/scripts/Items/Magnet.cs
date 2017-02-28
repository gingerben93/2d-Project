using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{

    public GameObject item;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(item.transform.position, GameController.GameControllerSingle.transform.position) < 5)
        {
            transform.position = Vector3.MoveTowards(item.transform.position, GameController.GameControllerSingle.transform.position, Time.deltaTime * 6);
        }
    }
}
