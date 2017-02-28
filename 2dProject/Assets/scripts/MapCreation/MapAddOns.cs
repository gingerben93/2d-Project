using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MapAddOns : MonoBehaviour
{
    public Transform doorPrefab;

    public Transform EnenyPreFab;

    public List<Vector2> GenerateDoors(int[,] map)
    {
        //for pos doors
        List<Vector2> doorPositions;

        MapGenerator mapInfo = FindObjectOfType<MapGenerator>();
        MapInformation data = mapInfo.MapInfo[mapInfo.seed];

        doorPositions = new List<Vector2>();

        //Pass border size to script
        for (int x = 1; x < data.width - 1; x++)
        {
            for (int y = 1; y < data.height - 1; y++)
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

    public List<Vector2> SpawnEnemy(MapInformation map1)
    {
        //for pos doors
        List<Vector2> enemyPositions;
        List<Vector2> drawEnemys;

        int numEnemies = 5;

        enemyPositions = new List<Vector2>();
        drawEnemys = new List<Vector2>();
        //pick spots for enemies to spawn
        for (int x = 1; x < map1.width - 1; x++)
        {
            for (int y = 1; y < map1.height - 1; y++)
            {
                if ((map1.map[x, y] == 0 && map1.map[x + 1, y] == 0 && map1.map[x - 1, y] == 0 && map1.map[x, y + 1] == 0) &&
                    (map1.map[x, y - 1] == 1 || map1.map[x + 1, y - 1] == 1 || map1.map[x - 1, y - 1] == 1))
                {
                    Vector2 possibleEnemyXY;
                    possibleEnemyXY = new Vector2(x, y);
                    enemyPositions.Add(possibleEnemyXY);
                }
            }
        }

        Vector2 enemyXY;
        float xPos;
        float yPos;
        for (int x = 0; x < numEnemies; x++)
        {
            //don't spawn enemies by possible door locations
            enemyXY = CheckIfDoorLocations(enemyPositions, map1.doorLocationsX, map1.doorLocationsY);

            xPos = -map1.width / 2 + enemyXY.x * map1.squareSize + map1.squareSize / 2;
            yPos = -map1.height / 2 + enemyXY.y * map1.squareSize + map1.squareSize;

            enemyXY = new Vector2(xPos, yPos);

            drawEnemys.Add(enemyXY);
        }
        return drawEnemys;
    }

    //make sure enemies can't spawn at x distance within door spawn
    public Vector2 CheckIfDoorLocations(List<Vector2> enemyPositions, List<float> doorLocationsX, List<float> doorLocationsY)
    {
        Vector2 tempEnemyPos;
        tempEnemyPos = enemyPositions[Random.Range(0, enemyPositions.Count)];
        //foreach (float door in doorLocationsX)
        for(int x = 0; x < doorLocationsX.Count; x++)
        {
            if (Vector2.Distance(tempEnemyPos, new Vector2(doorLocationsX[x], doorLocationsY[x])) <= 2)
            {
                enemyPositions.Remove(tempEnemyPos);
                tempEnemyPos = CheckIfDoorLocations(enemyPositions, doorLocationsX, doorLocationsY);
                return tempEnemyPos;
            }
        }
        return tempEnemyPos;
    }

    public void RemoveAllEnemies()
    {
        var oldEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        //this is for removing the old doors
        foreach (var enemy in oldEnemy)
        {
            Destroy(enemy);
        }
    }

    public void DrawEnemys(List<Vector2> enemyLocations)
    {
        RemoveAllEnemies();

        //drawing new enemies
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
