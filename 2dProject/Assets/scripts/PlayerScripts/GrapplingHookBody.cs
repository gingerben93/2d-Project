using UnityEngine;
using System.Collections;

public class GrapplingHookBody : MonoBehaviour
{
    DrawGrapHook drawGrapHook;

    void Start()
    {
        drawGrapHook = FindObjectOfType<DrawGrapHook>();
    }

    public void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            //drawGrapHook.rb2dTip.MovePosition(coll.gameObject.transform.position);
            drawGrapHook.rb2dTip.gameObject.transform.position = coll.gameObject.transform.position;
            drawGrapHook.line.SetPosition(drawGrapHook.currentNumberLines - 2, coll.gameObject.transform.position);
            drawGrapHook.currentPosLine = coll.gameObject.transform.position;
            //drawGrapHook.joint.distance = Vector2.Distance(drawGrapHook.currentPosLine, PlayerController.PlayerControllerSingle.transform.position);
        }
        if(coll.gameObject.tag == "Untagged")
        {
            foreach (ContactPoint2D missileHit in coll.contacts)
            {
                Vector2 hitPoint = missileHit.point;

                //update all variables in drawGrapHook for line
                drawGrapHook.rb2dTip.gameObject.transform.position = new Vector2(hitPoint.x, hitPoint.y) + missileHit.normal * .3f;
                drawGrapHook.line.SetPosition(drawGrapHook.currentNumberLines - 2, new Vector2(hitPoint.x, hitPoint.y));
                //drawGrapHook.line.numPositions += 1;
                drawGrapHook.currentPosLine = new Vector3(hitPoint.x, hitPoint.y, 0);
                drawGrapHook.joint.distance = Vector2.Distance(drawGrapHook.currentPosLine, PlayerController.PlayerControllerSingle.transform.position);
                break;
            }
        }
    }
}
