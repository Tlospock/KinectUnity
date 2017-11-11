using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRange : MonoBehaviour
{

    // Update is called once per frame
    public int rangeInTiles;
    public GameObject tile;
    public List<GameObject> tilesDisplayed;
    public Transform tileSpawn;

    private static int minX = -2, maxX = 8, minY = -2, maxY = 8;

    private bool[,] reachable;

    private void Start()
    {
        tilesDisplayed = new List<GameObject>();
        reachable = new bool[2 * rangeInTiles + 1, 2 * rangeInTiles + 1];

        ArrangeReachArray(ref reachable, 0, 0);
        reachable[rangeInTiles, rangeInTiles] = false; //Start point
    }

    private void ArrangeReachArray(ref bool[,] array, int x, int y)
    {
        if (x < 0 || x >= array.GetLength(0) ||
            y < 0 || y >= array.GetLength(1))
            return;

        int distanceToOrigin = Mathf.Abs(rangeInTiles - x) + Mathf.Abs(rangeInTiles - y);
        if (rangeInTiles >= distanceToOrigin)
            array[x, y] = true;


        ArrangeReachArray(ref array, x + 1, y);
        ArrangeReachArray(ref array, x, y + 1);
    }

    private void OnMouseDown()
    {
        GameObject offsetTile = new GameObject();
        offsetTile.transform.rotation = tileSpawn.rotation;
        offsetTile.transform.localScale = tileSpawn.localScale;

        for (int i = 0; i < reachable.GetLength(0); i++)
        {
            for (int j = 0; j < reachable.GetLength(1); j++)
            {
                if (reachable[i, j])
                {
                    Debug.Log(string.Format("Creating tile on [{0}, {1}]", i, j));
                    offsetTile.transform.position = tileSpawn.position + new Vector3(i - rangeInTiles, j - rangeInTiles);
                    if (offsetTile.transform.position.x < minX || maxX < offsetTile.transform.position.x ||
                       offsetTile.transform.position.y < minY || maxY < offsetTile.transform.position.y)
                        continue;   //Out of map
                    tilesDisplayed.Add(Instantiate(tile, offsetTile.transform.position, offsetTile.transform.rotation) as GameObject);
                }
            }
        }

    }

    private void OnMouseDrag()
    {

    }

    private void OnMouseUp()
    {
        foreach (GameObject tile in tilesDisplayed)
        {
            Destroy(tile);
        }
        tilesDisplayed.Clear();
    }
}
