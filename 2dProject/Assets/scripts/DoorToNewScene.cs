using UnityEngine;

public class DoorToNewScene : MonoBehaviour
{
    public string sceneToLoad;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameController.GameControllerSingle.loadScence(sceneToLoad);
            GameController.GameControllerSingle.transform.position = Vector3.zero;
            LoadOnClick.LoadOnClickSingle.mapGenerator.SetActive(false);
        }
    }
}
