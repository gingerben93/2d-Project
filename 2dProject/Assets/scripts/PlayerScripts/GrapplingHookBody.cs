using UnityEngine;
using System.Collections;

public class GrapplingHookBody : MonoBehaviour
{
    DrawGrapHook drawGrapHook;

    public GameObject explosion;

    void Start()
    {
        drawGrapHook = FindObjectOfType<DrawGrapHook>();
    }

    //public void OnCollisionEnter2D(Collision2D coll)
    ////public void OnTriggerEnter2D(Collider2D tip)
    //{

    //    foreach (ContactPoint2D missileHit in coll.contacts)
    //    {
    //        Vector2 hitPoint = missileHit.point;
    //        //Instantiate(explosion, new Vector3(hitPoint.x, hitPoint.y, 0), Quaternion.identity);

    //        //update all variables in drawGrapHook for line
    //        drawGrapHook.rb2dTip.gameObject.transform.position = new Vector2(hitPoint.x, hitPoint.y) + missileHit.normal*.3f;
    //        drawGrapHook.line.SetPosition(1, new Vector2(hitPoint.x, hitPoint.y));
    //        //drawGrapHook.line.numPositions += 1;
    //        drawGrapHook.currentPosLine = new Vector3(hitPoint.x, hitPoint.y, 0);
    //    }

    //    Debug.Log("body works");
    //    //drawGrapHook.hasBodyCollided = true;
    //}

    public void OnCollisionStay2D(Collision2D coll)
    //public void OnTriggerEnter2D(Collider2D tip)
    {
        foreach (ContactPoint2D missileHit in coll.contacts)
        {
            Vector2 hitPoint = missileHit.point;
            //Instantiate(explosion, new Vector3(hitPoint.x, hitPoint.y, 0), Quaternion.identity);
            
            //update all variables in drawGrapHook for line
            drawGrapHook.rb2dTip.gameObject.transform.position = new Vector2(hitPoint.x, hitPoint.y) + missileHit.normal * .3f;
            drawGrapHook.line.SetPosition(1, new Vector2(hitPoint.x, hitPoint.y));
            //drawGrapHook.line.numPositions += 1;
            drawGrapHook.currentPosLine = new Vector3(hitPoint.x, hitPoint.y, 0);
            break;
        }
    }
}
