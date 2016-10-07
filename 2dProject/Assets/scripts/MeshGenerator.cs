using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class MeshGenerator : MonoBehaviour
{

    public SquareGrid squareGrid;
    public MeshFilter cave;

    List<Vector3> vertices;
    List<int> triangles;

    Dictionary<int, List<Triangle>> triangleDictionary = new Dictionary<int, List<Triangle>>();
    List<List<int>> outlines = new List<List<int>>();
    HashSet<int> checkedVertices = new HashSet<int>();

    List<string> groundPieceName = new List<string>();
    int groundPieceIndex;

    public Transform groundPiece;

    // need to save the mesh for the map
    public void GenerateMesh(int[,] map, float squareSize)
    {

        triangleDictionary.Clear();
        outlines.Clear();
        checkedVertices.Clear();

        squareGrid = new SquareGrid(map, squareSize);

        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
        {
            for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
            {
                TriangulateSquare(squareGrid.squares[x, y]);
            }
        }

        Mesh mesh = new Mesh();
        cave.mesh = mesh;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        int tileAmount = 10;
        Vector2[] uvs = new Vector2[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
        {
            float percentX = Mathf.InverseLerp(-map.GetLength(0) / 2 * squareSize, map.GetLength(0) / 2 * squareSize, vertices[i].x) * tileAmount;
            float percentY = Mathf.InverseLerp(-map.GetLength(0) / 2 * squareSize, map.GetLength(0) / 2 * squareSize, vertices[i].z) * tileAmount;
            uvs[i] = new Vector2(percentX, percentY);
        }
        mesh.uv = uvs;

        Generate2DColliders();

        //save mesh
        MapGenerator mapInfo = GameObject.FindObjectOfType<MapGenerator>();
        string mapSeed = mapInfo.seed;
        var savePath = "Assets/CurrentMaps/" + mapSeed + ".asset";
        Debug.Log("Saved Mesh to:" + savePath);
        AssetDatabase.CreateAsset(cave.mesh, savePath);
    }
    
    //doesn't recreate mesh; load it from data; need to find a way to save 2d edge collider as well
    public void LoadMeshFromAssests(int[,] map, float squareSize)
    {
        MapGenerator mapInfo = GameObject.FindObjectOfType<MapGenerator>();
        string mapSeed = mapInfo.seed;
        var loadPath = "Assets/CurrentMaps/" + mapSeed + ".asset";

        Debug.Log("Load Mesh from:" + loadPath);
        Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath(loadPath, typeof(Mesh));
        cave.mesh = mesh;

        triangleDictionary.Clear();
        outlines.Clear();
        checkedVertices.Clear();

        squareGrid = new SquareGrid(map, squareSize);

        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
        {
            for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
            {
                TriangulateSquare(squareGrid.squares[x, y]);
            }
        }

        Generate2DColliders();

    }

    void Generate2DColliders()
    {

        EdgeCollider2D[] currentColliders = gameObject.GetComponents<EdgeCollider2D>();
        for (int i = 0; i < currentColliders.Length; i++)
        {
            Destroy(currentColliders[i]);
        }

        CalculateMeshOutlines();

        foreach (List<int> outline in outlines)
        {
            EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
            Vector2[] edgePoints = new Vector2[outline.Count];

            foreach (Transform child in transform)
            {
                if (child.tag == "Ground")
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
            float vertX;
            float vertXPLus1;
            float vertXMinus1;
            float vertZ;
            float vertZPLus1;
            float vertZMinus1;
            int count = outline.Count;
            groundPieceName.Add("FlatGround_CAVE");
            groundPieceName.Add("RLGround_CAVE");
            groundPieceName.Add("UPSlope_To_Ground");
            groundPieceName.Add("Groud_To_UPSlope");

            for (int i = 1; i <  outline.Count; i++)
            {
                edgePoints[i] = new Vector2(vertices[outline[i]].x, vertices[outline[i]].z);

                //for loading sprites in

                vertX = vertices[outline[i]].x;
                vertXPLus1 = vertices[outline[(i + 1) % count]].x;
                vertXMinus1 = vertices[outline[(count + i - 1) % count]].x;
                //Z IS Y VALUE
                vertZ = vertices[outline[i]].z;
                vertZPLus1 = vertices[outline[(i + 1) % count]].z;
                vertZMinus1 = vertices[outline[(count + i - 1) % count]].z;

                if (i == (outline.Count - 1))
                {
                    vertXPLus1 = vertices[outline[0]].x;
                    vertZPLus1 = vertices[outline[0]].z;
                }

                DrawSprites(vertX, vertXPLus1, vertXMinus1, vertZ, vertZPLus1, vertZMinus1, count, i);
            }
            edgeCollider.points = edgePoints;
        }
    }

    void DrawSprites(float vertX, float vertXPLus1,  float vertXMinus1 ,float vertZ , float vertZPLus1 ,float vertZMinus1 , int count, int i)
    {
        
        
        var groundSprite = Instantiate(groundPiece) as Transform;
        groundSprite.transform.SetParent(transform);
        groundSprite.name = i.ToString();

        //horizontal and vetical sprites
        //does not work when change between - and minus fix that problem
        if (vertZ == vertZPLus1 || vertX == vertXPLus1 || vertZ == vertZMinus1 || vertX == vertXMinus1)
        {
            //top piece
            if (vertX > vertXPLus1 && (vertZ == vertZPLus1 || vertZ == vertZMinus1))
            {
                groundPiece.transform.position = new Vector3(vertX, vertZ + .5f, 0);
                groundPiece.eulerAngles = new Vector3(180, 0, 0);
                if (Mathf.Abs(vertX - vertXPLus1) <= .5f || Mathf.Abs(vertX - vertXMinus1) <= .5f)
                {
                    if ((Mathf.Abs(vertZ - vertZMinus1) == 0f) && (vertZ - vertZPLus1) > 0)
                    {
                        groundPiece.eulerAngles = new Vector3(0, 0, 180);
                        groundPieceIndex = 3;
                    }
                    else if ((Mathf.Abs(vertZ - vertZPLus1) == 0f) && (vertZ - vertZMinus1) > 0)
                    {
                        groundPieceIndex = 3;
                    }
                    else
                    {
                        if ((vertZ - vertZMinus1) < 0)
                        {
                            groundPiece.eulerAngles = new Vector3(0, 0, 180);
                            groundPieceIndex = 2;
                        }
                        else
                            groundPieceIndex = 2;
                    }
                }
                else {
                    groundPieceIndex = 0;
                }
            }
            //bottom pieces
            else if (vertX < vertXPLus1 && (vertZ == vertZPLus1 || vertZ == vertZMinus1))
            {
                
                groundPiece.transform.position = new Vector3(vertX, vertZ - .5f, 0);
                groundPiece.eulerAngles = new Vector3(0, 0, 0);
                if (Mathf.Abs(vertX - vertXPLus1) <= .5f || Mathf.Abs(vertX - vertXMinus1) <= .5f)
                {
                    if (Mathf.Abs(vertX - vertXPLus1) <= .5f || Mathf.Abs(vertX - vertXMinus1) <= .5f)
                    {
                        if ((Mathf.Abs(vertZ - vertZMinus1) == 0f) && (vertZ - vertZPLus1) < 0)
                        {
                            groundPiece.eulerAngles = new Vector3(0, 0, 0);
                            groundPieceIndex = 3;
                        }
                        else if ((Mathf.Abs(vertZ - vertZPLus1) == 0f) && (vertZ - vertZMinus1) < 0)
                        {
                            groundPiece.eulerAngles = new Vector3(0, 180, 0);
                            groundPieceIndex = 3;
                        }
                        else
                        {
                            if ((vertZ - vertZPLus1) > 0)
                            {
                                groundPiece.eulerAngles = new Vector3(0, 180, 0);
                                groundPieceIndex = 2;
                            }
                            else
                                groundPieceIndex = 2;
                        }
                    }
                }
                else {
                    groundPieceIndex = 0;
                }
                
            }

            //left wall
            else if (vertZ > vertZPLus1 || vertZ < vertZMinus1)
            {
                groundPiece.transform.position = new Vector3(vertX - .5f, vertZ , 0);
                groundPiece.eulerAngles = new Vector3(0, 0, 270);
                if (vertZ > vertZPLus1 && (vertX == vertXPLus1 || vertX == vertXMinus1))
                {
                    groundPiece.transform.position = new Vector3(vertX - .5f, vertZ, 0);
                    groundPiece.eulerAngles = new Vector3(0, 0, 270);
                    if (Mathf.Abs(vertZ - vertZPLus1) <= .5f || Mathf.Abs(vertZ - vertZMinus1) <= .5f)
                    {
                        if ((Mathf.Abs(vertX - vertXMinus1) == 0f) && (vertX - vertXPLus1) < 0)
                        {
                            groundPiece.eulerAngles = new Vector3(0, 0, 270);
                            groundPieceIndex = 3;
                        }
                        else if ((Mathf.Abs(vertX - vertXPLus1) == 0f) && (vertX - vertXMinus1) < 0)
                        {
                            groundPiece.eulerAngles = new Vector3(180, 0, 270);
                            groundPieceIndex = 3;
                        }
                        else
                        {
                            if ((vertX - vertXPLus1) > 0)
                            {
                                groundPiece.eulerAngles = new Vector3(180, 0, 270);
                                groundPieceIndex = 2;
                            }
                            else
                                groundPieceIndex = 2;
                        }
                    }
                    else
                    {
                        groundPieceIndex = 0;
                    }
                }
            }

            //right wall
            else if (vertZ < vertZPLus1 || vertZ > vertZMinus1)
            {
                groundPiece.transform.position = new Vector3(vertX + .5f, vertZ, 0);
                groundPiece.eulerAngles = new Vector3(0, 0, 90);
                if (Mathf.Abs(vertZ - vertZPLus1) <= .5f || Mathf.Abs(vertZ - vertZMinus1) <= .5f)
                {
                    if ((Mathf.Abs(vertX - vertXMinus1) == 0f) && (vertX - vertXPLus1) > 0)
                    {
                        groundPiece.eulerAngles = new Vector3(0, 0, 90);
                        groundPieceIndex = 3;
                    }
                    else if ((Mathf.Abs(vertX - vertXPLus1) == 0f) && (vertX - vertXMinus1) > 0)
                    {
                        groundPiece.eulerAngles = new Vector3(180, 0, 90);
                        groundPieceIndex = 3;
                    }
                    else
                    {
                        if ((vertX - vertXPLus1) < 0)
                        {
                            groundPiece.eulerAngles = new Vector3(180, 0, 90);
                            groundPieceIndex = 2;
                        }
                        else
                            groundPieceIndex = 2;
                    }
                }
                else
                {
                    groundPieceIndex = 0;
                }
            }
        }

        //diagonal sprites
        else if (vertX != vertXPLus1 && vertZ != vertZPLus1 && (vertZPLus1 - vertZMinus1) != 0 && (vertXPLus1 - vertXMinus1) != 0)
        {
            if (vertX < vertXPLus1 && vertZ < vertZPLus1)
            {
                groundPiece.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (vertX > vertXPLus1 && vertZ < vertZPLus1)
            {
                groundPiece.eulerAngles = new Vector3(0, 0, 90);
            }
            else if (vertX > vertXPLus1 && vertZ > vertZPLus1)
            {
                groundPiece.eulerAngles = new Vector3(0, 0, 180);
            }
            else if (vertX < vertXPLus1 && vertZ > vertZPLus1)
            {
                groundPiece.eulerAngles = new Vector3(0, 0, 270);
            }
            groundPieceIndex = 1;
            groundPiece.transform.position = new Vector3(vertX, vertZ, 0);
            groundPiece.transform.localScale = new Vector3(.125f, .125f, 0);

        }
        SpriteRenderer renderer = groundSprite.GetComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load(groundPieceName[groundPieceIndex], typeof(Sprite)) as Sprite;
        renderer.transform.eulerAngles = groundPiece.eulerAngles;
        renderer.transform.position = groundPiece.transform.position;
    }

    void TriangulateSquare(Square square)
    {
        switch (square.configuration)
        {
            case 0:
                break;

            // 1 points:
            case 1:
                MeshFromPoints(square.centreLeft, square.centreBottom, square.bottomLeft);
                break;
            case 2:
                MeshFromPoints(square.bottomRight, square.centreBottom, square.centreRight);
                break;
            case 4:
                MeshFromPoints(square.topRight, square.centreRight, square.centreTop);
                break;
            case 8:
                MeshFromPoints(square.topLeft, square.centreTop, square.centreLeft);
                break;

            // 2 points:
            case 3:
                MeshFromPoints(square.centreRight, square.bottomRight, square.bottomLeft, square.centreLeft);
                break;
            case 6:
                MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.centreBottom);
                break;
            case 9:
                MeshFromPoints(square.topLeft, square.centreTop, square.centreBottom, square.bottomLeft);
                break;
            case 12:
                MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreLeft);
                break;
            case 5:
                MeshFromPoints(square.centreTop, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft, square.centreLeft);
                break;
            case 10:
                MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.centreBottom, square.centreLeft);
                break;

            // 3 point:
            case 7:
                MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.bottomLeft, square.centreLeft);
                break;
            case 11:
                MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.bottomLeft);
                break;
            case 13:
                MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft);
                break;
            case 14:
                MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centreBottom, square.centreLeft);
                break;

            // 4 point:
            case 15:
                MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
                checkedVertices.Add(square.topLeft.vertexIndex);
                checkedVertices.Add(square.topRight.vertexIndex);
                checkedVertices.Add(square.bottomRight.vertexIndex);
                checkedVertices.Add(square.bottomLeft.vertexIndex);
                break;
        }

    }

    void MeshFromPoints(params Node[] points)
    {
        AssignVertices(points);

        if (points.Length >= 3)
            CreateTriangle(points[0], points[1], points[2]);
        if (points.Length >= 4)
            CreateTriangle(points[0], points[2], points[3]);
        if (points.Length >= 5)
            CreateTriangle(points[0], points[3], points[4]);
        if (points.Length >= 6)
            CreateTriangle(points[0], points[4], points[5]);

    }

    void AssignVertices(Node[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].vertexIndex == -1)
            {
                points[i].vertexIndex = vertices.Count;
                vertices.Add(points[i].position);
            }
        }
    }

    void CreateTriangle(Node a, Node b, Node c)
    {
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);

        Triangle triangle = new Triangle(a.vertexIndex, b.vertexIndex, c.vertexIndex);
        AddTriangleToDictionary(triangle.vertexIndexA, triangle);
        AddTriangleToDictionary(triangle.vertexIndexB, triangle);
        AddTriangleToDictionary(triangle.vertexIndexC, triangle);
    }

    void AddTriangleToDictionary(int vertexIndexKey, Triangle triangle)
    {
        if (triangleDictionary.ContainsKey(vertexIndexKey))
        {
            triangleDictionary[vertexIndexKey].Add(triangle);
        }
        else {
            List<Triangle> triangleList = new List<Triangle>();
            triangleList.Add(triangle);
            triangleDictionary.Add(vertexIndexKey, triangleList);
        }
    }

    void CalculateMeshOutlines()
    {

        for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
        {
            if (!checkedVertices.Contains(vertexIndex))
            {
                int newOutlineVertex = GetConnectedOutlineVertex(vertexIndex);
                if (newOutlineVertex != -1)
                {
                    checkedVertices.Add(vertexIndex);

                    List<int> newOutline = new List<int>();
                    newOutline.Add(vertexIndex);
                    outlines.Add(newOutline);
                    FollowOutline(newOutlineVertex, outlines.Count - 1);
                    outlines[outlines.Count - 1].Add(vertexIndex);
                }
            }
        }
    }

    void FollowOutline(int vertexIndex, int outlineIndex)
    {
        outlines[outlineIndex].Add(vertexIndex);
        checkedVertices.Add(vertexIndex);
        int nextVertexIndex = GetConnectedOutlineVertex(vertexIndex);

        if (nextVertexIndex != -1)
        {
            FollowOutline(nextVertexIndex, outlineIndex);
        }
    }

    int GetConnectedOutlineVertex(int vertexIndex)
    {
        List<Triangle> trianglesContainingVertex = triangleDictionary[vertexIndex];

        for (int i = 0; i < trianglesContainingVertex.Count; i++)
        {
            Triangle triangle = trianglesContainingVertex[i];

            for (int j = 0; j < 3; j++)
            {
                int vertexB = triangle[j];
                if (vertexB != vertexIndex && !checkedVertices.Contains(vertexB))
                {
                    if (IsOutlineEdge(vertexIndex, vertexB))
                    {
                        return vertexB;
                    }
                }
            }
        }

        return -1;
    }

    bool IsOutlineEdge(int vertexA, int vertexB)
    {
        List<Triangle> trianglesContainingVertexA = triangleDictionary[vertexA];
        int sharedTriangleCount = 0;

        for (int i = 0; i < trianglesContainingVertexA.Count; i++)
        {
            if (trianglesContainingVertexA[i].Contains(vertexB))
            {
                sharedTriangleCount++;
                if (sharedTriangleCount > 1)
                {
                    break;
                }
            }
        }
        return sharedTriangleCount == 1;
    }

    struct Triangle
    {
        public int vertexIndexA;
        public int vertexIndexB;
        public int vertexIndexC;
        int[] vertices;

        public Triangle(int a, int b, int c)
        {
            vertexIndexA = a;
            vertexIndexB = b;
            vertexIndexC = c;

            vertices = new int[3];
            vertices[0] = a;
            vertices[1] = b;
            vertices[2] = c;
        }

        public int this[int i]
        {
            get
            {
                return vertices[i];
            }
        }


        public bool Contains(int vertexIndex)
        {
            return vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC;
        }
    }

    public class SquareGrid
    {
        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector3 pos = new Vector3(-mapWidth / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2);
                    controlNodes[x, y] = new ControlNode(pos, map[x, y] == 1, squareSize);
                }
            }

            squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for (int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
                    squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
                }
            }

        }
    }

    public class Square
    {

        public ControlNode topLeft, topRight, bottomRight, bottomLeft;
        public Node centreTop, centreRight, centreBottom, centreLeft;
        public int configuration;

        public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft)
        {
            topLeft = _topLeft;
            topRight = _topRight;
            bottomRight = _bottomRight;
            bottomLeft = _bottomLeft;

            centreTop = topLeft.right;
            centreRight = bottomRight.above;
            centreBottom = bottomLeft.right;
            centreLeft = bottomLeft.above;

            if (topLeft.active)
                configuration += 8;
            if (topRight.active)
                configuration += 4;
            if (bottomRight.active)
                configuration += 2;
            if (bottomLeft.active)
                configuration += 1;
        }

    }

    public class Node
    {
        public Vector3 position;
        public int vertexIndex = -1;

        public Node(Vector3 _pos)
        {
            position = _pos;
        }
    }

    public class ControlNode : Node
    {

        public bool active;
        public Node above, right;

        public ControlNode(Vector3 _pos, bool _active, float squareSize) : base(_pos)
        {
            active = _active;
            above = new Node(position + Vector3.forward * squareSize / 2f);
            right = new Node(position + Vector3.right * squareSize / 2f);
        }

    }
}