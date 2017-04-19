using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodHands : MonoBehaviour {

    GodHandsFist godHandsFist;
    GodHandsFistPlayer godHandsFistPlayer;

    public Transform fistPrefab;
    public Transform fistPlayerPrefab;

    public Transform PlayerFistLocation;

    Transform fist;

    public void Attack(Vector3 targetLocation)
    {
        GameController.GameControllerSingle.GodhandsCanAttack = false;
        PlayerFistLocation = GameObject.Find("PlayerProjectiles").transform;

        fist = Instantiate(fistPlayerPrefab, PlayerFistLocation);
        fist.name = "Fist";

        godHandsFistPlayer = GameObject.Find("PlayerProjectiles").transform.GetComponentInChildren<GodHandsFistPlayer>();

        godHandsFistPlayer.tag = "Player";

        godHandsFistPlayer.pullAttackOn = true;
        godHandsFistPlayer.newTargetLocation = targetLocation;
        
        godHandsFistPlayer.gotoPosition = GameController.GameControllerSingle.transform.position;
        godHandsFistPlayer.userTransform = fist.GetComponent<Transform>();
        godHandsFistPlayer.userTransform.position = GameController.GameControllerSingle.transform.position;
    }

    public void AttackBoss(Vector3 targetLocation)
    {
        fist = Instantiate(fistPrefab, transform);
        fist.name = "Fist";

        godHandsFist = transform.GetComponentInChildren<GodHandsFist>();

        godHandsFist.tag = "Enemy";

        godHandsFist.pullAttackOn = true;
        godHandsFist.newTargetLocation = targetLocation;

        godHandsFist.gotoPosition = transform.position;
        godHandsFist.userTransform = fist.GetComponent<Transform>();
        godHandsFist.userTransform.position = transform.position;
    }
}
