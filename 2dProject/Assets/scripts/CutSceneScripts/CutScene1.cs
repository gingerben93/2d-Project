using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene1 : MonoBehaviour {

    public Camera cam;

    Vector3 targetLocation;
    float speed;
    float start, end;
    void Start () {
        cam.transform.position = new Vector3(5.5f, -6.7f, -1);
        cam.orthographicSize = 13f;
        StartCoroutine(WaitForScene());
    }

    void Update()
    {
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, targetLocation, speed);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, end, .10f);
    }

    IEnumerator WaitForScene()
    {
        end = 13f;
        yield return new WaitForSeconds(2);
        speed = 2.0f;
        end = 3.5f;


        targetLocation = new Vector3(0.06f, 1.51f, -1.0f);
        yield return new WaitForSeconds(2);

        targetLocation = new Vector3(9.02f, 1.78f, -1.0f);
        end = 3.3f;
        yield return new WaitForSeconds(2);

        targetLocation = new Vector3(0.00f, -6.50f, -1.0f);
        end = 4f;
        yield return new WaitForSeconds(2);

        targetLocation = new Vector3(5.80f, -6.50f, -1.0f);
        yield return new WaitForSeconds(2);

        targetLocation = new Vector3(12.60f, -6.00f, -1.0f);
        yield return new WaitForSeconds(2);

        targetLocation = new Vector3(0.70f, -14.40f, -1.0f);
        end = 3.2f;
        yield return new WaitForSeconds(2);

        speed = 0.1f;
        targetLocation = new Vector3(11.70f, -14.40f, -1.0f);


    }

}
