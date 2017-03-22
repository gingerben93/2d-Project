using UnityEngine;
using System.Collections;
//generic is for using lists
using System.Collections.Generic;
using System;

public class MapGenerator : MonoBehaviour
{
    //map size
    //needs to be changed to private
    public int width;
    public int height;

    //fill seeds
    public string seed { get; set; }

    //fill range
    [Range(0, 100)]
    public int randomFillPercent;

    //size of passageway between rooms
    [Range(0, 4)]
    public int passageLength;

    //pick how smooth to make ground
    [Range(0, 10)]
    public int smoothness;

    int[,] map;
    int[,] borderedMap;

    //public int squareSize;

    public int numMaps { get; private set; }
    private int currentMap = 0;

    //
    public List<Vector2> possibleDoorLocations;
    public List<Vector2> doorLocations;
    public List<Vector2> enemyLocations;

    public List<string> bossRooms;

    // for storing game information
    GameData gameData;
    MeshGenerator meshGen;
    MapAddOns MapAddOns;

    public Dictionary<string, MapInformation> MapInfo = new Dictionary<string, MapInformation>();

    public static MapGenerator MapGeneratorSingle;

    //for spawing boss
    public Transform BossPrefab;

    void Awake()
    {
        if (MapGeneratorSingle == null)
        {
            DontDestroyOnLoad(gameObject);
            MapGeneratorSingle = this;
        }
        else if (MapGeneratorSingle != this)
        {
            Destroy(gameObject);
        }
    }

    //where program stars
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        meshGen = GetComponent<MeshGenerator>();
        MapAddOns = GetComponent<MapAddOns>();

        if (GameLoader.GameLoaderSingle == null)
        {
            numMaps = 6;
            int startMap = 0;
            int y = numMaps;

            List<Vector2> mapSets = new List<Vector2>();
            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);

            while (y > 0)
            {
                //make size 2 or 3
                int ranSizeSetMaps = UnityEngine.Random.Range(2, 4);
                if (y >= ranSizeSetMaps)
                {
                    //Debug.Log("ranSizeSetMaps = " + ranSizeSetMaps + "startMap = " + startMap);
                    mapSets.Add(new Vector2(ranSizeSetMaps, numMaps - y));
                    y -= ranSizeSetMaps;
                    startMap += y;
                }
                else
                {
                    //Debug.Log("new Vector2(mapSets[mapSets.Count - 1].x + x = " + (mapSets[mapSets.Count - 1].x + x)  + " mapSets[mapSets.Count - 1].y) = " + mapSets[mapSets.Count - 1].y);
                    mapSets[mapSets.Count - 1] = new Vector2(mapSets[mapSets.Count - 1].x + y, mapSets[mapSets.Count - 1].y);
                    break;
                }
            }

            gameData.mapSets = mapSets;

            int x = 0;
            for (x = 0; x < numMaps; x++)
            {
                seed = currentMap.ToString();
                GenerateMap();
                currentMap += 1;
            }

            width = 50; height = 100;
            randomFillPercent = 0;
            seed = currentMap.ToString();
            bossRooms.Add(seed);
            Debug.Log("seed boos room = " + seed);
            GameData.GameDataSingle.isBossRoomOpen.Add(seed, false);
            GenerateMap();
            currentMap += 1;

            foreach (Vector2 num in gameData.mapSets)
            {
                //use: start map, end map, num doors, map1 seed, map2 seed
                //Debug.Log("(int)num.y = " + num.y + " (int)num.x + (int)num.y - 1 = " + (num.x + num.y - 1));
                gameData.CreatDoorConnections((int)num.y, (int)num.x + (int)num.y - 1, 2);
                gameData.EnsureConnectivityOfMaps((int)num.y, (int)num.x + (int)num.y - 1);
                gameData.ConnectDoors();
            }

            foreach (Vector2 num in gameData.mapSets)
            {
                if (gameData.mapSets[gameData.mapSets.Count - 1] == num)
                {
                    break;
                }
                gameData.ConnectSetOfRooms((int)num.y, (int)num.x + (int)num.y);
            }

            gameData.CreatDoorConnections(6, 6, 0);
            gameData.ConnectSetOfRooms(1, 6);

