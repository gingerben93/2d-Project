using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MapAddOns : MonoBehaviour
{
    public Transform doorPrefab;

    public Transform EnenyPreFab;

    public List<Vector2> GenerateDoors(int[,] map, float squareSize, int borderSize)
    {
        //for pos doors
        List<Vector2> doorPositions;


        doorPositions = new List<Vector2>();

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
        return doorPositions;
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

    public List<Vector2> SpawnEnemy(int[,] map, float squareSize, int borderSize)
    {
        //for size of map
        int nodeCountX = map.GetLength(0);
        int nodeCountY = map.GetLength(1);
        float mapWidth = nodeCountX * squareSize;
        float mapHeight = nodeCountY * squareSize;

        //for pos doors
        List<Vector2> enemyPositions;
        List<Vector2> drawEnemys;

        int numEnemies = 5;

        enemyPositions = new List<Vector2>();
        drawEnemys = new List<Vector2>();

        //Pass border size to script
        for (int x = 1; x < map.GetLength(0) - 1; x++)
        {
            for (int y = 1; y < map.GetLength(1) - 1; y++)
            {
                if ((map[x, y] == 0 && map[x + 1, y] == 0 && map[x - 1, y] == 0 && map[x, y + 1] == 0) && 
                    (map[x, y - 1] == 1 || map[x + 1, y - 1] == 1 || map[x - 1, y - 1] == 1))
                {
                    Vector2 enemyXY;
                    enemyXY = new Vector2(x, y);
                    enemyPositions.Add(enemyXY);
                }
            }
        }

        for (int x = 0; x < numEnemies; x++)
        {
            Vector2 doorXY;

            doorXY = enemyPositions[Random.Range(0, enemyPositions.Count)];
            enemyPositions.Remove(doorXY);

            float xPos = -mapWidth / 2 + doorXY.x * squareSize + squareSize / 2;
            float yPos = -mapHeight / 2 + doorXY.y * squareSize + squareSize;


            doorXY = new Vector2(xPos, yPos);

            drawEnemys.Add(doorXY);

        }

        return drawEnemys;
    }

    public void DrawEnemys(List<Vector2> enemyLocations)
    {
        var oldEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        //MapGenerator map = FindObjectOfType<MapGenerator>();

        //this is for removing the old doors
        foreach (var enemy in oldEnemy)
        {
            Destroy(enemy);
        }

        //drawing new doors
        for (int x = 0; x < enemyLocations.Count; x++)
        {
            float xPos = enemyLocations[x].x;
            float yPos = enemyLocations[x].y;

            var doorTransform = Instantiate(EnenyPreFab) as Transform;
            doorTransform.transform.SetParent(GameObject.Find("EnemyList").transform);
            doorTransform.position = new Vector3(xPos, yPos, 0);

        }
    }
}
