using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour
{
    //map size
    public int width;
    public int height;

    //fill seeds
    public string seed;
    public bool useRandomSeed;

    //fill range
    [Range(0, 100)]
    public int randomFillPercent;

    //pick how smooth to make ground
    [Range(0, 10)]
    public int smoothness;

    int[,] map;

    //where program stars
    void Start()
    {
        GenerateMap();
    }

    //begining of map generation
    void GenerateMap()
    {
        map = new int[width, height];
        
        //fills map
        RandomFillMap();

        // smooths the map
        for (int i = 0; i < smoothness; i++)
        {
            SmoothMap();
        }

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(map, 1);

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
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

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
