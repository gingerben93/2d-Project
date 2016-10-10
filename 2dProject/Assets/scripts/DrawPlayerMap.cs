using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

public class DrawPlayerMap : MonoBehaviour {

    public MeshFilter playerLocalMap;
    public MeshFilter playerWorldMap;
    public GameObject player;

    public Transform tempMap;
    //private Vector3 offset;

    //map bools
    bool localMapOn = false;
    bool worldMapOn = false;
    bool makeMap = true;

    //if change room bool
    public bool touchingDoor = false;

    //for map marker
    GameObject MapMarkerTeemo;
    public List<Vector3> MapPos;
    int mapSeed;

    void Start()
    {
        MapMarkerTeemo = GameObject.Find("MapMarker");
        MapPos = new List<Vector3>();
        //mapSeed = Int32.Parse(GameObject.FindObjectOfType<MapGenerator>().seed);
    }

    // Update is called once per frame
    void LateUpdate () {
        mapSeed = Int32.Parse(GameObject.FindObjectOfType<MapGenerator>().seed);
        // local map
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (localMapOn)
            {
                //turn map off and erase it
                localMapOn = false;
                playerLocalMap.mesh = null;
            }
            else
            {
                //turn map on and draw it
                worldMapOn = false;
                playerWorldMap.mesh = null;
                localMapOn = true;
                DrawLocalMap();
            }
        }
        if(touchingDoor == true && Input.GetKeyDown(KeyCode.R) && localMapOn)
        {
            DrawLocalMap();
            touchingDoor = false;
        }
        if (localMapOn)
        {
            //keep map locked on character
            UpdatePosition();
        }

        //world map
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (worldMapOn)
            {
                //turn off teemo marker
                MapMarkerTeemo.GetComponent<SpriteRenderer>().enabled = false;
                //turn map off and erase it
                worldMapOn = false;
                playerWorldMap.mesh = null;
            }
            else
            {
                //TURN ON TEEMO MARKER
                MapMarkerTeemo.GetComponent<SpriteRenderer>().enabled = true;
                //turn map on and draw it
                worldMapOn = true;
                playerLocalMap.mesh = null;
                localMapOn = false;
                DrawWorldMap();
            }
        }
        if (worldMapOn)
        {
            //keep map locked on character
            MapMarkerTeemo.GetComponent<Transform>().position = player.transform.position + (MapPos[mapSeed] * 3.5f) + (player.transform.position * .035f);
            //MapMarkerTeemo.transform.position = transform.position;
            UpdatePosition();
        }
    }

    void DrawPlayerOnMap()
    {

    }

    void DrawLocalMap()
    {        
        MapGenerator mapGen = FindObjectOfType<MapGenerator>();

        //use this to get all maps

        string mapSeed = mapGen.seed;

        //load map in
        var loadPath = "Assets/CurrentMaps/" + mapSeed + ".asset";
        Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath(loadPath, typeof(Mesh));
        playerLocalMap.mesh = mesh;

        //scale size down and set position
        transform.localScale = new Vector3(.035f, .035f, .035f);
        transform.position = player.transform.position;
        transform.eulerAngles = new Vector3(270, 0, 0);
        //transform.Rotate(Vector3.zero);
    }

    void UpdatePosition()
    {
        if (localMapOn)
        {
            transform.position = player.transform.position - new Vector3(0, -3, 0);
        }
        else
        {
            transform.position = player.transform.position;
        }
    }

    void DrawWorldMap()
    {
        if (makeMap)
        {
            makeMap = false;
            GameData gameData = FindObjectOfType<GameData>();
            int totalMaps = gameData.mapSeed.Count;

            MeshFilter[] meshFilters = new MeshFilter[totalMaps];

            //create prefabs for all maps then add them to a list of filters, then remove all prefabs later
            for (int x = 0; x < totalMaps; x++)
            {
                string mapSeed = gameData.mapSeed[x];
                var loadPath = "Assets/CurrentMaps/" + mapSeed + ".asset";
                Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath(loadPath, typeof(Mesh));
               
                // create prefab
                var tempMapPrefab = Instantiate(tempMap) as Transform;
                tempMapPrefab.transform.SetParent(transform);
                tempMapPrefab.name = mapSeed;
                tempMapPrefab.GetComponent<MeshFilter>().mesh = mesh;
                meshFilters[x] = tempMapPrefab.GetComponent<MeshFilter>();
            }

            CombineInstance[] combine = new CombineInstance[totalMaps];

            Debug.Log("combine.Length = " + combine.Length);

            //put all mesh filters into the combiner object
            for (int x = 0; x < totalMaps; x++)
            {
                combine[x].mesh = meshFilters[x].sharedMesh;
                //draw maps in a polygon shape based on how many there are
                combine[x].transform = Matrix4x4.TRS(new Vector3(Mathf.Cos(2 * Mathf.PI * x / totalMaps) * 100, 0, Mathf.Sin(2 * Mathf.PI * x / totalMaps) * 100), Quaternion.identity, new Vector3(1, 1, 1));
                MapPos.Add(new Vector2(Mathf.Cos(2 * Mathf.PI * x / totalMaps), Mathf.Sin(2 * Mathf.PI * x / totalMaps)));
            }
            //65k max vertices or won't work
            playerWorldMap.mesh.CombineMeshes(combine);

            //set scale
            transform.localScale = new Vector3(.035f, .035f, .035f);
            transform.position = player.transform.position;

            //removes all the children of map object
            foreach (Transform child in transform)
            {
                if (child.name != "MapMarker")
                {
                    GameObject.Destroy(child.gameObject);
                }
            }

            //save fullmap to assests
            string worldMap = "WorldMap";
            var savePath = "Assets/CurrentMaps/" + worldMap + ".asset";
            Debug.Log("Saved Mesh to:" + savePath);
            AssetDatabase.CreateAsset(playerWorldMap.mesh, savePath);
            
        }
        else
        {
            //load map in
            string worldMap = "WorldMap";
            var loadPath = "Assets/CurrentMaps/" + worldMap + ".asset";
            Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath(loadPath, typeof(Mesh));
            transform.localScale = new Vector3(.035f, .035f, .035f);
            transform.position = player.transform.position;
            playerWorldMap.mesh = mesh;
        }
    }
   
}