            for (x = 0; x < numMaps; x++)
            {
                enemyLocations = new List<Vector2>();
                map = new int[MapInfo[gameData.mapSeed[x]].width, MapInfo[gameData.mapSeed[x]].height];
                enemyLocations = MapAddOns.SpawnEnemy(MapInfo[gameData.mapSeed[x]]);

                //for storing data in not unity vectors
                List<float> tempHolderX = new List<float>();
                List<float> tempHolderY = new List<float>();
                foreach (Vector2 XYCoord in enemyLocations)
                {
                    tempHolderX.Add(XYCoord.x);
                    tempHolderY.Add(XYCoord.y);
                }
                MapInfo[gameData.mapSeed[x]].enemyLocationsX = tempHolderX;
                MapInfo[gameData.mapSeed[x]].enemyLocationsY = tempHolderY;

                //Debug.Log("MapInfo[gameData.mapSeed[x]].doorLocationsX.Count = " + MapInfo[gameData.mapSeed[x]].doorLocationsX.Count + " x = " + x);

            }
            //make a new map
            //DrawPlayerMap.DrawPlayerMapSingle.makeMap = true;
        }
        else
        {
            MapInfo = GameLoader.GameLoaderSingle.playerDat.MapInfo;
            gameData.doorConnectionDictionary = GameLoader.GameLoaderSingle.playerDat.doorConnectionDictionary;
            gameData.mapSeed = GameLoader.GameLoaderSingle.playerDat.mapSeed;
            for (int tempCounter = 0; tempCounter < GameLoader.GameLoaderSingle.playerDat.mapSetsX.Count; tempCounter++)
            {
                gameData.mapSets.Add(new Vector2(GameLoader.GameLoaderSingle.playerDat.mapSetsX[tempCounter], GameLoader.GameLoaderSingle.playerDat.mapSetsY[tempCounter]));
            }
            PlayerStats.playerStatistics.health = GameLoader.GameLoaderSingle.playerDat.health;
            PlayerStats.playerStatistics.experiencePoints = GameLoader.GameLoaderSingle.playerDat.experiencePoints;

            //don't make a map
            //DrawPlayerMap.DrawPlayerMapSingle.makeMap = false;

            //Debug.Log("GameLoader.GameLoaderSingle.playerDat.MapInfo.Count = " + GameLoader.GameLoaderSingle.playerDat.MapInfo.Count);
            //Debug.Log("GameLoader.GameLoaderSingle.playerDat.mapSetsX.Count = " + GameLoader.GameLoaderSingle.playerDat.mapSetsX.Count);
            //Debug.Log("gameData.mapSeed.Count = " + gameData.mapSeed.Count);
            //Debug.Log("GameLoader.GameLoaderSingle.playerDat.doorConnectionDictionary.Count = " + GameLoader.GameLoaderSingle.playerDat.doorConnectionDictionary.Count);
        }
        PlayerStats.playerStatistics.MapInfo = MapInfo;


        seed = gameData.mapSeed[0];
        LoadMap();
        DrawPlayerMap.DrawPlayerMapSingle.currentMap = seed;
        GameController.GameControllerSingle.respawnLocation = new Vector3(PlayerStats.playerStatistics.MapInfo[seed].doorLocationsX[0],
                                                                          PlayerStats.playerStatistics.MapInfo[seed].doorLocationsY[0],0);
        LoadOnClick.LoadOnClickSingle.LoadScene(0);
    }

    void OnEnable()
    {
        //doesn't go on first load
        if (seed != null)
        {
            StartCoroutine(WaitForGameToLoad());
        }
    }

    IEnumerator WaitForGameToLoad()
    {
        yield return new WaitForSeconds(1);
        LoadMap();
    }

    //begining of map generation
    public void GenerateMap()
    {
        int squareSize = 1;

        //for getting map set number
        int currentSetMap = 0;
        foreach (Vector2 mapSet in gameData.mapSets)
        {
            if (currentMap >= (int)mapSet.y && currentMap < (int)mapSet.y + (int)mapSet.x)
            {
                break;
            }
            currentSetMap += 1;
        }

        map = new int[width, height];
        //save game data for recall later
        gameData.AddSeed(seed);
        //save map data
        MapInformation TempData = new MapInformation();
        TempData.index = currentMap;
        TempData.mapSet = currentSetMap;
        TempData.width = width;
        TempData.height = height;
        TempData.randomFillPercent = randomFillPercent;
        TempData.passageLength = passageLength;
        TempData.smoothness = smoothness;
        TempData.squareSize = squareSize;
        MapInfo.Add(seed, TempData);

        //fills map
        RandomFillMap();

        // smooths the map
        for (int i = 0; i < smoothness; i++)
        {
            SmoothMap();
        }
        //adds the mesh and other parts
        ProcessMap();
        MapInfo[seed].map = map;

        int borderSize = 5;
        borderedMap = new int[width + borderSize * 2, height + borderSize * 2];

        for (int x = 0; x < borderedMap.GetLength(0); x++)
        {
            for (int y = 0; y < borderedMap.GetLength(1); y++)
            {
                if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize)
                {
                    borderedMap[x, y] = map[x - borderSize, y - borderSize];
                }
                else
                {
                    borderedMap[x, y] = 1;
                }
            }
        }
        MapInfo[seed].borderedMap = borderedMap;
 
        meshGen.GenerateMesh(borderedMap, squareSize);

        //for doors spawns
        possibleDoorLocations = new List<Vector2>();
        possibleDoorLocations = MapAddOns.GenerateDoors(map);

        List<float> tempHolderX = new List<float>();
        List<float> tempHolderY = new List<float>();
        foreach (Vector2 XYCoord in possibleDoorLocations)
        {
            tempHolderX.Add(XYCoord.x);
            tempHolderY.Add(XYCoord.y);
        }
        MapInfo[seed].possibleDoorLocationsX = tempHolderX;
        MapInfo[seed].possibleDoorLocationsY = tempHolderY;
    }

    public void LoadMap()
    {
        //Debug.Log("seed = " + seed);
        MapInformation currentData = MapInfo[seed];
        borderedMap = currentData.borderedMap;

        //find a way to save the map data so oyu don't even have to recreate the borderedMap (and doorLocations); this can jsut go right at the top.
        meshGen.LoadMeshFromAssests(borderedMap, currentData.squareSize);

        //for door spawns
        doorLocations = new List<Vector2>();

        for(int tempCounter = 0; tempCounter < MapInfo[seed].doorLocationsX.Count; tempCounter++)
        {
            doorLocations.Add(new Vector2(MapInfo[seed].doorLocationsX[tempCounter], MapInfo[seed].doorLocationsY[tempCounter]));
        }
        //doorLocations = MapInfo[seed].doorLocations;
        MapAddOns.DrawOldDoors(doorLocations);

        //for enemy spawns
        enemyLocations = new List<Vector2>();
        //foreach (float tempXY in MapInfo[seed].enemyLocationsX)
        if (MapInfo[seed].enemyLocationsX != null)
        {
            for (int tempCounter = 0; tempCounter < MapInfo[seed].enemyLocationsX.Count; tempCounter++)
            {
                enemyLocations.Add(new Vector2(MapInfo[seed].enemyLocationsX[tempCounter], (MapInfo[seed].enemyLocationsY[tempCounter])));
            }

            if (enemyLocations != null && enemyLocations.Count > 0)
            {
                MapAddOns.DrawEnemys(enemyLocations);
            }
        }
        else if (bossRooms.Contains(seed))
        {
            MapAddOns.RemoveAllEnemies();
            var temp = Instantiate(BossPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            temp.transform.SetParent(GameObject.Find("EnemyList").transform);
        }
        else
        {
            MapAddOns.RemoveAllEnemies();
        }
    }

    //smooths the map with arbitrary process, change be changed and modified 
    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTitles = GetSurroundingWallCount(x, y);

                if (neighbourWallTitles > 4)
                {
                    map[x, y] = 1;

                }
                else if (neighbourWallTitles < 4)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    // gets counts of walls from 9 tiles, all the tiles surrounding current tile.
    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (IsInMapRange(neighbourX, neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    //checks if given x y is in the map range
    bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    //for random filling of map with border
    void RandomFillMap()
    {
        //get the hash of the seed to fill array, same seed will give same array
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //make solid border
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                //random fill
                else
                {
                    if (pseudoRandom.Next(0, 100) < randomFillPercent)
                    {
                        map[x, y] = 1;
                    }
                    else
                    {
                        map[x, y] = 0;
                    }
                }
            }
        }
    }

    void ProcessMap()
    {
        // 1 represents the wall
        List<List<Coord>> wallRegions = GetRegions(1);

        // remove wall if not larger than xxx
        int wallThresholdSize = 50;

        foreach (List<Coord> wallRegion in wallRegions)
        {
            if (wallRegion.Count < wallThresholdSize)
            {
                foreach (Coord tile in wallRegion)
                {
                    map[tile.tileX, tile.tileY] = 0;
                }
            }

        }

        // 0 represents the room
        List<List<Coord>> roomRegions = GetRegions(0);

        // remove room if not larger than xxx
        int roomThresholdSize = 50;

        //list of all rooms that are left after removing
        List<Room> survivingRooms = new List<Room>();

        foreach (List<Coord> roomRegion in roomRegions)
        {
            if (roomRegion.Count < roomThresholdSize)
            {
                foreach (Coord tile in roomRegion)
                {
                    map[tile.tileX, tile.tileY] = 1;
                }
            }
            else
            {
                survivingRooms.Add(new Room(roomRegion, map));
            }
        }

        if (survivingRooms.Count > 0)
        {


            survivingRooms.Sort();
            survivingRooms[0].isMainRoom = true;
            survivingRooms[0].isAccessibleFromMainRoom = true;

            if (passageLength > 0)
            {
                ConnectClosestRooms(survivingRooms);
            }
        }
    }

    //for region detection
    struct Coord
    {
        public int tileX;
        public int tileY;
        // public int positionX;
        // public int positionY;
        // public int positionZ;


        //contructor
        public Coord(int x, int y)
        {
            tileX = x;
            tileY = y;
        }
        /*
        public CoordWithPos(int x, int y, int posX, int posY, int posZ)
        {
            tileX = x;
            tileY = y;
            positionX = posX;
            positionY = posY;
            positionZ = posZ;
        }
        */

    }

    //method takes tile type and return all regions with that type of tile
    List<List<Coord>> GetRegions(int tileType)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        //2d array for checking if titles has been seen before
        int[,] mapFlags = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);
                    foreach (Coord tile in newRegion)
                    {
                        mapFlags[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }
        return regions;
    }

    //for region detection, flood fill algorithm to find rooms
    List<Coord> GetRegionTiles(int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();
        //2d array for checking if titles has been seen before
        int[,] mapFlags = new int[width, height];
        //for type of tile
        int tileType = map[startX, startY];

        //queue for checking if element was visted before
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        //
        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);
            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    if (IsInMapRange(x, y) && (x == tile.tileX || y == tile.tileY))
                    {
                        if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                        {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }
        return tiles;
    }

    //for connecting rooms
    class Room : IComparable<Room>
    {
        public List<Coord> tiles;
        public List<Coord> edgeTiles;
        public List<Room> connectedRooms;
        public int roomSize;
        public bool isAccessibleFromMainRoom;
        public bool isMainRoom;

        //if empty room is wanted
        public Room()
        {

        }

        public Room(List<Coord> roomTiles, int[,] map)
        {
            tiles = roomTiles;
            roomSize = tiles.Count;
            connectedRooms = new List<Room>();

            edgeTiles = new List<Coord>();
            foreach (Coord tile in tiles)
            {
                //check if tiles neighbours is a wall tile, if so is on edge of room
                for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
                {
                    for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                    {
                        // exclude diagnol tiles
                        if (x == tile.tileX || y == tile.tileY)
                        {
                            if (map[x, y] == 1)
                            {
                                edgeTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }

        public void SetAccessibleFromMainRoom()
        {
            if (!isAccessibleFromMainRoom)
            {
                isAccessibleFromMainRoom = true;
                foreach (Room connectedRoom in connectedRooms)
                {
                    connectedRoom.SetAccessibleFromMainRoom();
                }
            }
        }

        public static void ConnectRooms(Room roomA, Room roomB)
        {
            if (roomA.isAccessibleFromMainRoom)
            {
                roomB.SetAccessibleFromMainRoom();
            }
            else if (roomB.isAccessibleFromMainRoom)
            {
                roomA.SetAccessibleFromMainRoom();
            }
            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }

        public bool IsConnected(Room otherRoom)
        {

            return connectedRooms.Contains(otherRoom);
        }

        //use with IComparable
        public int CompareTo(Room otherRoom)
        {
            return otherRoom.roomSize.CompareTo(roomSize);
        }

    }

    void ConnectClosestRooms(List<Room> allRooms, bool forceAccessiblilityFromMainRoom = false)
    {
        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if (forceAccessiblilityFromMainRoom)
        {
            foreach (Room room in allRooms)
            {
                if (room.isAccessibleFromMainRoom)
                {
                    roomListB.Add(room);
                }
                else
                {
                    roomListA.Add(room);
                }
            }
        }
        else
        {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        int bestDistance = 0;
        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConnectionFound = false;


        foreach (Room roomA in roomListA)
        {
            if (!forceAccessiblilityFromMainRoom)
            {
                possibleConnectionFound = false;
                if (roomA.connectedRooms.Count > 0)
                {
                    continue;
                }
            }

            foreach (Room roomB in roomListB)
            {
                //optimizing
                if (roomA == roomB || roomA.IsConnected(roomB))
                {
                    continue;
                }
                for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++)
                {
                    for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++)
                    {
                        Coord tileA = roomA.edgeTiles[tileIndexA];
                        Coord tileB = roomB.edgeTiles[tileIndexB];
                        int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY - tileB.tileY, 2));

                        //best connection was found
                        if (distanceBetweenRooms < bestDistance || !possibleConnectionFound)
                        {
                            bestDistance = distanceBetweenRooms;
                            possibleConnectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }

                    }
                }
            }
            //when you just want rooms to be connect to atleat one other room
            if (possibleConnectionFound && !forceAccessiblilityFromMainRoom)
            {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }
        }
        //when you want all rooms connected
        if (possibleConnectionFound && forceAccessiblilityFromMainRoom)
        {
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            ConnectClosestRooms(allRooms, true);
        }
        if (!forceAccessiblilityFromMainRoom)
        {
            ConnectClosestRooms(allRooms, true);
        }
    }

    void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB)
    {
        Room.ConnectRooms(roomA, roomB);
        //Debug.DrawLine(CoordToWorldPoint(tileA), CoordToWorldPoint(tileB), Color.green, 100);

        List<Coord> line = GetLine(tileA, tileB);
        foreach (Coord c in line)
        {
            DrawCircle(c, passageLength);
        }
    }

    List<Coord> GetLine(Coord from, Coord to)
    {
        List<Coord> line = new List<Coord>();
        int x = from.tileX;
        int y = from.tileY;

        int dx = to.tileX - x;
        int dy = to.tileY - y;

        bool inverted = false;
        int step = Math.Sign(dx);
        int gradientStep = Math.Sign(dy);

        int longest = Math.Abs(dx);
        int shortest = Math.Abs(dy);

        if (longest < shortest)
        {
            inverted = true;
            longest = Math.Abs(dy);
            shortest = Math.Abs(dx);

            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }

        int gradientAccumulation = longest / 2;
        for (int i = 0; i < longest; i++)
        {
            line.Add(new Coord(x, y));
            if (inverted)
            {
                y += step;
            }
            else
            {
                x += step;
            }
            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest)
            {
                if (inverted)
                {
                    x += gradientStep;
                }
                else
                {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }
        return line;
    }

    void DrawCircle(Coord c, int r)
    {
        for (int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                if (x * x + y * y <= r * r)
                {
                    int realX = c.tileX + x;
                    int realY = c.tileY + y;
                    if (IsInMapRange(realX, realY))
                    {
                        map[realX, realY] = 0;
                    }
                }
            }
        }
    }


    //this is used for visual debugging, doesn't create anything real.
    /*
    void OnDrawGizmos()
    {
        if(map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (map[x,y] == 1)
                    {
                       Gizmos.color = Color.black;
                    }
                    else
                    {
                        Gizmos.color = Color.white;
                    }
                    Vector2 pos = new Vector2(-width / 2 + x + .5f, -height / 2 + y + .5f);
                    Gizmos.DrawCube(pos, Vector2.one);
                }
            }
        }
    }
    */
}
