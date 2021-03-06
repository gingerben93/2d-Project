﻿using UnityEngine;
using System.Collections.Generic;


public class MapAddOns : MonoBehaviour
{
    public Transform doorPrefab;
    public Transform bossDoorPrefab;

    //enemy prefabs
    public Transform EnemyPreFab;
    public Transform TurretPreFab;
    public GameObject SlideEnemyPreFab;

    //gather prefabs
    public GameObject ManaPotion;
    public GameObject HealthPotion;

    public List<Vector2> GenerateDoorsLocations(int[,] map)
    {
        //for pos doors
        List<Vector2> doorPositions;
        MapInformation data = MapGenerator.MapGeneratorSingle.MapInfo[MapGenerator.MapGeneratorSingle.seed];

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

    public void PlaceDoorsOnMap(List<Vector2> doorLocations, List<int> doorType)
    {
        string curSeed = MapGenerator.MapGeneratorSingle.seed;

        //this is for removing the old doors
        foreach (Transform door in transform.Find("DoorHolder"))
        {
            Destroy(door.gameObject);
        }

        //drawing new doors
        for (int x = 0; x < doorLocations.Count; x++)
        {
            float xPos = doorLocations[x].x;
            float yPos = doorLocations[x].y;

            //Debug.Log("Door Type = " + doorType[x]);
            switch (doorType[x])
            {
                case 0:
                    Transform doorTransform = Instantiate(doorPrefab, transform.Find("DoorHolder")) as Transform;
                    doorTransform.name = "Door";
                    doorTransform.position = new Vector3(xPos, yPos, 0);

                    //for setting the map and door ref of each door
                    DoorPrefabInfo mapInfo = FindObjectOfType<DoorPrefabInfo>();
                    //Debug.Log("curSeed = " + curSeed + " currentDoor = " + x);
                    mapInfo.doorReference = x;
                    mapInfo.seedReference = curSeed;
                    break;
                case 1:
                    Transform BossDoor = Instantiate(bossDoorPrefab, transform.Find("DoorHolder")) as Transform;
                    BossDoor.GetComponent<DoorToNewScene>().sceneToLoad = "Boss1";
                    BossDoor.name = "BossDoor1";
                    BossDoor.position = new Vector3(xPos, yPos, 0);
                    break;
            }
        }
    }

    public List<Vector2> GenerateEnemySpawnLocation(MapInformation map1)
    {
        //for pos doors
        List<Vector2> AllPossibleEnemyLocations;
        List<Vector2> EnemyLocations;

        int numEnemies = 5;

        AllPossibleEnemyLocations = new List<Vector2>();
        EnemyLocations = new List<Vector2>();
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
                    AllPossibleEnemyLocations.Add(possibleEnemyXY);
                }
            }
        }

        Vector2 enemyXY;
        float xPos;
        float yPos;
        for (int x = 0; x < numEnemies; x++)
        {
            //don't spawn enemies by possible door locations
            enemyXY = CheckIfDoorLocations(AllPossibleEnemyLocations, map1);

            xPos = -map1.width / 2 + enemyXY.x * map1.squareSize + map1.squareSize / 2;
            yPos = -map1.height / 2 + enemyXY.y * map1.squareSize + map1.squareSize;

            enemyXY = new Vector2(xPos, yPos);

            EnemyLocations.Add(enemyXY);
        }
        return EnemyLocations;
    }

    public List<Vector2> GenerateTurretSpawnLocation(MapInformation map1)
    {
        //for pos doors
        List<Vector2> AllPossibleTurretLocations;
        List<Vector2> TurretLocations;

        int numTurrets = 5;

        AllPossibleTurretLocations = new List<Vector2>();
        TurretLocations = new List<Vector2>();
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
                    AllPossibleTurretLocations.Add(possibleEnemyXY);
                }
            }
        }

        Vector2 enemyXY;
        float xPos;
        float yPos;
        for (int x = 0; x < numTurrets; x++)
        {
            //don't spawn enemies by possible door locations
            enemyXY = CheckIfDoorLocations(AllPossibleTurretLocations, map1);

            xPos = -map1.width / 2 + enemyXY.x * map1.squareSize + map1.squareSize / 2;
            yPos = -map1.height / 2 + enemyXY.y * map1.squareSize + map1.squareSize;

            enemyXY = new Vector2(xPos, yPos);

            TurretLocations.Add(enemyXY);
        }
        return TurretLocations;
    }

    //make sure enemies can't spawn at x distance within door spawn
    public Vector2 CheckIfDoorLocations(List<Vector2> enemyPositions, MapInformation map1)
    {
        Vector2 tempEnemyPos;
        tempEnemyPos = enemyPositions[Random.Range(0, enemyPositions.Count)];
        //foreach (float door in doorLocationsX)

        float xPos;
        float yPos;

        for (int x = 0; x < map1.doorLocationsX.Count; x++)
        {
            xPos = -map1.width / 2 + tempEnemyPos.x * map1.squareSize + map1.squareSize / 2;
            yPos = -map1.height / 2 + tempEnemyPos.y * map1.squareSize + map1.squareSize;

            if (Vector2.Distance(new Vector2(xPos, yPos), new Vector2(map1.doorLocationsX[x], map1.doorLocationsY[x])) <= 5)
            {
                enemyPositions.Remove(tempEnemyPos);
                tempEnemyPos = CheckIfDoorLocations(enemyPositions, map1);
                return tempEnemyPos;
            }
        }
        return tempEnemyPos;
    }

    public void PlaceEnemyOnMap(List<Vector2> enemyLocations, List<Vector2> turretLocations)
    {

        Transform Parent = GameObject.Find("EnemyList").transform;

        //placing enemies enemies
        for (int x = 0; x < enemyLocations.Count; x++)
        {
            float xPos = enemyLocations[x].x;
            float yPos = enemyLocations[x].y;

            var enemyTransform = Instantiate(EnemyPreFab) as Transform;
            enemyTransform.name = EnemyPreFab.name;
            enemyTransform.transform.SetParent(Parent);
            enemyTransform.position = new Vector3(xPos, yPos, 0);
        }

        //placing turrets
        for (int x = 0; x < turretLocations.Count; x++)
        {
            float xPos = turretLocations[x].x;
            float yPos = turretLocations[x].y;

            var turretTransform = Instantiate(TurretPreFab) as Transform;
            turretTransform.name = TurretPreFab.name;
            turretTransform.transform.SetParent(Parent);
            turretTransform.position = new Vector3(xPos, yPos, 0);
        }
    }

    public void SpawnSliderEnemy()
    {
        for (int x = 0; x < 1; x++)
        {
            GameObject SlideEnemy = Instantiate(SlideEnemyPreFab);
            SlideEnemy.transform.SetParent(GameObject.Find("EnemyList").transform);
            SlideEnemy.name = SlideEnemyPreFab.name;
        }
    }

    //public void SpawnSeekingEnemy(MapInformation map1)
    //{
    //    for (int x = 1; x < map1.width - 1; x++)
    //    {
    //        for (int y = 1; y < map1.height - 1; y++)
    //        {
    //            if (map1.map[x, y] == 0)
    //            {
    //                int xPos = -map1.width / 2 + x * map1.squareSize + map1.squareSize / 2;
    //                int yPos = -map1.height / 2 + y * map1.squareSize + map1.squareSize;
    //            }
    //        }
    //    }
    //}

    public List<Vector2> GenerateAllOpenMapLocations(MapInformation map1)
    {
        List<Vector2> allOpenMapLocations;
        allOpenMapLocations = new List<Vector2>();
        Vector2 openLocation;
        for (int x = 1; x < map1.width - 1; x++)
        {
            for (int y = 1; y < map1.height - 1; y++)
            {
                if ((map1.map[x, y] == 0))
                {
                    openLocation = new Vector2(x, y);
                    allOpenMapLocations.Add(openLocation);
                }
            }
        }
        return allOpenMapLocations;
    }

    public void GatherQuestItems(MapInformation map1)
    {
        //List<GatherQuest> listGatherQuests = new List<GatherQuest>();
        GatherQuest[] listGatherQuests = FindObjectsOfType(typeof(GatherQuest)) as GatherQuest[];
        if (listGatherQuests.Length > 0)
        {
            //get spawn locations for items
            List<Vector2> availableSpawnLocations = new List<Vector2>();
            availableSpawnLocations = GenerateAllOpenMapLocations(map1);
            int spawnLocationIndex;

            Transform ItemHeirarchySpawn = GameObject.Find("WorldItems").transform;

            //spawn all items that need to be spawned
            foreach (GatherQuest gatherQuest in listGatherQuests)
            {
                GameObject spawnedItem = Instantiate(gatherQuest.gatherItemPrefab, ItemHeirarchySpawn);
                Debug.Log(spawnedItem.name);
                spawnedItem.name = gatherQuest.gatherTarget;
                spawnLocationIndex = Random.Range(0, availableSpawnLocations.Count);
                spawnedItem.transform.position = new Vector2(-map1.width / 2 + availableSpawnLocations[spawnLocationIndex].x * map1.squareSize + map1.squareSize / 2, -map1.height / 2 + availableSpawnLocations[spawnLocationIndex].y * map1.squareSize + map1.squareSize);
                availableSpawnLocations.RemoveAt(spawnLocationIndex);

                //give item gather script
                //spawnedItem.AddComponent<Gather>();
            }
        }
    }
}
