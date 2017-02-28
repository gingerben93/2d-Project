using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGrapplingHook : MonoBehaviour {

    public static SaveGrapplingHook SaveGrapplingHookSingle;

    void Awake()
    {
        if (SaveGrapplingHookSingle == null)
        {
            DontDestroyOnLoad(gameObject);
            SaveGrapplingHookSingle = this;
        }
        else if (SaveGrapplingHookSingle != this)
        {
            Destroy(gameObject);
        }
    }
}
