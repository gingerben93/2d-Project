using UnityEngine;

public class DoorToNewScene : MonoBehaviour
{
    public string sceneToLoad;

    void Update()
    {
        if (GameController.GameControllerSingle.questTravel)
        {
            //for checking quest when loading scenes
            QuestController.QuestControllerSingle.isQuestCurrent = false;

            GameController.GameControllerSingle.questTravel = false;
            GameController.GameControllerSingle.loadScence(sceneToLoad);
            GameController.GameControllerSingle.transform.position = Vector3.zero;
            LoadOnClick.LoadOnClickSingle.mapGenerator.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameController.GameControllerSingle.touchingQuestDoor = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameController.GameControllerSingle.touchingQuestDoor = false;
        }
    }
}
