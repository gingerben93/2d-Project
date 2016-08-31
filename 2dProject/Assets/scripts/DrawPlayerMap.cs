using UnityEngine;
using System.Collections;
using UnityEditor;

public class DrawPlayerMap : MonoBehaviour {

    public MeshFilter playerLocalMap;
    public MeshFilter playerWorldMap;
    public GameObject player;

    public Transform tempMap;
    //private Vector3 offset;

    bool localMapOn = false;
    bool worldMapOn = false;
    bool makeMap = true;
    // Update is called once per frame
    void LateUpdate () {

        // local map
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (localMapOn)
            {
                //turn map off and erase it
                localMapOn = false;
                playerLocalMap.mesh = null;
            }
            else
            {
                //turn map on and draw it
                worldMapOn = false;
                playerWorldMap.mesh = null;
                localMapOn = true;
                DrawLocalMap();
            }
        }
        if (localMapOn)
        {
            //keep map locked on character
            UpdatePosition();
        }

        //world map
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (worldMapOn)
            {
                //turn map off and erase it
                worldMapOn = false;
                playerWorldMap.mesh = null;
            }
            else
            {
                //turn map on and draw it
                worldMapOn = true;
                playerLocalMap.mesh = null;
                localMapOn = false;
                DrawWorldMap();
            }
        }
        if (worldMapOn)
        {
            //keep map locked on character
            UpdatePosition();
        }
    }

    void DrawLocalMap()
    {
        
        MapGenerator mapGen = FindObjectOfType<MapGenerator>();

        //use this to get all maps

        string mapSeed = mapGen.seed;

        //load map in
        var loadPath = "Assets/CurrentMaps/" + mapSeed + ".asset";
        Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath(loadPath, typeof(Mesh));
        playerLocalMap.mesh = mesh;

        //scale size down and set position
        transform.localScale = new Vector3(.05f, .05f, .05f);
        transform.position = player.transform.position;
        transform.eulerAngles = new Vector3(270, 0, 0);
        //transform.Rotate(Vector3.zero);
    }

    void UpdatePosition()
    {
        if (localMapOn)
        {
            transform.position = player.transform.position - new Vector3(0, -3, 0);
        }
        else
        {
            transform.position = player.transform.position - new Vector3(8,-3,0);
        }
        //transform.Rotate(Vector3.zero);
    }

    void DrawWorldMap()
    {
        if (makeMap)
        {
            makeMap = false;
            GameData gameData = FindObjectOfType<GameData>();

            MeshFilter[] meshFilters = new MeshFilter[gameData.mapSeed.Count];

            for (int x = 0; x < gameData.mapSeed.Count; x++)
            {
                string mapSeed = gameData.mapSeed[x];
                var loadPath = "Assets/CurrentMaps/" + mapSeed + ".asset";
                Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath(loadPath, typeof(Mesh));
               

                var tempMapPrefab = Instantiate(tempMap) as Transform;
                tempMapPrefab.transform.SetParent(transform);
                tempMapPrefab.name = mapSeed;
                tempMapPrefab.GetComponent<MeshFilter>().mesh = mesh;
                meshFilters[x] = tempMapPrefab.GetComponent<MeshFilter>();
            }

            CombineInstance[] combine = new CombineInstance[gameData.mapSeed.Count];

            Debug.Log("combine.Length = " + combine.Length);

            for (int x = 0; x < gameData.mapSeed.Count; x++)
            {
                combine[x].mesh = meshFilters[x].sharedMesh;
                combine[x].transform = Matrix4x4.TRS(new Vector3(x * 150, 0, 0), Quaternion.identity, new Vector3(1, 1, 1));
                
            }
            //65k max vertices or won't work
            playerWorldMap.mesh.CombineMeshes(combine);

            //set scale
            transform.localScale = new Vector3(.05f, .05f, .05f);
            transform.position = player.transform.position;

            string worldMap = "WorldMap";
            var savePath = "Assets/CurrentMaps/" + worldMap + ".asset";
            Debug.Log("Saved Mesh to:" + savePath);
            AssetDatabase.CreateAsset(playerWorldMap.mesh, savePath);
            
        }
        else
        {
            //load map in
            string worldMap = "WorldMap";
            var loadPath = "Assets/CurrentMaps/" + worldMap + ".asset";
            Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath(loadPath, typeof(Mesh));
            transform.localScale = new Vector3(.05f, .05f, .05f);
            transform.position = player.transform.position;
            playerWorldMap.mesh = mesh;
        }
    }
   
}
