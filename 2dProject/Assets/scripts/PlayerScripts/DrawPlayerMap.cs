using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class DrawPlayerMap : MonoBehaviour {

    public MeshFilter playerLocalMap;
    public GameObject playerWorldMap;

    //for drawing doors on map
    public Transform doorPrefab;

    public Transform tempMap;
    //private Vector3 offset;

    //map bools
    private bool localMapOn = false;
    private bool worldMapOn = false;

    //if change room bool
    //public bool touchingDoor { get; set; }

    //for map marker
    GameObject MapMarkerTeemo;
    SpriteRenderer MapMarkerTeemoSprite;
    Transform MapMarkerTeemoPos;
    public List<Vector3> MapPos;
    public List<Vector3> MapPosWorldMaps;
    private int mapSeed;


    private int totalMaps;

    //for drawing door connections
    //private List<List<Vector2>> doorLocations;
    public Transform mapLine;
    public string currentMap { get; set; }
    public string nextMap { get; set; }
    public int currentDoor { get; set; }
    public int nextDoor { get; set; }
    //private List<Vector3> LinePos;

    //for line color
    Color firstColor;
    Color secondColor;

    //for drawing doors
    private bool drawDoors = false;

    //for door map
    private bool firstRun = false;

    //for leading lines
    private string currentMapSet = "";
    private int currentMapSetIndex = 0;

    //for keeping worldmap locked on player
    GameObject currentWorldMap;

    public static DrawPlayerMap DrawPlayerMapSingle;

    void Awake()
    {
        if (DrawPlayerMapSingle == null)
        {
            DrawPlayerMapSingle = this;
        }
        else if (DrawPlayerMapSingle != this)
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        //for drawing map
        totalMaps = GameData.GameDataSingle.mapSeed.Count;

        //map opacity
        transform.GetComponent<MeshRenderer>().material.color = new Vector4(1, 1, 1, .5f);

        // this is for teemo marker and lines
        MapPos = new List<Vector3>();
        for (int x = 0; x < totalMaps; x++)
        {
            MapPos.Add(new Vector2(Mathf.Cos(2 * Mathf.PI * x / totalMaps), Mathf.Sin(2 * Mathf.PI * x / totalMaps)));
        }

        //for correct position on small world maps
        MapPosWorldMaps = new List<Vector3>();
        foreach (Vector2 mapSet in GameData.GameDataSingle.mapSets)
        {
            for (int x = 0; x < (int)mapSet.x; x++)
            {
                MapPosWorldMaps.Add(new Vector2(Mathf.Cos(2 * Mathf.PI * x / (int)mapSet.x), Mathf.Sin(2 * Mathf.PI * x / (int)mapSet.x)));
            }
        }

        //for teemo mapmarker
        MapMarkerTeemo = GameObject.Find("MapMarker");
        MapMarkerTeemoSprite = MapMarkerTeemo.GetComponent<SpriteRenderer>();
        MapMarkerTeemoPos = MapMarkerTeemo.GetComponent<Transform>();


        //for line between doors map marker
        //LinePos = MapGenerator.MapGeneratorSingle.LinePos;

        //for making world map
        //DrawWorldMap();
        //removes all the children of map object
        foreach (Transform child in transform)
        {
            //currently does nothngn beacuse map marker is not a child
            if (child.name != "MapMarker")
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update () {

        // local map
        if (Input.GetKeyDown(KeyCode.M))
        {
            //turn off worldmap if on
            if (worldMapOn)
            {
                foreach (Transform child in transform.parent.Find("WorldMaps"))
                {
                    child.gameObject.SetActive(false);
                }
            }
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

        if (localMapOn)
        {
            //keep map locked on character
            MapMarkerTeemoPos.position = PlayerController.PlayerControllerSingle.transform.position + (PlayerController.PlayerControllerSingle.transform.position * .175f);
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

                //turn off lines
                foreach (Transform child in GameObject.Find("MapDoorLines").transform)
                {
                    child.GetComponent<LineRenderer>().enabled = false;
                }

                //turn off
                if (transform.parent.Find("WorldMaps").GetComponent<Transform>())
                {
                    currentWorldMap.SetActive(false);
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

                LoadCorrectDoorLines(currentMap);
                //TURN ON TEEMO MARKER
                MapMarkerTeemoSprite.enabled = true;
                //turn map on and draw it
                worldMapOn = true;
                playerLocalMap.mesh = null;
                localMapOn = false;
                LoadWorldMap();
            }
        }
        if (worldMapOn)
        {
            //keep map locked on character
            if (currentMapSetIndex < GameData.GameDataSingle.mapSets.Count)
            {
                MapMarkerTeemoPos.position = PlayerController.PlayerControllerSingle.transform.position + GetCurrentMapLocation(currentMap) + (PlayerController.PlayerControllerSingle.transform.position * .075f);
            }
            else
            {
                MapMarkerTeemoPos.position = PlayerController.PlayerControllerSingle.transform.position + (PlayerController.PlayerControllerSingle.transform.position * .075f);
            }
            //update door lines pos
            int x = 0;
            foreach (Transform child in GameObject.Find("MapDoorLines").transform)
            {
                //can ig et the current line 0 position? i want that to currrent plus player pos
                child.GetComponent<LineRenderer>().SetPosition(0, PlayerController.PlayerControllerSingle.transform.position + MapGenerator.MapGeneratorSingle.LinePos[x * 2]);
                child.GetComponent<LineRenderer>().SetPosition(1, PlayerController.PlayerControllerSingle.transform.position + MapGenerator.MapGeneratorSingle.LinePos[x * 2 + 1]);
                x++;
            }

            //update map pos
            UpdatePosition();
        }
    }

    public void UpdateMap()
    {
        if (localMapOn)
        {
            drawDoors = false;
            DrawLocalMap();

        }
        else if (worldMapOn)
        {
            //for door maps
            transform.localScale = new Vector3(.175f, .175f, .175f);
            transform.position = PlayerController.PlayerControllerSingle.transform.position;
            drawDoors = false;
            DrawDoorsLocalMap(nextMap);
            var oldMapDoors = GameObject.FindGameObjectsWithTag("MapDoor");
            foreach (var door in oldMapDoors)
            {
                door.GetComponent<SpriteRenderer>().enabled = false;
            }
            transform.localScale = new Vector3(.075f, .075f, .075f);
            //for door lines
            LoadCorrectDoorLines(nextMap);
            LoadWorldMap();
        }
        else if (!localMapOn && !worldMapOn)
        {

            //for door maps
            transform.localScale = new Vector3(.175f, .175f, .175f);
            transform.position = PlayerController.PlayerControllerSingle.transform.position;
            drawDoors = false;
            DrawDoorsLocalMap(nextMap);
            var oldMapDoors = GameObject.FindGameObjectsWithTag("MapDoor");
            foreach (var door in oldMapDoors)
            {
                door.GetComponent<SpriteRenderer>().enabled = false;
            }
            transform.localScale = new Vector3(.075f, .075f, .075f);
        }

    }

    void CreateLines(string seed1, string seed2, int door1, int door2, int set1, int set2)
    {
        Vector3 linePos1;
        Vector3 linePos2;
        // check if already a line is there.
        if (set1 == set2)
        {
            foreach (Transform child in GameObject.Find("MapDoorLines").transform)
            {
                if (child.name == (seed1.ToString() + door1.ToString() + seed2.ToString() + door2.ToString() + "," + set1 + "," + set2)
                    || child.name == (seed2.ToString() + door2.ToString() + seed1.ToString() + door1.ToString() + "," + set2 + "," + set1))
                {
                    return;
                }
            }
            
            linePos1 = GetCurrentMapLocation(seed1) + new Vector3(MapGenerator.MapGeneratorSingle.MapInfo[seed1].doorLocationsX[door1] * .075f, MapGenerator.MapGeneratorSingle.MapInfo[seed1].doorLocationsY[door1] * .075f, 0);
            linePos2 = GetCurrentMapLocation(seed2) + new Vector3(MapGenerator.MapGeneratorSingle.MapInfo[seed2].doorLocationsX[door2] * .075f, MapGenerator.MapGeneratorSingle.MapInfo[seed2].doorLocationsY[door2] * .075f, 0);

        }
        else
        {
            linePos1 = Vector3.zero;
            linePos2 = Vector3.zero;
        }
        //save line position


        //save pos for moving with player
        MapGenerator.MapGeneratorSingle.LinePos.Add(linePos1);
        MapGenerator.MapGeneratorSingle.LinePos.Add(linePos2);

        //Instantiate line
        var tempMapLine = Instantiate(mapLine) as Transform;
        tempMapLine.SetParent(GameObject.Find("MapDoorLines").transform);
        //tempMapLine.GetComponent<LineRenderer>().SetWidth(.1f, .1f); old version now outdated
        tempMapLine.GetComponent<LineRenderer>().startWidth = .1f;
        tempMapLine.GetComponent<LineRenderer>().endWidth = .1f;
        tempMapLine.GetComponent<LineRenderer>().SetPosition(0, PlayerController.PlayerControllerSingle.transform.position + linePos1);
        tempMapLine.GetComponent<LineRenderer>().SetPosition(1, PlayerController.PlayerControllerSingle.transform.position + linePos2);
        tempMapLine.name = seed1.ToString() + door1.ToString() + seed2.ToString() + door2.ToString() + "," + set1 + "," + set2;

        if (!worldMapOn)
        {
            tempMapLine.GetComponent<LineRenderer>().enabled = false;
        }

        //for color; try to enum the colors for each map i.e. map 0 is red, map 1 is blue ...
        firstColor = PickLineColor(GameData.GameDataSingle.FindMapIndex(seed1));
        secondColor = PickLineColor(GameData.GameDataSingle.FindMapIndex(seed2));

        //load material for line
        tempMapLine.GetComponent<LineRenderer>().material = Resources.Load("LineMat", typeof(Material)) as Material;
        tempMapLine.GetComponent<LineRenderer>().startColor = firstColor;
        tempMapLine.GetComponent<LineRenderer>().endColor = secondColor;
    }

    void CreateLinesNotInSet(string seed1, string seed2, int door1, int door2, int set1, int set2)
    {
        Vector3 linePos1;
        Vector3 linePos2;
        if(set1 < GameData.GameDataSingle.mapSets.Count)
        {
            linePos1 = GetCurrentMapLocation(seed1) + new Vector3(MapGenerator.MapGeneratorSingle.MapInfo[seed1].doorLocationsX[door1] * .075f, MapGenerator.MapGeneratorSingle.MapInfo[seed1].doorLocationsY[door1] * .075f, 0);
            linePos2 = GetCurrentMapLocation(seed1)*2;
        }
        else
        {
            linePos1 = new Vector3(MapGenerator.MapGeneratorSingle.MapInfo[seed1].doorLocationsX[door1] * .075f, MapGenerator.MapGeneratorSingle.MapInfo[seed1].doorLocationsY[door1] * .075f, 0);
            linePos2 = new Vector3(0, -10f, 0);
        }

        //save line position


        //save pos for moving with player
        MapGenerator.MapGeneratorSingle.LinePos.Add(linePos1);
        MapGenerator.MapGeneratorSingle.LinePos.Add(linePos2);

        //Instantiate line
        var tempMapLine = Instantiate(mapLine) as Transform;
        tempMapLine.SetParent(GameObject.Find("MapDoorLines").transform);
        //tempMapLine.GetComponent<LineRenderer>().SetWidth(.1f, .1f); old version now outdated
        tempMapLine.GetComponent<LineRenderer>().startWidth = .1f;
        tempMapLine.GetComponent<LineRenderer>().endWidth = .1f;
        tempMapLine.GetComponent<LineRenderer>().SetPosition(0, PlayerController.PlayerControllerSingle.transform.position + linePos1);
        tempMapLine.GetComponent<LineRenderer>().SetPosition(1, PlayerController.PlayerControllerSingle.transform.position + linePos2);
        tempMapLine.name = seed1.ToString() + door1.ToString() + seed2.ToString() + door2.ToString() + "," + set1 + "," + set2;

        if (!worldMapOn)
        {
            tempMapLine.GetComponent<LineRenderer>().enabled = false;
        }

        //for color; try to enum the colors for each map i.e. map 0 is red, map 1 is blue ...
        firstColor = PickLineColor(GameData.GameDataSingle.FindMapIndex(seed1));
        secondColor = PickLineColor(GameData.GameDataSingle.FindMapIndex(seed2));

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
    Vector3 GetCurrentMapLocation(string pickMap)
    {
        return new Vector3(MapGenerator.MapGeneratorSingle.MapPosWorldMaps[GameData.GameDataSingle.FindMapIndex(pickMap)].x * .075f * 150, MapGenerator.MapGeneratorSingle.MapPosWorldMaps[GameData.GameDataSingle.FindMapIndex(pickMap)].y * .075f * 100, 0);
    }

    void DrawDoorsLocalMap(string curMap)
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
            //foreach (Vector3 door in mapGen.MapInfo[curMap].doorLocations)
            for(int tempCounter = 0; tempCounter < MapGenerator.MapGeneratorSingle.MapInfo[curMap].doorLocationsX.Count; tempCounter++)
            {
                var doorTransform = Instantiate(doorPrefab);
                doorTransform.transform.SetParent(transform);
                doorTransform.position = PlayerController.PlayerControllerSingle.transform.position + new Vector3(MapGenerator.MapGeneratorSingle.MapInfo[curMap].doorLocationsX[tempCounter] * .175f,
                                                                                 MapGenerator.MapGeneratorSingle.MapInfo[curMap].doorLocationsY[tempCounter] * .175f, 0);
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
            objectTransform.position = PlayerController.PlayerControllerSingle.transform.position + new Vector3(single.x * .175f, single.y * .175f, 0);
            objectTransform.localScale = new Vector3(.7f, .7f, .7f);
        }
    }

    void DrawLocalMap()
    {
        //MapGenerator mapGen = FindObjectOfType<MapGenerator>();

        //use this to get all maps

        string mapSeed = MapGenerator.MapGeneratorSingle.seed;

        //load map in
        //var loadPath = "Assets/CurrentMaps/" + mapSeed + ".asset";
        //Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath(loadPath, typeof(Mesh));
        playerLocalMap.mesh = MapGenerator.MapGeneratorSingle.GetComponentInChildren<MeshFilter>().mesh;

        //scale size down and set position
        transform.localScale = new Vector3(.175f, .175f, .175f);
        transform.position = PlayerController.PlayerControllerSingle.transform.position;
        transform.eulerAngles = new Vector3(270, 0, 0);

        //for mapdoors
        if (firstRun)
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
            transform.position = PlayerController.PlayerControllerSingle.transform.position;
        }
        else
        {
            currentWorldMap.transform.position = PlayerController.PlayerControllerSingle.transform.position;
        }
    }

    public void CreateWorldMap()
    {
        MeshFilter[] meshFilters = new MeshFilter[GameData.GameDataSingle.mapSeed.Count];

        //create prefabs for all maps then add them to a list of filters, then remove all prefabs later
        for (int x = 0; x < GameData.GameDataSingle.mapSeed.Count; x++)
        {
            string mapSeed = GameData.GameDataSingle.mapSeed[x];

            //get cave name
            string caveName = "Cave" + mapSeed;
            //Debug.Log("Cave + mapSeed = " + caveName);
            Mesh mesh = GameObject.Find(caveName).GetComponent<MeshFilter>().mesh;

            mesh.name = caveName;

            // create prefab
            var tempMapPrefab = Instantiate(tempMap) as Transform;
            tempMapPrefab.transform.SetParent(transform);
            tempMapPrefab.name = mapSeed;
            tempMapPrefab.GetComponent<MeshFilter>().mesh = mesh;
            meshFilters[x] = tempMapPrefab.GetComponent<MeshFilter>();
        }

        int currentSetMaps = 0;
        string worldMapName;
        foreach (Vector2 mapSet in GameData.GameDataSingle.mapSets)
        {

            //(int)num.y, (int)num.x + (int)num.y - 1;
            CombineInstance[] combineTest = new CombineInstance[(int)mapSet.x];

            for (int x = 0; x < (int)mapSet.x; x++)
            {
                // (int)mapSet.x so start on correct map
                combineTest[x].mesh = meshFilters[x + (int)mapSet.y].sharedMesh;
                //draw maps in a polygon shape based on how many there are 
                //numbers are map dimentions Ex: 150 in x and 100 in y;
                combineTest[x].transform = Matrix4x4.TRS(new Vector3(Mathf.Cos(2 * Mathf.PI * x / (int)mapSet.x) * 150, 0, Mathf.Sin(2 * Mathf.PI * x / (int)mapSet.x) * 100), Quaternion.identity, new Vector3(1, 1, 1));
            }

            //create a worldmap object for this set of maps
            GameObject tempWorldMap = Instantiate(playerWorldMap, transform.parent.Find("WorldMaps").transform);
            tempWorldMap.name = "WorldMap" + currentSetMaps;
            tempWorldMap.GetComponent<MeshFilter>().mesh.CombineMeshes(combineTest);

            //set scale
            tempWorldMap.transform.localScale = new Vector3(.075f, .075f, .075f);

            worldMapName = "WorldMap" + currentSetMaps;
            MapGenerator.MapGeneratorSingle.WorlMapDictionary.Add(worldMapName, tempWorldMap.transform);

            //turn off world map until needed
            tempWorldMap.SetActive(false);

            currentSetMaps += 1;
        }

        //Destroy(MapGenerator.MapGeneratorSingle.gameObject);
        CreateDoorConnections();
    }

    void LoadWorldMap()
    {
        string worldMap = "WorldMap";
        int currentSetMaps = 0;

        string mapSeed = MapGenerator.MapGeneratorSingle.seed;

        MapInformation newData = MapGenerator.MapGeneratorSingle.MapInfo[mapSeed];
        currentSetMaps = newData.mapSet;

        if (currentSetMaps < GameData.GameDataSingle.mapSets.Count)
        {
            worldMap = "WorldMap" + currentSetMaps;
            //Debug.Log("worldMap1 = " + worldMap);
        }
        else
        {
            worldMap = mapSeed;
            //Debug.Log("worldMap2 = " + worldMap);
        }

        //bad try at active world map
        foreach (Transform child in transform.parent.Find("WorldMaps"))
        {
            child.gameObject.SetActive(false);
        }

        //turn on correct world map
        currentWorldMap = MapGenerator.MapGeneratorSingle.WorlMapDictionary[worldMap].gameObject;
        currentWorldMap.SetActive(true);
    }

    void CreateDoorConnections()
    {
        //get reference of door dic
        Dictionary<string, string> tempDoorConnectionDictionary = new Dictionary<string, string>(GameData.GameDataSingle.doorConnectionDictionary);

        //for breaking door dic into maps and doors
        string[] values;
        string oldSeed;
        string oldDoor;
        string oldDicRef;

        string newSeed;
        string newDoor;
        string newDicRef;

        int oldCurrentSetMaps = 0;
        int newCurrentSetMaps = 0;
        foreach (KeyValuePair<string, string> entry in tempDoorConnectionDictionary)
        {
            //for first map and door
            values = entry.Value.Split(',');
            oldDicRef = entry.Value;
            oldSeed = values[0];
            oldDoor = values[1];

            values = entry.Key.Split(',');

            //check if it's a boss room
            if (oldSeed != "BossKey1")
            {
                MapInformation oldData = MapGenerator.MapGeneratorSingle.MapInfo[oldSeed];
                oldCurrentSetMaps = oldData.mapSet;
            
                values = tempDoorConnectionDictionary[oldDicRef].Split(',');
                newDicRef = tempDoorConnectionDictionary[oldDicRef];
                newSeed = values[0];
                newDoor = values[1];

                //Debug.Log("newDicRef = " + newDicRef + " newSeed = " + newSeed + " newDoor = " + newDoor);

                MapInformation newData = MapGenerator.MapGeneratorSingle.MapInfo[newSeed];
                newCurrentSetMaps = newData.mapSet;

                //need fix for the special add maps
                if (oldCurrentSetMaps < GameData.GameDataSingle.mapSets.Count && newCurrentSetMaps < GameData.GameDataSingle.mapSets.Count && oldCurrentSetMaps == newCurrentSetMaps)
                {
                    CreateLines(oldSeed, newSeed, Int32.Parse(oldDoor), Int32.Parse(newDoor), oldCurrentSetMaps, newCurrentSetMaps);
                }
                else
                {
                    CreateLinesNotInSet(oldSeed, newSeed, Int32.Parse(oldDoor), Int32.Parse(newDoor), oldCurrentSetMaps, newCurrentSetMaps);
                }
            }
            else
            {
                values = entry.Key.Split(',');
                newDicRef = entry.Value;
                newSeed = values[0];
                newDoor = values[1];

                MapInformation newData = MapGenerator.MapGeneratorSingle.MapInfo[newSeed];
                newCurrentSetMaps = newData.mapSet;
                //Debug.Log("oldDicRef = " + oldDicRef + " oldSeed = " + oldSeed + " oldDoor = " + oldDoor + " oldCurrentSetMaps = " + oldCurrentSetMaps);
                //Debug.Log("newDicRef = " + newDicRef + " newSeed = " + newSeed + " newDoor = " + newDoor + " newCurrentSetMaps = " + newCurrentSetMaps);

                //boss rooms load new seed first not old seed
                CreateLinesNotInSet(newSeed, oldSeed, Int32.Parse(newDoor), Int32.Parse(newDoor), newCurrentSetMaps, newCurrentSetMaps);
            }
        }
    }

    void LoadCorrectDoorLines(string map)
    {
        MapInformation oldData = MapGenerator.MapGeneratorSingle.MapInfo[map];
        currentMapSet = oldData.mapSet.ToString();
        currentMapSetIndex = oldData.mapSet;

        string[] values;
        string set1;
        //string set2;

        foreach (Transform child in GameObject.Find("MapDoorLines").transform)
        {
            values = child.name.Split(',');
            set1 = values[1];
            //set2 = values[2];
            if (set1 == currentMapSet)
            {
                child.GetComponent<LineRenderer>().enabled = true;
            }
            else
            {
                child.GetComponent<LineRenderer>().enabled = false;
            }
        }
    }
}
