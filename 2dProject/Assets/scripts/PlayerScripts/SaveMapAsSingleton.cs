using UnityEngine;

public class SaveMapAsSingleton : MonoBehaviour {

    public static SaveMapAsSingleton SaveTheMap;

    void Awake()
    {
        if (SaveTheMap == null)
        {
            DontDestroyOnLoad(gameObject);
            SaveTheMap = this;
        }
        else if (SaveTheMap != this)
        {
            Destroy(gameObject);
        }
    }
}
