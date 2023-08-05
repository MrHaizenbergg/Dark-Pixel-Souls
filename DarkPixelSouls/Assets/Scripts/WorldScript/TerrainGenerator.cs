using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject Cell;
    public Transform Zero;
    public int Heith, Width;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        int y = 0;

        for (int x = 0; x < Width; x++)
        {
            var cell = Instantiate(Cell, Zero);
            if (x % 2 == 0)
            {
                y += Random.Range(-1, 2);
            }

            cell.transform.localPosition = new Vector3(x, y, 0);
        }
    }
}