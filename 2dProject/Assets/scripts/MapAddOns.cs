using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MapAddOns : MonoBehaviour
{
    public Transform doorPrefab;

    public List<Vector2> TestEnemyWalkPath;

    public List<Vector2> GenerateDoors(int[,] map, float squareSize, int borderSize)
    {
        //for size of map
        int nodeCountX = map.GetLength(0);
        int nodeCountY = map.GetLength(1);
        float mapWidth = nodeCountX * squareSize;
        float mapHeight = nodeCountY * squareSize;

        //for pos doors
        List<Vector2> doorPositions;
        List<Vector2> drawDoors;


        doorPositions = new List<Vector2>();
        drawDoors = new List<Vector2>();

        int numDoors = 4;

        //for removing doors
        /*
        var clones = GameObject.FindGameObjectsWithTag("Door");

        foreach (var clone in clones)
        {
            Destroy(clone);
        }
        */

        //Pass border size to script
        for (int x = 1; x < map.GetLength(0) - 1; x++)
        {
            for (int y = 1; y < map.GetLength(1) - 1; y++)
            {
                if (map[x, y] == 0 && map[x + 1, y] == 0 && map[x - 1, y] == 0 && map[x, y + 1] == 0 && map[x, y - 1] == 1 && map[x + 1, y - 1] == 1 && map[x - 1, y - 1] == 1 && map[x, y + 2] == 0)
                {
                    Vector2 doorXY;
                    //storing door positions
                    doorXY = new Vector2(x, y);
                    doorPositions.Add(doorXY);
                }
            }
        }

        for (int x = 0; x < numDoors; x++)
        {
            Vector2 doorXY;

            doorXY = doorPositions[Random.Range(0, doorPositions.Count)];
            doorPositions.Remove(doorXY);

            //door
            //var doorTransform = Instantiate(doorPrefab) as Transform;

            float xPos = -mapWidth / 2 + doorXY.x * squareSize + squareSize / 2;
            float yPos = -mapHeight / 2 + doorXY.y * squareSize + squareSize;

            // Assign position
            //doorTransform.position = new Vector3(xPos, yPos, 0);
            //-mapWidth / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2

            doorXY = new Vector2(xPos, yPos);

            drawDoors.Add(doorXY);

        }

        return drawDoors;
    }

    public void DrawOldDoors(List<Vector2> doorLocations)
    {
        var oldDoors = GameObject.FindGameObjectsWithTag("Door");
        MapGenerator map = FindObjectOfType<MapGenerator>();

        string curSeed = map.seed;

        //this is for removing the old doors
        foreach (var door in oldDoors)
        {
            Destroy(door);
        }

        //drawing new doors
        for (int x = 0; x < doorLocations.Count; x++)
        {
            float xPos = doorLocations[x].x;
            float yPos = doorLocations[x].y;

            var doorTransform = Instantiate(doorPrefab) as Transform;
            doorTransform.transform.SetParent(transform);
            doorTransform.position = new Vector3(xPos, yPos, 0);

            //want yo use find vs get here to set door info
            //DoorPrefabInfo info = doorPrefab.GetComponent<DoorPrefabInfo>();
            DoorPrefabInfo mapInfo = FindObjectOfType<DoorPrefabInfo>();

            mapInfo.doorReference = x;
            mapInfo.seedReference = curSeed;

            //Debug.Log("curSeed =" + curSeed + "x =" + x);
        }
    }

    public void PlaceEnemies(int[,] map)
    {
       TestEnemyWalkPath = new List<Vector2>();

        for (int x = 1; x < map.GetLength(0) - 1; x++)
        {
            for (int y = 1; y < map.GetLength(1) - 1; y++)
            {
                AddPlaceToEnemyWalkArea(map, x, y, TestEnemyWalkPath);
            }
        }
    }

    private void AddPlaceToEnemyWalkArea(int[,] map, int x, int y, List<Vector2> list)
    {
        if (map[x, y] == 0 && map[x + 1, y] == 0 && map[x - 1, y] == 0 && map[x, y + 1] == 0 && map[x, y - 1] == 1 && map[x + 1, y - 1] == 1 && map[x - 1, y - 1] == 1 && map[x, y + 2] == 0)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * 1;
            float mapHeight = nodeCountY * 1;
            float xPos = -mapWidth / 2 + x * 1 + 1 / 2;
            float yPos = -mapHeight / 2 + y * 1 + 1;
            list.Add(new Vector2(xPos, yPos));
        }
        else
        {

        }
    }




}
