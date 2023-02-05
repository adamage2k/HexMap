using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexChunk : MonoBehaviour
{
    Hexagon[] cells;

    HexMesh hexMesh;
    Canvas canvas;

    void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        cells = new Hexagon[HexData.chunkSizeX * HexData.chunkSizeZ];
    }

    void Start()
    {
        hexMesh.Triangulate(cells);
    }

    public void AddCell(int index, Hexagon cell) 
    {
        cells[index] = cell;
        cell.transform.SetParent(canvas.transform, false);
    }
}
