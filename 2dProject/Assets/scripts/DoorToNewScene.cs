﻿using UnityEngine;

public class DoorToNewScene : MonoBehaviour
{
    public string sceneToLoad;

    void Update()
    {
        if (GameController.GameControllerSingle.questTravel)
        {
            GameController.GameControllerSingle.questTravel = false;
            GameController.GameControllerSingle.loadScence(sceneToLoad);
            PlayerController.PlayerControllerSingle.transform.position = Vector3.zero;
        }
    }
}
