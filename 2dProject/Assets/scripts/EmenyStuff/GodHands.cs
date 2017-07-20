using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodHands : MonoBehaviour {

    GodHandsFist godHandsFist;
    GodHandsFistPlayer godHandsFistPlayer;

    public Transform fistPrefab;
    public GameObject fistPlayerPrefab;

    public Transform PlayerFistLocation;

    public Vector3 targetLocation;

    Transform fist;

    void Start()
    {
        fistPlayerPrefab = Resources.Load("Prefabs/WeaponProjectiles/FistPlayer", typeof(GameObject)) as GameObject;
    }

    public void Attack()
    {
        PlayerFistLocation = GameObject.Find("PlayerProjectiles").transform;
        GameObject weaponTransform = Instantiate(fistPlayerPrefab, GameObject.Find("PlayerProjectiles").transform) as GameObject;
        weaponTransform.name = "Fist";

        godHandsFistPlayer = GameObject.Find("PlayerProjectiles").transform.GetComponentInChildren<GodHandsFistPlayer>();

        godHandsFistPlayer.pullAttackOn = true;
        godHandsFistPlayer.newTargetLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        godHandsFistPlayer.gotoPosition = GameController.GameControllerSingle.transform.position;
        godHandsFistPlayer.userTransform = weaponTransform.GetComponent<Transform>();
        godHandsFistPlayer.userTransform.position = GameController.GameControllerSingle.transform.position;
    }

    public void AttackBoss(Vector3 targetLocation)
    {
        fist = Instantiate(fistPrefab, transform);
        fist.name = "Fist";

        godHandsFist = transform.GetComponentInChildren<GodHandsFist>();

        godHandsFist.tag = "EnemyProjectile";

        godHandsFist.pullAttackOn = true;
        godHandsFist.newTargetLocation = targetLocation;

        godHandsFist.gotoPosition = transform.position;
        godHandsFist.userTransform = fist.GetComponent<Transform>();
        godHandsFist.userTransform.position = transform.position;
    }
}
