using UnityEngine;

public class EnemyList : MonoBehaviour
{
    public static EnemyList EnemyListSingle;

    void Awake()
    {
        if (EnemyListSingle == null)
        {
            DontDestroyOnLoad(gameObject);
            EnemyListSingle = this;
        }
        else if (EnemyListSingle != this)
        {
            Destroy(gameObject);
        }
    }
}
