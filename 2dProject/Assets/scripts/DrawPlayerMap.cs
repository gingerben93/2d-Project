﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;

public class DrawPlayerMap : MonoBehaviour {

    public MeshFilter playerLocalMap;
    public MeshFilter playerWorldMap;
    public GameObject player;

    //for drawing doors on map
    public Transform doorPrefab;

    public Transform tempMap;
    //private Vector3 offset;

    //map bools
    private bool localMapOn = false;
    private bool worldMapOn = false;
    private bool makeMap = true;

    //if change room bool
    public bool touchingDoor { get; set; }

    //for map marker
    GameObject MapMarkerTeemo;
    SpriteRenderer MapMarkerTeemoSprite;
    Transform MapMarkerTeemoPos;
    public List<Vector3> MapPos;
    private int mapSeed;

    //for combine map and draw lines
    GameData gameData;
    private int totalMaps;

    //for drawing door connections
    private List<List<Vector2>> doorLocations;
    public Transform mapLine;
    public int currentMap { get; set; }
    public int nextMap { get; set; }
    public int currentDoor { get; set; }
    public int nextDoor { get; set; }
    private Vector3 linePos1;
    private Vector3 linePos2;
    private List<Vector3> LinePos;

    //for line color
    Color firstColor;
    Color secondColor;

    //for drawing doors
    private bool drawDoors = false;

    //for door map
    private bool firstRun = false;

    void Start()
    {
        //for drawing map
        gameData = FindObjectOfType<GameData>();
        totalMaps = gameData.mapSeed.Count;

        //map opacity
        transform.GetComponent<MeshRenderer>().material.color = new Vector4(1, 1, 1, .5f);

        // this is for teemo marker and lines
        MapPos = new List<Vector3>();
        for (int x = 0; x < totalMaps; x++)
        {
            MapPos.Add(new Vector2(Mathf.Cos(2 * Mathf.PI * x / totalMaps), Mathf.Sin(2 * Mathf.PI * x / totalMaps)));
        }

        //for teemo mapmarker
        MapMarkerTeemo = GameObject.Find("MapMarker");
        MapMarkerTeemoSprite = MapMarkerTeemo.GetComponent<SpriteRenderer>();
        MapMarkerTeemoPos = MapMarkerTeemo.GetComponent<Transform>();
        

        //for line between doors map marker
        doorLocations = new List<List<Vector2>>();
        doorLocations = gameData.doorlocations;
        LinePos = new List<Vector3>();

        //for making world map
        DrawWorldMap();
        //removes all the children of map object
        foreach (Transform child in transform)
        {
            //currently does nothngn beacuse map marker is not a child
            if (child.name != "MapMarker")
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        makeMap = false;
        playerWorldMap.mesh = null;

    }

    // Update is called once per frame
    void LateUpdate () {

        // local map
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (localMapOn)
            {
                //turn map doors off
                var oldMapDoors = GameObject.FindGameObjectsWithTag("MapDoor");
                foreach (var door in oldMapDoors)
                {
                    door.GetComponent<SpriteRenderer>().enabled = false;
                }

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

                //turn map doors on
                var oldMapDoors = GameObject.FindGameObjectsWithTag("MapDoor");
                foreach (var door in oldMapDoors)
                {
                    door.GetComponent<SpriteRenderer>().enabled = true;
                }

                //turn off world map lines
                foreach (Transform child in GameObject.Find("MapDoorLines").transform)
                {
                    child.GetComponent<LineRenderer>().enabled = false;
                }
            }
        }
        if (touchingDoor == true && Input.GetKeyDown(KeyCode.R)) {

            
            if (localMapOn)
            {
                drawDoors = false;
                DrawLocalMap();
                //touchingDoor = false;

            }
            else
            {
                //for door maps
                transform.localScale = new Vector3(.175f, .175f, .175f);
                transform.position = player.transform.position;
                drawDoors = false;
                DrawDoorsLocalMap(nextMap);
                var oldMapDoors = GameObject.FindGameObjectsWithTag("MapDoor");
                foreach (var door in oldMapDoors)
                {
                    door.GetComponent<SpriteRenderer>().enabled = false;
                }
                transform.localScale = new Vector3(.075f, .075f, .075f);
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
                CreateLines();
            }
        }

        if (localMapOn)
        {
            //keep map locked on character
            MapMarkerTeemoPos.position = player.transform.position + (player.transform.position * .175f);
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

                //turn off lines
                foreach (Transform child in GameObject.Find("MapDoorLines").transform)
                {
                    child.GetComponent<LineRenderer>().enabled = false;
                }
            }
            else
            {
                //turn map doors off
                var oldMapDoors = GameObject.FindGameObjectsWithTag("MapDoor");
                foreach (var door in oldMapDoors)
                {
                    door.GetComponent<SpriteRenderer>().enabled = false;
                }

                //turn on lines
                foreach (Transform child in GameObject.Find("MapDoorLines").transform)
                {
                    child.GetComponent<LineRenderer>().enabled = true;
                }
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
            MapMarkerTeemoPos.position = player.transform.position + GetCurrentMapLocation(currentMap) + (player.transform.position * .075f);

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

    void CreateLines()
    {
        //save line position
        linePos1 = GetCurrentMapLocation(currentMap) + new Vector3(doorLocations[currentMap][currentDoor].x * .075f, doorLocations[currentMap][currentDoor].y * .075f, 0);
        linePos2 = GetCurrentMapLocation(nextMap) + new Vector3(doorLocations[nextMap][nextDoor].x * .075f, doorLocations[nextMap][nextDoor].y * .075f, 0);

        //save pos for moving with player
        LinePos.Add(linePos1);
        LinePos.Add(linePos2);

        //Instantiate line
        var tempMapLine = Instantiate(mapLine) as Transform;
        tempMapLine.SetParent(GameObject.Find("MapDoorLines").transform);
        //tempMapLine.GetComponent<LineRenderer>().SetWidth(.1f, .1f); old version now outdated
        tempMapLine.GetComponent<LineRenderer>().startWidth = .1f;
        tempMapLine.GetComponent<LineRenderer>().endWidth = .1f;
        tempMapLine.GetComponent<LineRenderer>().SetPosition(0, player.transform.position + linePos1);
        tempMapLine.GetComponent<LineRenderer>().SetPosition(1, player.transform.position + linePos2);
        tempMapLine.name = currentMap.ToString() + currentDoor.ToString() + nextMap.ToString() + nextDoor.ToString();

        if (!worldMapOn)
        {
            tempMapLine.GetComponent<LineRenderer>().enabled = false;
        }

        //for color; try to enum the colors for each map i.e. map 0 is red, map 1 is blue ...
        firstColor = PickLineColor(currentMap);
        secondColor = PickLineColor(nextMap);

        tempMapLine.GetComponent<LineRenderer>().material = new Material(Shader.Find("Particles/Additive"));
        //tempMapLine.GetComponent<LineRenderer>().SetColors(firstColor, secondColor); old version, now outdated
        tempMapLine.GetComponent<LineRenderer>().startColor = firstColor;
        tempMapLine.GetComponent<LineRenderer>().endColor = secondColor;
    }

    //for picking map line colors
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
            return Color.gray;
        }
        return Color.white;
    }

