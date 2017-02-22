using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadOnClick : MonoBehaviour {

    //[SerializeField]
    //private int scene;
    [SerializeField]
    private Text loadingText;

    private bool loading = false;
    private bool loadScene = false;
    public string loadMap = "StartArea";

    public static LoadOnClick LoadClick;

    // for getting scene name
    Scene scene;

    //for getting mapgenerator
    private GameObject mapGenerator;

    void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();
        mapGenerator = GameObject.Find("MapGenerator");
        loadMap = scene.name;
        if (scene.name != "StartMenu")
        {
            if (LoadClick == null)
            {
                DontDestroyOnLoad(gameObject);
                LoadClick = this;
            }
            else if (LoadClick != this)
            {
                Destroy(gameObject);
            }
        }
    }

    void Update() {
        if (loading) {
            if(loadMap == "StartArea")
            {
                loadMap = "MainGame";
                mapGenerator.SetActive(true);
            }
            else if (loadMap == "MainGame")
            {
                loadMap = "StartArea";
                mapGenerator.SetActive(false);
            }
            else
            {
                loadMap = "MainGame";
            }

            loadScene = true;
            loading = false;
            loadingText.text = "Loading...";
            //GameController.GameControllerSingle.transform.position = new Vector3(0, 0, 0);
            StartCoroutine(LoadNewScene());

        }
        if (loadScene == true) {
            
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, .1f));
        }

    }

    //load
    public void LoadScene(int level)
    {
        loading = true;
    }

    IEnumerator LoadNewScene() {
        AsyncOperation async = SceneManager.LoadSceneAsync(loadMap);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone) {
            yield return null;
        }
        loadingText.text = "";
    }
}
