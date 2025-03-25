using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.Tilemaps.Tilemap;
using Random = UnityEngine.Random;


public class Generator : MonoBehaviour
{
    [Header("World Settings")]
    public int sizeX = 50;
    public int sizeY = 10;
    public int sizeZ = 50;
    public float seed;
    [Range(0, 1)] public float stoneLimit = 0.25f;

    [Header("Noise Settings")]
    public float scale = 25;

    [Header("Tiles")]
    public GameObject groundTile;
    public GameObject stoneTile;

    private float[,] grid;

    void Start()
    {
        grid = new float[sizeX, sizeZ];

        GenerateNoise();
        BuildWorld();
    }

    void GenerateNoise()
    {
        seed = Random.Range(-1000, 1000);
        for (int z = 0; z < sizeZ; z++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                var px = (x + seed) / scale;
                var pz = (z + seed) / scale;

                grid[x, z] = (noise.snoise(new float2(px, pz)) + 1) / 2;
            }
        }
    }

    void BuildWorld()
    {
        for (int z = 0; z < sizeZ; z++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                var height = (int)(grid[x, z] * sizeY);

                for(int y = height; y >= 0; y--)
                {
                    
                    if (grid[x, z] <= stoneLimit)
                    {
                        Instantiate(stoneTile, new Vector3(x, y, z), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(groundTile, new Vector3(x, y, z), Quaternion.identity);
                    }
                }

            }
        }
    }
}
