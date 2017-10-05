using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToMainGame : MonoBehaviour
{
    //[SerializeField]
    //private int scene;
    private bool loading = false;
    public string loadMap;

    //for getting mapgenerator
    private GameObject mapGenerator;

    public static DoorToMainGame DoorToMainGameSingle;

    void Awake()
    {
        if (DoorToMainGameSingle == null)
        {
            DontDestroyOnLoad(gameObject);
            DoorToMainGameSingle = this;
        }
        else if (DoorToMainGameSingle != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (loading)
        {
            loading = false;
            StartCoroutine(LoadNewScene());
        }
    }

    IEnumerator LoadNewScene()
    {
        PlayerController.PlayerControllerSingle.LockPosition();

        //load functions
        AsyncOperation async = SceneManager.LoadSceneAsync("Area1");

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }

        //lets player move after loading
        PlayerController.PlayerControllerSingle.UnLockPosition();

        PlayerController.PlayerControllerSingle.touchingDoor = false;
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            loading = true;
        }
    }
}
