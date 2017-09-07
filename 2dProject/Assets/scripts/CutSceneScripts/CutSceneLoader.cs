using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneLoader : MonoBehaviour
{
    //[SerializeField]
    //private int scene;
    public bool loadCutScene = false;
    public bool loadBackToGame = false;
    private string CutSceneName = "NameOfCutSceneToLoad";
    public string LoadNewScene = "NewcutScene";

    //for getting mapgenerator
    private GameObject mapGenerator;
    public GameObject[] allCurrentGameObjects;

    public static CutSceneLoader CutSceneLoaderSingle;

    void Awake()
    {
        if (CutSceneLoaderSingle == null)
        {
            DontDestroyOnLoad(gameObject);
            CutSceneLoaderSingle = this;
        }
        else if (CutSceneLoaderSingle != this)
        {
            Destroy(gameObject);
        }

        //allCurrentGameObjects = FindObjectsOfType<GameObject>();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (loadCutScene)
        {
            //sets all gameobjects that are not part of the cut scene to false
            Time.timeScale = 1;
            allCurrentGameObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject ob in allCurrentGameObjects)
            {
                if (ob.name != gameObject.name)
                {
                    ob.SetActive(false);
                }
            }
            loadCutScene = false;
            StartCoroutine(LoadCutScene());
        }

        if (loadBackToGame)
        {
            Time.timeScale = 1;
            foreach (GameObject ob in allCurrentGameObjects)
            {
                if (ob)
                {
                    if (ob.name != "CutScene")
                    {
                        ob.SetActive(true);
                    }
                }
            }
            StartCoroutine(LoadOldScene());
            loadBackToGame = false;
        }
    }

    public void loadScene(string name)
    {
        CutSceneName = name;
        loadCutScene = true;
    }

    IEnumerator LoadCutScene()
    {
        //load function
        AsyncOperation async = SceneManager.LoadSceneAsync(CutSceneName);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadOldScene()
    {
        //load function
        AsyncOperation async = SceneManager.LoadSceneAsync(LoadNewScene);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }
    }
}
