using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{

    //[SerializeField]
    //private int scene;
    [SerializeField]
    private Text loadingText;

    private bool loadNew = false;
    private bool loadGame = false;
    private bool isSceneLoading = false;
    public string loadMap = "MainGame";

    void Update()
    {
        if (loadNew)
        {

            isSceneLoading = true;
            loadNew = false;
            loadingText.text = "Loading...";
            StartCoroutine(LoadNewScene());
        }
        if (loadGame)
        {
            isSceneLoading = true;
            loadGame = false;
            loadingText.text = "Continue...";
            StartCoroutine(LoadNewScene());
        }
        if (isSceneLoading == true)
        {
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, .1f));
        }

    }

    //load
    public void LoadScene(int level)
    {
        loadNew = true;
    }

    public void ContinueGame(int level)
    {
        loadGame = true;
        GameObject GameLoaderObject = Instantiate((GameObject)Resources.Load("player/GameLoader", typeof(GameObject)));
        GameLoaderObject.name = "GameLoader";
        DontDestroyOnLoad(GameLoaderObject);
    }

    IEnumerator LoadNewScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(loadMap);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }
    }

    IEnumerator ContinueScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(loadMap);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }
    }
}
