using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    public int chunkCountX = 4, chunkCountZ = 3;

    int cellCountX, cellCountZ;

    public Color deafultColor = Color.gray;
    public Color waterColor = Color.blue;
    public Color sandColor = Color.yellow;
    public Color groundColor = Color.green;

    public Hexagon cellPrefab;
    public Text cellInfoPrefab;
    public HexChunk chunkPrefab;
    public Canvas infoCanvas;
    public Text infoCanvasTextColor;
    public Text infoCanvasTextCoords;

    Hexagon[] cells;
    HexChunk[] chunks;


    void Awake()
    {
        cellCountX = chunkCountX * HexData.chunkSizeX;
        cellCountZ = chunkCountZ * HexData.chunkSizeZ;

        CreateChunks();
        CreateCells();
    }

    void CreateChunks()
    {
        chunks = new HexChunk[chunkCountX * chunkCountZ];

        for (int z = 0, i = 0; z < chunkCountZ; z++)
        {
            for (int x = 0; x < chunkCountX; x++)
            {
                HexChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
                chunk.transform.SetParent(transform);
            }
        }
    }

    void CreateCells()
    {
        cells = new Hexagon[cellCountX * cellCountZ];
        for (int z = 0, i = 0; z < cellCountZ; z++)
        {
            for (int x = 0; x < cellCountX; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            HandleInput();
        }


        if (Input.GetKey(KeyCode.Escape)) 
        {
            infoCanvas.gameObject.SetActive(false);
        }


    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            TouchCell(hit.point);
        }
    }

    void TouchCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
        Hexagon cell = cells[index];
        string cellColor;
        if (cell.color == Color.gray)
        {
            cellColor = "Gray";
        }
        else if (cell.color == Color.yellow)
        {
            cellColor = "Yellow";
            infoCanvas.gameObject.SetActive(true);
            infoCanvasTextColor.text = "Color: " + cellColor;
            infoCanvasTextCoords.text = "Coords: " + coordinates.ToString();
        }
        else if (cell.color == Color.green)
        {
            cellColor = "Green";
            infoCanvas.gameObject.SetActive(true);
            infoCanvasTextColor.text = "Color: " + cellColor;
            infoCanvasTextCoords.text = "Coords: " + coordinates.ToString();
        }
        else 
        {
            cellColor = "Blue";
        }
        Debug.Log("Touched at " + coordinates.ToString() + "Color is: " + cellColor);
    }

    void CreateCell(int x, int z, int i)
    {
        float randomValue = UnityEngine.Random.Range(0, 100);

        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexData.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexData.outerRadius * 1.5f);

        Hexagon cell = cells[i] = Instantiate<Hexagon>(cellPrefab);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        if (randomValue < 100 && randomValue >= 40)
        {
            cell.color = waterColor;
        }
        else if (randomValue < 15 && randomValue >= 5)
        {
            cell.color = groundColor;
        }
        else if (randomValue < 40 && randomValue >= 15)
        {
            cell.color = deafultColor;
        }
        else
        {
            cell.color = sandColor;
        }

        //Text label = Instantiate<Text>(cellInfoPrefab);
        //label.rectTransform.SetParent(gridCanvas.transform, false);
        //label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        //label.text = cell.coordinates.ToStringOnSeparateLines();

        AddCellToChunk(x, z, cell);
    }

    void AddCellToChunk(int x, int z, Hexagon cell) 
    {
        int chunkX = x / HexData.chunkSizeX;
        int chunkZ = z / HexData.chunkSizeZ;
        HexChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

        int localX = x - chunkX * HexData.chunkSizeX;
        int localZ = z- chunkZ * HexData.chunkSizeZ;
        chunk.AddCell(localX + localZ * HexData.chunkSizeX, cell);
    }
}
