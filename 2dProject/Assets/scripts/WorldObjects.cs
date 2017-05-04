using UnityEngine;

public class WorldObjects : MonoBehaviour
{
    public static WorldObjects WorldObjectsSingle;

    void Awake()
    {
        if (WorldObjectsSingle == null)
        {
            DontDestroyOnLoad(gameObject);
            WorldObjectsSingle = this;
        }
        else if (WorldObjectsSingle != this)
        {
            Destroy(gameObject);
        }
    }
}
