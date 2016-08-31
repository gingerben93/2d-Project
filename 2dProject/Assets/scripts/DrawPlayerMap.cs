using UnityEngine;
using System.Collections;
using UnityEditor;

public class DrawPlayerMap : MonoBehaviour {

    public MeshFilter playerMap;
    public GameObject player;
    //private Vector3 offset;

    bool MapOn = false;
    // Update is called once per frame
    void LateUpdate () {

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (MapOn)
            {
                //turn map off and erase it
                MapOn = false;
                playerMap.mesh = null;
            }
            else
            {
                //turn map on and draw it
                MapOn = true;
                DrawMap();
            }
        }
        if (MapOn)
        {
            //keep map locked on character
            UpdatePosition();
        }
    }

    void DrawMap()
    {
        GameData gameData = FindObjectOfType<GameData>();
        MapGenerator mapGen = FindObjectOfType<MapGenerator>();

        /*
        for (int x = 0; x < gameData.mapSeed.Count; x++) {
            
        }
        */
        //use this to get all maps
        //string mapSeed = gameData.mapSeed[];

        string mapSeed = mapGen.seed;

        //load map in
        var loadPath = "Assets/CurrentMaps/" + mapSeed + ".asset";
        Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath(loadPath, typeof(Mesh));
        playerMap.mesh = mesh;

        //scale size down and set position
        transform.localScale = new Vector3(.05f, .05f, .05f);
        transform.position = player.transform.position;
        transform.Rotate(Vector3.zero);
    }

    void UpdatePosition()
    {
        transform.position = player.transform.position;
        transform.Rotate(Vector3.zero);
    }
}
