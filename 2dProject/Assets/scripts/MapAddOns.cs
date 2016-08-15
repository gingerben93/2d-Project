﻿using UnityEngine;
using System.Collections;

public class MapAddOns : MonoBehaviour
{
    public Transform doorPrefab;

    public void GenerateDoors(int[,] map, float squareSize, int borderSize)
    {
        int nodeCountX = map.GetLength(0);
        int nodeCountY = map.GetLength(1);
        float mapWidth = nodeCountX * squareSize;
        float mapHeight = nodeCountY * squareSize;

        var clones = GameObject.FindGameObjectsWithTag("Door");

        foreach (var clone in clones)
        {
            Destroy(clone);
        }

        //Pass border size to script
        for (int x = borderSize; x < map.GetLength(0) - borderSize; x++)
        {
            for (int y = borderSize; y < map.GetLength(1) - borderSize; y++)
            {
                if(map[x,y] == 0 && map[x + 1, y] == 0 && map[x - 1, y] == 0 && map[x, y + 1] == 0 && map[x, y - 1] == 1)
                {
                    //door
                    var doorTransform = Instantiate(doorPrefab) as Transform;

                    // Assign position
                    doorTransform.position = new Vector3(-mapWidth / 2 + x * squareSize + squareSize / 2, -mapHeight / 2 + y * squareSize + squareSize, 0);
                    //-mapWidth / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2
                }
            }
        }

    }
}
