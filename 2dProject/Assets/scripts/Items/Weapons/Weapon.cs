using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public virtual void Attack()
    {
        Debug.Log("base attack find override");
    }

    //public virtual void onCollide()
    //{
    //    Debug.Log("base onCollide find override");
    //}
}
