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

    public static LoadOnClick LoadOnClickSingle;

    // for getting scene name
    Scene scene;

    void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();
        loadMap = scene.name;
        if (scene.name != "StartMenu")
        {
            if (LoadOnClickSingle == null)
            {
                //DontDestroyOnLoad(gameObject);
                LoadOnClickSingle = this;
            }
            else if (LoadOnClickSingle != this)
            {
                Destroy(gameObject);
            }
        }
    }

    void Update() {
        if (loading) {
            foreach (Transform child in WorldObjects.WorldObjectsSingle.transform)
            {
                foreach (Transform child2 in child)
                {
                    Destroy(child2.gameObject);
                }
            }
            loadMap = "StartArea";
            GameController.GameControllerSingle.transform.position = new Vector3(-0f, 1.2f, 0);

            loadScene = true;
            loading = false;
            loadingText.text = "Loading...";
            
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

    IEnumerator LoadNewScene()
    {
        GameObject hero = GameObject.Find("Hero");
        //stops player from moving during loading
        hero.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        //deactivates gamecon to stop player from doing anything while game is loading
        GameController.GameControllerSingle.isGameLoading = true;
        
        //load functions
        AsyncOperation async = SceneManager.LoadSceneAsync(loadMap);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone) {
            yield return null;
        }

        //lets player move after loading
        hero.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        hero.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        hero.transform.rotation = Quaternion.identity;

        //actives game controler for player actions
        GameController.GameControllerSingle.isGameLoading = false;
        GameController.GameControllerSingle.touchingDoor = false;
        GameController.GameControllerSingle.questTravel = false;
        loadingText.text = "";
    }
}
