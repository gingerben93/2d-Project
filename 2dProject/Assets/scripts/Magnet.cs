using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{

    public GameObject item;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(item.transform.position, GameObject.Find("Hero").transform.position) < 5)
        {
            transform.position = Vector3.MoveTowards(item.transform.position, GameObject.Find("Hero").transform.position, Time.deltaTime * 6);
        }
    }
}
