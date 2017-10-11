using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGrid : MonoBehaviour {

    public Slider cellSizeSlider;

    public float cellSize = 1;
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float yOffset = 0.5f;
    public Material cellMaterialValid;
    public Material cellMaterialInvalid;

    public Texture mapTexture;

    private GameObject[] _cells;
    private float[] _heights;

    void Start()
    {
        //Adds a listener to the main slider and invokes a method when the value changes.
        cellSizeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        cellSize = cellSizeSlider.value;

        mapTexture = (Texture2D)Resources.Load("maps/map");

        gridWidth = (int)(mapTexture.width/cellSize);
        gridHeight = (int)(mapTexture.height/cellSize);

        Debug.Log(gridWidth + ", " + gridHeight);

        _cells = new GameObject[gridHeight * gridWidth];
        _heights = new float[(gridHeight + 1) * (gridWidth + 1)];

        for (int z = 0; z < gridHeight; z++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                _cells[z * gridWidth + x] = CreateChild(x, z);
            }
        }
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        cellSize = cellSizeSlider.value;
    }

    void Update()
    {
        UpdateSize();
        //UpdatePosition();
        //UpdateHeights();
        UpdateCells();
    }

    GameObject CreateChild(int x, int z)
    {
        GameObject go = new GameObject();

        go.name = "Grid Cell";
        go.transform.parent = transform;
        go.transform.localPosition = new Vector3(-mapTexture.width/2 + x*cellSize, 0, -mapTexture.height/2 + z*cellSize);
        //go.transform.localPosition = Vector3.zero;
        go.AddComponent<MeshRenderer>();
        go.AddComponent<MeshFilter>().mesh = CreateMesh();

        return go;
    }

    void UpdateSize()
    {

        int newSize = gridHeight * gridWidth;
        int oldSize = _cells.Length;

        if (newSize == oldSize)
            return;

        GameObject[] oldCells = _cells;
        _cells = new GameObject[newSize];

        if (newSize < oldSize)
        {
            for (int i = 0; i < newSize; i++)
            {
                _cells[i] = oldCells[i];
            }

            for (int i = newSize; i < oldSize; i++)
            {
                Destroy(oldCells[i]);
            }
        }
        else if (newSize > oldSize)
        {
            for (int i = 0; i < oldSize; i++)
            {
                _cells[i] = oldCells[i];
            }

            for (int i = oldSize; i < newSize; i++)
            {
                _cells[i] = CreateChild(i % gridWidth, i / gridHeight);
            }
        }

        _heights = new float[(gridHeight + 1) * (gridWidth + 1)];
    }

    void UpdateCells()
    {
        for (int z = 0; z < gridHeight; z++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                GameObject cell = _cells[z * gridWidth + x];
                MeshRenderer meshRenderer = cell.GetComponent<MeshRenderer>();
                MeshFilter meshFilter = cell.GetComponent<MeshFilter>();

                meshRenderer.material = IsCellValid(x, z) ? cellMaterialValid : cellMaterialInvalid;
                UpdateMesh(meshFilter.mesh, x, z);
            }
        }
    }

    bool IsCellValid(int x, int z)
    {
        return true;
    }

    Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.name = "Grid Cell";
        mesh.vertices = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
        mesh.triangles = new int[] { 0, 1, 2, 2, 1, 3 };
        mesh.normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
        mesh.uv = new Vector2[] { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 0) };

        return mesh;
    }

    void UpdateMesh(Mesh mesh, int x, int z)
    {
        mesh.vertices = new Vector3[] {
            MeshVertex(x, z),
            MeshVertex(x, z + 1),
            MeshVertex(x + 1, z),
            MeshVertex(x + 1, z + 1),
        };
    }

    Vector3 MeshVertex(int x, int z)
    {
        return new Vector3(x * cellSize, _heights[z * (gridWidth + 1) + x] + yOffset, z * cellSize);
    }
}
