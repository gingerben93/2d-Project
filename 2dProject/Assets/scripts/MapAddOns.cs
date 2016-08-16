using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MapAddOns : MonoBehaviour
{
    public Transform doorPrefab;

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
        var clones = GameObject.FindGameObjectsWithTag("Door");

        foreach (var clone in clones)
        {
            Destroy(clone);
        }

        //random number

        Random rnd = new Random();

        //Pass border size to script
        for (int x = 1; x < map.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] == 0 && map[x + 1, y] == 0 && map[x - 1, y] == 0 && map[x, y + 1] == 0 && map[x, y - 1] == 1 && map[x + 1, y - 1] == 1 && map[x - 1, y - 1] == 1 && map[x, y + 2] == 0)
                {
                    //door
                    // var doorTransform = Instantiate(doorPrefab) as Transform;

                    // Assign position
                    //doorTransform.position = new Vector3(-mapWidth / 2 + x * squareSize + squareSize / 2, -mapHeight / 2 + y * squareSize + squareSize, 0);
                    //-mapWidth / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2

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
            var doorTransform = Instantiate(doorPrefab) as Transform;

            float xPos = -mapWidth / 2 + doorXY.x * squareSize + squareSize / 2;
            float yPos = -mapHeight / 2 + doorXY.y * squareSize + squareSize;

            // Assign position
            doorTransform.position = new Vector3(xPos, yPos, 0);
            //-mapWidth / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2

            doorXY = new Vector2(xPos, yPos);

            drawDoors.Add(doorXY);

        }

        return drawDoors;
    }
}