    //for the world map; gets shift of x and y; used for setting map marker line pos
    Vector3 GetCurrentMapLocation(int pickMap)
    {
        return new Vector3(MapPos[pickMap].x * .075f * 150, MapPos[pickMap].y * .075f * 100, 0);
    }

    void DrawDoorsLocalMap(int curMap)
    {
        if (drawDoors == false)
        {
            var oldMapDoors = GameObject.FindGameObjectsWithTag("MapDoor");

            //this is for removing the old doors
            foreach (var door in oldMapDoors)
            {
                Destroy(door);
            }

            drawDoors = true;
            foreach (Vector3 door in doorLocations[curMap])
            {
                var doorTransform = Instantiate(doorPrefab);
                doorTransform.transform.SetParent(transform);
                doorTransform.position = player.transform.position + new Vector3(door.x * .175f, door.y *.175f, 0);
                doorTransform.localScale = new Vector3(.7f, .7f, .7f);
            }
        }
    }

    void PutObjectOnLocalMap(GameObject MapObject, List<Vector2> listOfMapObjects, Transform PrefabObject)
    {
        string newTag = "Map" + MapObject.tag;

        var oldMapObject = GameObject.FindGameObjectsWithTag(newTag);

        //this is for removing the old map objects
        foreach (var single in oldMapObject)
        {
            Destroy(single);
        }

        //adds objects to map
        drawDoors = true;
        foreach (Vector3 single in listOfMapObjects)
        {
            var objectTransform = Instantiate(PrefabObject);
            objectTransform.transform.SetParent(transform);
            objectTransform.position = player.transform.position + new Vector3(single.x * .175f, single.y * .175f, 0);
            objectTransform.localScale = new Vector3(.7f, .7f, .7f);
        }
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
        transform.localScale = new Vector3(.175f, .175f, .175f);
        transform.position = player.transform.position;
        transform.eulerAngles = new Vector3(270, 0, 0);
        //transform.Rotate(Vector3.zero);

        //for mapdoors
        if (touchingDoor == true && firstRun != false)
        {
            DrawDoorsLocalMap(nextMap);
        }
        else
        {
            firstRun = true;
            DrawDoorsLocalMap(currentMap);
        }
    }

    void UpdatePosition()
    {
        if (localMapOn)
        {
            transform.position = player.transform.position;
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

            //Debug.Log("combine.Length = " + combine.Length);

            //put all mesh filters into the combiner object
            for (int x = 0; x < totalMaps; x++)
            {
                combine[x].mesh = meshFilters[x].sharedMesh;
                //draw maps in a polygon shape based on how many there are 
                //numbers are map dimentions Ex: 150 in x and 100 in y;
                combine[x].transform = Matrix4x4.TRS(new Vector3(Mathf.Cos(2 * Mathf.PI * x / totalMaps) * 150, 0, Mathf.Sin(2 * Mathf.PI * x / totalMaps) * 100), Quaternion.identity, new Vector3(1, 1, 1));
            }
            //65k max vertices or won't work
            playerWorldMap.mesh.CombineMeshes(combine);

            //set scale
            transform.localScale = new Vector3(.075f, .075f, .075f);
            transform.position = player.transform.position;

            //this remove all objects in playmap; problem beacuse it was removing mapdoors. only want to remove maps
            /*
            //removes all the children of map object
            foreach (Transform child in transform)
            {
                //currently does nothngn beacuse map marker is not a child
                if (child.name != "MapMarker")
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
            */

            //save fullmap to assests
            string worldMap = "WorldMap";
            var savePath = "Assets/CurrentMaps/" + worldMap + ".asset";
            //Debug.Log("Saved Mesh to:" + savePath);
            AssetDatabase.CreateAsset(playerWorldMap.mesh, savePath);
            
        }
        else
        {
            //load map in
            string worldMap = "WorldMap";
            var loadPath = "Assets/CurrentMaps/" + worldMap + ".asset";
            Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath(loadPath, typeof(Mesh));
            transform.localScale = new Vector3(.075f, .075f, .075f);
            transform.position = player.transform.position;
            playerWorldMap.mesh = mesh;
        }
    }
   
}
