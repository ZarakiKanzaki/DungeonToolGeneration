using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.Tilemaps;
using UnityEditor;

public class MapGenerator : MonoBehaviour
{
    #region Variables

    [SerializeField]
    InputField txtWidth;
    [SerializeField]
    InputField txtHeight;
    [SerializeField]
    Dropdown ddlType;
    [SerializeField]
    Tilemap topMap;
    [SerializeField]
    RuleTile topTile;
    [Range(0, 100)]
    [SerializeField]
    private int randomFillPercent;
    [SerializeField]
    private Camera camera;

    private int count = 0;
    public string seed;
    public bool useRandomSeed;

    int[,] map;

    #endregion


    void Start()
    {

    }

    void Update()
    {
        Movement();
    }

     private void Movement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        camera.transform.Translate(Vector3.right * 5 * horizontalInput * Time.deltaTime);
        camera.transform.Translate(Vector3.up * 5 * verticalInput * Time.deltaTime);

    }

    void GenerateMap(int width, int height)
    {
        map = new int[width, height];
        RandomFillMap(width, height);

        for (int i = 0; i < 5; i++)
        {
            SmoothMap(width, height);
        }
    }


    void RandomFillMap(int width, int height)
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap(int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y, width, height);

                if (neighbourWallTiles > 4)
                    map[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    map[x, y] = 0;

            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY, int width, int height)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }


    public void SaveAssetMap(Dropdown ddl)
    {
        // comment on this row just to understand if travis works
        string saveName = "tmapXY_" + count;
        var mf = GameObject.Find("Grid");
        string root="Assets/Prefabs/" + ddl.value+ "/";
        if(!System.IO.Directory.Exists(root))
        {
            System.IO.Directory.CreateDirectory(root);
        }
        if (mf)
        {
            var savePath = root + saveName + ".prefab";
            if (PrefabUtility.CreatePrefab(savePath,mf))
            {
                EditorUtility.DisplayDialog("Tilemap saved", "Your Tilemap was saved under" + savePath, "Continue");
            }
            else
            {
                EditorUtility.DisplayDialog("Tilemap NOT saved", "An ERROR occured while trying to saveTilemap under" + savePath, "Continue");
            }


        }


    }


    public void DrawTileMap(Dropdown ddl)
    {
        if (ddl.value == 0)
        {
            var width = Convert.ToInt32(txtWidth.text);
            var height = Convert.ToInt32(txtHeight.text);
            topMap.ClearAllTiles();
            Debug.Log(width);
            Debug.Log(height);
            GenerateMap(width, height);
            if (map != null)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (map[x, y] == 1)
                        {
                            topMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), topTile);
                        }
                    }
                }

            }
        }
    }

}
