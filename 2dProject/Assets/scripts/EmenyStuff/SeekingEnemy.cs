using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingEnemy : MonoBehaviour {

    MapInformation map1;
    int oldX, oldY;
    int currentX = -10000;
    int currentY;
    Vector2 move;

    List<int> possibleSpawnX = new List<int>();
    List<int> possibleSpawnY = new List<int>();

    List<Vector2> forwardMoves = new List<Vector2>();
    List<Vector2> SideMoves = new List<Vector2>();

    //random variables
    float speed = .1f;
    bool running = false;


    void Start()
    {
        speed = Random.Range(.1f, .2f);
        map1 = MapGenerator.MapGeneratorSingle.MapInfo[MapGenerator.MapGeneratorSingle.seed];

        for (int x = 1; x < map1.width - 1; x++)
        {
            for (int y = 1; y < map1.height - 1; y++)
            {
                if (map1.map[x, y] == 0)
                {
                    possibleSpawnX.Add(x);
                    possibleSpawnY.Add(y);
                }
            }
        }

        int randomNumber = Random.Range(0, possibleSpawnX.Count);

        currentX = possibleSpawnX[randomNumber];
        currentY = possibleSpawnY[randomNumber];

        oldX = currentX - 1;
        oldY = currentY - 1;

        transform.position = new Vector2(-map1.width / 2 + currentX * map1.squareSize + map1.squareSize / 2, -map1.height / 2 + currentY * map1.squareSize + map1.squareSize);

        //gets first moves
        NextMove();
    }

	void Update ()
    {
        //stops moving when player is in range and do something
        if (Vector2.Distance(transform.position, PlayerController.PlayerControllerSingle.transform.position) <= 5f)
        {

        }
        else
        {
            //gets new move when old move reached
            if (Vector2.Distance(transform.position, move) <= .2f && !running)
            {
                running = true;
                NextMove();
            }
            transform.position = Vector2.MoveTowards(transform.position, move, speed);
        }
	}

    void NextMove()
    {
        //clear elements, not reset capacity
        forwardMoves.Clear();
        SideMoves.Clear();

        //find out what direction object is facing, checks forwards moves first only side if no forwards
        if (currentX - oldX == 1 && currentY - oldY == 1)
        {
            //forward
            GetMoveXY(currentX + 1, currentY + 1);
            //left
            GetMoveXY(currentX, currentY + 1);
            //right
            GetMoveXY(currentX + 1, currentY);

            //side left
            GetSideMove(currentX + 1, currentY - 1);
            //side right
            GetSideMove(currentX - 1, currentY + 1);
        }
        else if (currentX - oldX == -1 && currentY - oldY == -1)
        {
            //forward
            GetMoveXY(currentX - 1, currentY - 1);
            //left
            GetMoveXY(currentX, currentY - 1);
            //right
            GetMoveXY(currentX - 1, currentY);

            //side left
            GetSideMove(currentX - 1, currentY + 1);
            //side right
            GetSideMove(currentX + 1, currentY - 1);
        }
        else if (currentX - oldX == 1 && currentY - oldY == -1)
        {
            //forward
            GetMoveXY(currentX + 1, currentY - 1);
            //left
            GetMoveXY(currentX, currentY - 1);
            //right
            GetMoveXY(currentX + 1, currentY);

            //side left
            GetSideMove(currentX - 1, currentY - 1);
            //side right
            GetSideMove(currentX + 1, currentY + 1);

        }
        else if (currentX - oldX == -1 && currentY - oldY == 1)
        {
            //forward
            GetMoveXY(currentX - 1, currentY + 1);
            //left
            GetMoveXY(currentX, currentY + 1);
            //right
            GetMoveXY(currentX - 1, currentY);

            //side left
            GetSideMove(currentX + 1, currentY + 1);
            //side right
            GetSideMove(currentX - 1, currentY - 1);

        }
        else if (currentX - oldX == 1)
        {
            //forward
            GetMoveXY(currentX + 1, currentY);
            //left
            GetMoveXY(currentX + 1, currentY + 1);
            //right
            GetMoveXY(currentX + 1, currentY - 1);

            //side left
            GetSideMove(currentX, currentY + 1);
            //side right
            GetSideMove(currentX, currentY - 1);
        }
        else if(currentY - oldY == 1)
        {
            //forward
            GetMoveXY(currentX, currentY + 1);
            //left
            GetMoveXY(currentX + 1, currentY + 1);
            //right
            GetMoveXY(currentX - 1, currentY + 1);

            //side left
            GetSideMove(currentX + 1, currentY);
            //side right
            GetSideMove(currentX - 1, currentY);
        }
        else if (currentX - oldX == -1)
        {
            //forward
            GetMoveXY(currentX - 1, currentY);
            //left
            GetMoveXY(currentX - 1, currentY + 1);
            //right
            GetMoveXY(currentX - 1, currentY - 1);

            //side left
            GetSideMove(currentX, currentY + 1);
            //side right
            GetSideMove(currentX, currentY - 1);

        }
        else if (currentY - oldY == -1)
        {
            //forward
            GetMoveXY(currentX, currentY - 1);
            //left
            GetMoveXY(currentX - 1, currentY - 1);
            //right
            GetMoveXY(currentX + 1, currentY - 1);

            //side left
            GetSideMove(currentX + 1, currentY);
            //side right
            GetSideMove(currentX - 1, currentY);
        }

        //set old position before new one is found
        oldX = currentX;
        oldY = currentY;

        if (forwardMoves.Count != 0)
        {
            FindBestMove(forwardMoves);
        }
        else if (SideMoves.Count != 0)
        {
            FindBestMove(SideMoves);
        }
        else
        {
            // can only happen one case fix that by turing around
            Debug.Log("no move found");
        }
        running = false;
    }

    void GetMoveXY(int x, int y)
    {
        if (map1.map[x, y] == 0)
        {
            forwardMoves.Add(new Vector2(x, y));
        }
        else
        {
            //nothing there is a wall
        }
    }

    void GetSideMove(int x, int y)
    {
        if (map1.map[x, y] == 0)
        {
            SideMoves.Add(new Vector2(x, y));
        }
        else
        {
            //do nothing there is a wall
        }
    }

    void FindBestMove(List<Vector2> MovesToCheck)
    {
        //random distance that will always be futher away
        float distance = 1000;

        foreach (Vector2 nextMove in MovesToCheck)
        {
            //have to convert to world coordianates
            int xPos = -map1.width / 2 + (int)(nextMove.x) * map1.squareSize + map1.squareSize / 2;
            int yPos = -map1.height / 2 + (int)nextMove.y * map1.squareSize + map1.squareSize;

            if (Vector2.Distance(new Vector2(xPos, yPos), PlayerController.PlayerControllerSingle.transform.position) <= distance)
            {
                distance = Vector2.Distance(new Vector2(xPos, yPos), PlayerController.PlayerControllerSingle.transform.position);
                move = new Vector2(xPos, yPos);

                currentX = (int)nextMove.x;
                currentY = (int)nextMove.y;
            }
        }
    }
}
