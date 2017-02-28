using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameLoader : MonoBehaviour {

    public static GameLoader GameLoaderSingle;

    public PlayerData playerDat;

    void Awake()
    {
        if (GameLoaderSingle == null)
        {
            GameLoaderSingle = this;
        }
        else if (GameLoaderSingle != this)
        {
            Destroy(gameObject);
        }
        //GameObject HeroObject = Instantiate((GameObject)Resources.Load("player/Hero", typeof(GameObject)));
        //HeroObject.name = "Hero1";
        //GameObject savedGameData = Instantiate((GameObject)Resources.Load("player/GameData", typeof(GameObject)));
        //savedGameData.name = "GameData1";
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            playerDat = (PlayerData)bf.Deserialize(file);
            file.Close();
        }
    }
}