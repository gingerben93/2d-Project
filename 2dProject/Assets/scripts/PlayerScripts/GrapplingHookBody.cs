using UnityEngine;
using System.Collections;

public class GrapplingHookBody : MonoBehaviour
{
    DrawGrapHook drawGrapHook;

    void Start()
    {
        drawGrapHook = transform.parent.GetComponentInChildren<DrawGrapHook>();
    }
    
    public void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.GetComponent<EdgeCollider2D>())
        {
            drawGrapHook.hitEnemy = false;
            foreach (ContactPoint2D missileHit in coll.contacts)
            {
                Vector2 hitPoint = missileHit.point;
                //Debug.Log("missileHit.point = " + missileHit.point);

                drawGrapHook.rb2dTip.gameObject.transform.position = new Vector2(hitPoint.x, hitPoint.y) + missileHit.normal * .3f;
                drawGrapHook.joint.distance = Vector2.Distance(drawGrapHook.rb2dTip.gameObject.transform.position, PlayerController.PlayerControllerSingle.transform.position);
                break;
            }
        }
    }
}
