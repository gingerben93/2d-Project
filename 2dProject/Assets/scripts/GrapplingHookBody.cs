using UnityEngine;
using System.Collections;

public class GrapplingHookBody : MonoBehaviour
{
    DrawGrapHook hasBodyCollided;

    void Start()
    {
        hasBodyCollided = FindObjectOfType<DrawGrapHook>();
    }

    public void OnTriggerEnter2D(Collider2D tip)
    {
        hasBodyCollided.hasBodyCollided = true;
    }
}
