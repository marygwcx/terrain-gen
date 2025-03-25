using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Unity.Mathematics;
using UnityEngine.UIElements;

public class Generator3D : MonoBehaviour
{
    [Header("World Settings")]
    public int sizeX = 50;
    public int sizeY = 50;
    public int sizeZ = 50;
    public float seed;
    [Range(0, 1)] public float groundLevel = 0.5f;

    [Header("Noise Settings")]
    public float scale = 25;

    [Header("Tiles")]
    public GameObject groundTile;

    private float[,,] grid;

    void Start()
    {
        grid = new float[sizeX, sizeY, sizeZ];

        GenerateNoise();
        BuildWorld();
    }

    void GenerateNoise()
    {
        seed = Random.Range(-1000, 1000);

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    var px = (x + seed) / scale;
                    var py = (y + seed) / scale;
                    var pz = (z + seed) / scale;

                    grid[x, y, z] = (noise.snoise(new float3(px, py, pz)) + 1) / 2;
                }
            }
        }
    }

    void BuildWorld()
    {
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    if (grid[x, y, z] >= groundLevel && IsExposed(x, y, z))
                    {
                        Instantiate(groundTile, new Vector3(x, y, z), Quaternion.identity);
                    }
                }
            }
        }
    }

    bool IsExposed(int x, int y, int z)
    {
        int[,] direction =
        {
            {1, 0, 0 }, {-1, 0, 0 }, //left right
            {0, 1, 0 }, {0, -1, 0 }, //up down
            {0, 0, 1}, {0, 0, -1} //forward backward
        };

        for (int i=0; i < 6; i++)
        {
            var nx = x + direction[i, 0];
            var ny = y + direction[i, 1];
            var nz = z + direction[i, 2];

            if (nx < 0 || nx >= sizeX || ny < 0 || ny >= sizeY || nz < 0 || nz >= sizeZ)
            {
                return true;
            }

            if (grid[nx, ny, nz] < groundLevel)
            {
                return true; //at least one neighbour is air
            }
        }
        return false;
    }
}
