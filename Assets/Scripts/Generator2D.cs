using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Unity.Mathematics;

public class Generator2D : MonoBehaviour
{
    [Header("World settings")]
    public int width = 100;
    public int lenght = 100;
    [Range(0, 1)] public float groundLimit = 0.5f; // slider
    [Range(0, 1)] public float  sandLimit = 0.6f;
    

    [Header("Tiles")]
    public GameObject groundTile;
    public GameObject sandTile;
    public GameObject[] decorationTiles;

    [Header("Generator settings")]
    public float scale = 100;
    public float seed;

    private float[,] grid;

    void Start()
    {
        grid = new float[width, lenght];
        GenerateNoise();
        BuildWorld();
        Decorate();
    }

    void GenerateNoise()
    {
        seed = Random.Range(-1000, 1000);
        for (int z = 0; z < lenght; z++)
        {
            for(int x = 0;  x < width; x++)
            {
                var px = (x + seed) / scale;
                var pz = (z + seed) / scale;
                grid[x, z] = (noise.snoise(new float2(px, pz)) + 1) / 2;
            }
        }
    }

    void BuildWorld()
    {
        for(int z = 0; z < lenght; z++)
        {
            for(int x = 0; x < width; x++)
            {
                if (grid[x, z] >= groundLimit )
                {
                    if (grid[x, z] <= sandLimit)
                    {
                        Instantiate(sandTile, new Vector3(x, 0,z), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(groundTile, new Vector3(x, 0, z), Quaternion.identity);
                    }
                    
                }
            }
        }
    }

    void Decorate()
    {
        for (int i = 0; i < 100; i++)
        {
            var x = Random.Range(0, width);
            var z = Random.Range(0, lenght);
            var ray = new Ray(new Vector3(x, 10, z), Vector3.down);

            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                var tile = decorationTiles[Random.Range(0, decorationTiles.Length)];
                var rotationY = Random.Range(0, 360);
                Instantiate(tile, hit.point, Quaternion.Euler(0, rotationY, 0));
            }

        }
    }
}
