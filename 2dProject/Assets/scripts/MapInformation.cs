using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInformation {

    public int index { get; set; }
    public int mapSet { get; set; }
    public int width { get; set; }
    public int height { get; set; }
    public int randomFillPercent { get; set; }
    public int passageLength { get; set; }
    public int smoothness { get; set; }
    public int squareSize { get; set; }
    public int[,] map { get; set; }
    public int[,] borderedMap { get; set; }

    public List<Vector2> possibleDoorLocations;
    public List<Vector2> doorLocations;
    public List<Vector2> enemyLocations;



    //   void Start () {

    //}
}
