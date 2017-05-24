using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToMainGame : MonoBehaviour
{
    //[SerializeField]
    //private int scene;
    private bool loading = false;
    public string loadMap = "MainGame";

    //for getting mapgenerator
    private GameObject mapGenerator;

    void Awake()
    {
        mapGenerator = LoadOnClick.LoadOnClickSingle.mapGenerator;
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (loading)
        {
            loadMap = "MainGame";
            mapGenerator.SetActive(true);

            GameController.GameControllerSingle.transform.position = GameController.GameControllerSingle.respawnLocation;
            loading = false;

            StartCoroutine(LoadNewScene());

        }
    }

    IEnumerator LoadNewScene()
    {
        GameObject hero = GameObject.Find("Hero");
        //stops player from moving during loading
        hero.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        //deactivates gamecon to stop player from doing anything while game is loading
        GameController.GameControllerSingle.isGameLoading = true;

        //load functions
        AsyncOperation async = SceneManager.LoadSceneAsync("MainGame");

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }

        //lets player move after loading
        hero.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        hero.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        hero.transform.rotation = Quaternion.identity;

        //actives game controler for player actions
        GameController.GameControllerSingle.isGameLoading = false;
        GameController.GameControllerSingle.touchingDoor = false;
        
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
