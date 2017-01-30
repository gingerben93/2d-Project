using UnityEngine;
using System.Collections;
//generic is for using lists
using System.Collections.Generic;
using System;

public class MapGenerator : MonoBehaviour
{
    //map size
    //needs to be changed to  public int width {get; set;}
    public int width;
    public int height;

    //fill seeds
    public string seed { get; set; }
    public bool useRandomSeed;

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

    public int numMaps { get; private set; }
    int currentMap = 0;

    //
    public List<Vector2> possibleDoorLocations;
    public List<Vector2> doorLocations;
    public List<Vector2> enemyLocations;

    // for storing game information
    GameData gameData;
    MeshGenerator meshGen;
    MapAddOns MapAddOns;

    //where program stars
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        numMaps = 6;
        for (int x = 0; x < numMaps; x++)
        {
            seed = currentMap.ToString();
            GenerateMap();
            currentMap += 1;
        }

        width = 50; height = 100;
        randomFillPercent = 0;
        seed = currentMap.ToString();
        GenerateMap();
        currentMap += 1;



        useRandomSeed = false;

        //use is map index one, map index 2 and number of doors for each map.
        gameData.CreatDoorConnections(0, 2, 2);
        gameData.EnsureConnectivityOfMaps(0, 2);
        gameData.ConnectDoors(0);

        Debug.Log("set one done");

        gameData.CreatDoorConnections(3, 5 , 2);
        gameData.EnsureConnectivityOfMaps(3, 5);
        gameData.ConnectDoors(3);

        Debug.Log("set two done");

        gameData.ConnectSetOfRooms(0, 3);

        gameData.CreatDoorConnections(6, 6, 0);
        gameData.ConnectSetOfRooms(0, 6);

        //gameData = FindObjectOfType<GameData>();
        seed = gameData.mapSeed[0];

        map = new int[gameData.MapWidthHeight[Int32.Parse(seed)].GetLength(0), gameData.MapWidthHeight[Int32.Parse(seed)].GetLength(1)];
        GenerateMap();


    }

    //begining of map generation
    public void GenerateMap()
    {
        gameData = FindObjectOfType<GameData>();

        if (useRandomSeed == true)
        {
            //save game data for recall later
            gameData.AddSeed(seed);
            gameData.MapFillPassLengthAndSmoothness.Add(new Vector3(randomFillPercent, passageLength, smoothness));
            map = new int[width, height];
            gameData.MapWidthHeight.Add(map);
        }
        else
        {
            int tempIntSeed = Int32.Parse(seed);
            Debug.Log("gameData.MapWidthHeight[Int32.Parse(seed)].GetLength(1) = " + gameData.MapWidthHeight[tempIntSeed].GetLength(1));
            map = new int[gameData.MapWidthHeight[tempIntSeed].GetLength(0), gameData.MapWidthHeight[tempIntSeed].GetLength(1)];
            width = gameData.MapWidthHeight[tempIntSeed].GetLength(0);
            height = gameData.MapWidthHeight[tempIntSeed].GetLength(1);
            randomFillPercent = (int)gameData.MapFillPassLengthAndSmoothness[tempIntSeed].x;
            passageLength = (int)gameData.MapFillPassLengthAndSmoothness[tempIntSeed].y;
            smoothness = (int)gameData.MapFillPassLengthAndSmoothness[tempIntSeed].z;
        }

        //fills map
        RandomFillMap();

        // smooths the map
        for (int i = 0; i < smoothness; i++)
        {
            SmoothMap();
        }

        //adds the mesh and other parts
        ProcessMap();

        int borderSize = 5;
        int[,] borderedMap = new int[width + borderSize * 2, height + borderSize * 2];

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

        meshGen = GetComponent<MeshGenerator>();

        gameData = FindObjectOfType<GameData>();
        MapAddOns = GetComponent<MapAddOns>();

        if (useRandomSeed == true)
        {
            //for mesh
            meshGen.GenerateMesh(borderedMap, 1);

            //for doors spawns
            possibleDoorLocations = new List<Vector2>();
            possibleDoorLocations = MapAddOns.GenerateDoors(map, 1, borderSize);
            gameData.AddDoorLocations(possibleDoorLocations);

            //for enemy spawns
            enemyLocations = new List<Vector2>();
            enemyLocations = MapAddOns.SpawnEnemy(map, 1, borderSize);
            gameData.AddEnemyLocations(enemyLocations);

        }
        else
        {
            //find a way to save the map data so oyu don't even have to recreate the borderedMap (and doorLocations); this can jsut go right at the top.
            meshGen.LoadMeshFromAssests(borderedMap, 1);

            //for door spawns
            doorLocations = new List<Vector2>();
            Debug.Log("seed =" + seed);
            doorLocations = gameData.GetDoorForMap(gameData.FindMapIndex(seed));
            MapAddOns.DrawOldDoors(doorLocations);

            //for enemy spawns
            enemyLocations = new List<Vector2>();
            enemyLocations = gameData.GetEnemyForMap(gameData.FindMapIndex(seed));
            MapAddOns.DrawEnemys(enemyLocations);

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
