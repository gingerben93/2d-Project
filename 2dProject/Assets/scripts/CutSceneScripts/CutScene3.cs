using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutScene3 : MonoBehaviour {

    public Camera cam;
    public Text cutSceneText;

    Vector3 targetLocation;
    float speed;
    float start, end;

    //public static CutSceneLoader CutSceneLoaderSingle;

    void Start () {
        cam.transform.position = new Vector3(5.5f, -6.7f, -1);
        cam.orthographicSize = 13f;
        StartCoroutine(WaitForScene());
    }

    void Update()
    {
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, targetLocation, speed);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, end, .10f);

        //for pausing cut scene
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
            if (Time.timeScale >= 0)
            {
                Debug.Log("> 0");
                Time.timeScale = 0;
            }
            else
            {
                Debug.Log("= 0");
                Time.timeScale = 1;
            }
        }

        //up time scale to scroll to next panel quicker
        if (Input.GetMouseButtonDown(0))
        {
            Time.timeScale = 5;
            Debug.Log("left click");
        }
        //for skipping scene
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("right click");
            CutSceneLoader.CutSceneLoaderSingle.loadBackToGame = true;
        }
    }

    IEnumerator WaitForScene()
    {
        end = 13f;
        cutSceneText.text = "NEED a cut scene three panel";
        yield return new WaitForSeconds(2);
        speed = 2.0f;
        end = 3.5f;

        Time.timeScale = 1;
        cutSceneText.text = "NEED a cut scene three panel";
        targetLocation = new Vector3(0.06f, 1.51f, -1.0f);
        yield return new WaitForSeconds(2);

        Time.timeScale = 1;
        cutSceneText.text = "Panel 2 : No Dialogue. Panel of full scene with Dead bodies and a group of people approaching";
        targetLocation = new Vector3(9.02f, 1.78f, -1.0f);
        end = 3.3f;
        yield return new WaitForSeconds(2);

        Time.timeScale = 1;
        cutSceneText.text = "Panel 3 : Dialogue by random civilains person1-'Divine Spirits help us more bodies' person2-'this has been happening too often lately.' ";
        targetLocation = new Vector3(0.00f, -6.50f, -1.0f);
        end = 4f;
        yield return new WaitForSeconds(2);

        Time.timeScale = 1;
        cutSceneText.text = "Panel 4: close up of PT while people still talk. person1-'How many times does this make in the past year?' person2;-'I've lost count.' PT coughs.";
        targetLocation = new Vector3(5.80f, -6.50f, -1.0f);
        yield return new WaitForSeconds(2);

        Time.timeScale = 1;
        cutSceneText.text = "Panel 5: civilians ruhing to PT. person1- 'Merciful lords and ladies someone's alive.";
        targetLocation = new Vector3(12.60f, -6.00f, -1.0f);
        yield return new WaitForSeconds(2);

        Time.timeScale = 1;
        cutSceneText.text = "Panel 6: Civilians picking him up. person1- 'Lets get this lucky basdard back to town.'  Person2-'Has there ever been a surviour from one of these atrocities?'";
        targetLocation = new Vector3(0.70f, -14.40f, -1.0f);
        end = 3.2f;
        yield return new WaitForSeconds(2);

        Time.timeScale = 1;
        cutSceneText.text = "Panel 6-8 PT geting taken away. person1-'No, Never'";
        speed = 0.1f;
        targetLocation = new Vector3(11.70f, -14.40f, -1.0f);

        Time.timeScale = 1;
        yield return new WaitForSeconds(3);

        //for loading to main game
        CutSceneLoader.CutSceneLoaderSingle.loadBackToGame = true;
    }

    IEnumerator LoadNewScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("StartArea");
        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }
    }
}
