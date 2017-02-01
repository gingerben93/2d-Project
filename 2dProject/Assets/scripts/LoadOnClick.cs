using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadOnClick : MonoBehaviour {

    [SerializeField]
    private int scene;
    [SerializeField]
    private Text loadingText;

    private bool loading = false;
    private bool loadScene = false;

    void Update() {
        if (loading) {

            loadScene = true;
            loading = false;
            loadingText.text = "Loading...";
            StartCoroutine(LoadNewScene());

        }
        if (loadScene == true) {
            
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, .25f));

        }

    }

    //load
    public void LoadScene(int level)
    {
        loading = true;
    }

    IEnumerator LoadNewScene() {
        AsyncOperation async = SceneManager.LoadSceneAsync("MainGame");

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone) {
            yield return null;
        }

    }

}
