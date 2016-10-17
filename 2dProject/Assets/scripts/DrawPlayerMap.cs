﻿using UnityEngine;
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
    SpriteRenderer MapMarkerTeemoSprite;
    Transform MapMarkerTeemoPos;
    public List<Vector3> MapPos;
    int mapSeed;

    //for drawing door connections
    private List<List<Vector2>> doorLocations;
    public Transform mapLine;
    public int currentMap { get; set; }
    public int nextMap { get; set; }
    public int currentDoor { get; set; }
    public int nextDoor { get; set; }
    private Vector3 linePos1;
    private Vector3 linePos2;
    private List<string> drawnDoors;
    private List<Vector3> LinePos;

    //for line color
    Color firstColor;
    Color secondColor;

    void Start()
    {
        //for teemo mapmarker
        MapMarkerTeemo = GameObject.Find("MapMarker");
        MapMarkerTeemoSprite = MapMarkerTeemo.GetComponent<SpriteRenderer>();
        MapMarkerTeemoPos = MapMarkerTeemo.GetComponent<Transform>();
        MapPos = new List<Vector3>();
        //mapSeed = Int32.Parse(GameObject.FindObjectOfType<MapGenerator>().seed);



        //for line between doors map marker
        GameData data = FindObjectOfType<GameData>();
        doorLocations = new List<List<Vector2>>();
        doorLocations = data.doorlocations;
        drawnDoors = new List<string>();
        LinePos = new List<Vector3>();

    }

    // Update is called once per frame
    void LateUpdate () {
        //mapSeed = Int32.Parse(GameObject.FindObjectOfType<MapGenerator>().seed);
        //currentMap = Int32.Parse(GameObject.FindObjectOfType<DoorCollider>().oldSeed);
        //nextMap = Int32.Parse(GameObject.FindObjectOfType<DoorCollider>().newSeed);
        //currentDoor = Int32.Parse(GameObject.FindObjectOfType<DoorCollider>().oldDoor);
        //nextDoor = Int32.Parse(GameObject.FindObjectOfType<DoorCollider>().newDoor);

        // local map
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (localMapOn)
            {
                //turn off teemo marker
                MapMarkerTeemoSprite.enabled = false;
                //turn map off and erase it
                localMapOn = false;
                playerLocalMap.mesh = null;
            }
            else
            {
                //turn off teemo marker
                MapMarkerTeemoSprite.enabled = true;
                //turn map on and draw it
                worldMapOn = false;
                playerWorldMap.mesh = null;
                localMapOn = true;
                DrawLocalMap();
            }
        }
        if (touchingDoor == true && Input.GetKeyDown(KeyCode.R)) {
            if (localMapOn)
            {
                DrawLocalMap();
                touchingDoor = false;

            }
            //for drawing map lines
            bool test = false;
            foreach (Transform child in GameObject.Find("MapDoorLines").transform)
            {
                if (child.name == (currentMap.ToString() + currentDoor.ToString() + nextMap.ToString() + nextDoor.ToString()) || child.name == (nextMap.ToString() + nextDoor.ToString() + currentMap.ToString() + currentDoor.ToString()))
                {
                    test = true;
                    break;
                }
            }
            if (test == false)
            {
                //have to do a weird .45 shift in y, find out why later; might be due to vertex being at top of door
                linePos1 = (MapPos[currentMap] * 3.5f) + new Vector3(doorLocations[currentMap][currentDoor].x * .035f, doorLocations[currentMap][currentDoor].y * .035f, 0);
                linePos2 = (MapPos[nextMap] * 3.5f) + new Vector3(doorLocations[nextMap][nextDoor].x * .035f, doorLocations[nextMap][nextDoor].y * .035f, 0);

                //save pos for moving with player
                LinePos.Add(linePos1);
                LinePos.Add(linePos2);

                //Instantiate line
                var tempMapLine = Instantiate(mapLine) as Transform;
                tempMapLine.SetParent(GameObject.Find("MapDoorLines").transform);
                tempMapLine.GetComponent<LineRenderer>().SetWidth(.1f, .1f);
                tempMapLine.GetComponent<LineRenderer>().SetPosition(0, player.transform.position + linePos1);
                tempMapLine.GetComponent<LineRenderer>().SetPosition(1, player.transform.position + linePos2);
                tempMapLine.name = currentMap.ToString() + currentDoor.ToString() + nextMap.ToString() + nextDoor.ToString();

                //for color; try to enum the colors for each map i.e. map 0 is red, map 1 is blue ...
                firstColor = PickLineColor(currentMap);
                secondColor = PickLineColor(nextMap);

                tempMapLine.GetComponent<LineRenderer>().material.shader = Shader.Find("Self-Illumin/Specular");
                tempMapLine.GetComponent<LineRenderer>().material.color = Color.Lerp(firstColor, secondColor, .5f);

            }

        }

        if (localMapOn)
        {

            //keep map locked on character
            MapMarkerTeemoPos.position = player.transform.position + (new Vector3(0,1,0) * 3.5f) + (player.transform.position * .035f);
            //keep map locked on character
            UpdatePosition();
        }

        //world map
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (worldMapOn)
            {
                //turn off teemo marker
                MapMarkerTeemoSprite.enabled = false;
                //turn map off and erase it
                worldMapOn = false;
                playerWorldMap.mesh = null;
            }
            else
            {
                //TURN ON TEEMO MARKER
                MapMarkerTeemoSprite.enabled = true;
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
            MapMarkerTeemoPos.position = player.transform.position + (MapPos[currentMap] * 3.5f) + (player.transform.position * .035f);

            //update door lines pos
            int x = 0;
            foreach (Transform child in GameObject.Find("MapDoorLines").transform)
            {
                //can ig et the current line 0 position? i want that to currrent plus player pos
                child.GetComponent<LineRenderer>().SetPosition(0, player.transform.position + LinePos[x * 2]);
                child.GetComponent<LineRenderer>().SetPosition(1, player.transform.position + LinePos[x * 2 + 1]);
                x++;
            }

            //update map pos
            UpdatePosition();
        }
    }

    Color PickLineColor(int mapNum)
    {
        if (mapNum == 0)
        {
            return Color.red;
        }
        else if(mapNum == 1)
        {
            return Color.yellow;
        }
        else if (mapNum == 2)
        {
            return Color.green;
        }

        else if (mapNum == 3)
        {
            return Color.blue;
        }

        else if (mapNum == 4)
        {
            return Color.cyan;
        }
        else if (mapNum == 5)
        {
            return Color.magenta;
        }
        return Color.white;
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
            transform.position = player.transform.position - new Vector3(0, -3.5f, 0);
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
                //currently does nothngn beacuse map marker is not a child
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
