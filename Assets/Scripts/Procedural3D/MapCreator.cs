using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Tracing;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public enum CellDirection
{
    Left, 
    Right, 
    Up, 
    Down,
    None
}

public class Cell
{
    public bool IsClosed = false;
    public Vector2Int position;
    public List<int> possibleKeys;

    public Cell(int x, int y, IEnumerable<int> keys)
    {
        position = new Vector2Int(x, y);
        possibleKeys = keys.ToList();
    }

    public CellDirection GetOthercCellRelativeDirection(Cell other)
    {
        if (other.position.x == position.x && other.position.y == position.y) return CellDirection.None;

        if(other.position.x == position.x)
        {
            return other.position.y < position.y ? CellDirection.Down : CellDirection.Up;
        }
        else
        {
            return other.position.x < position.x ? CellDirection.Left : CellDirection.Right;
        }
    }
}

public class MapCreator : MonoBehaviour
{
    [SerializeField]
    private MapDetailsSO mapDetails;

    List<Cell> closedCells = new List<Cell>();

    Cell[,] map;

    private void Start()
    {
        map = new Cell[mapDetails.maxMapTileCount, mapDetails.maxMapTileCount];
        InitEmptyMap();
        ExecuteWFC();

        StartCoroutine(CreateMap());
    }

    private void InitEmptyMap()
    {
        for (int x = 0; x < mapDetails.maxMapTileCount; x++)
        {
            for (int y = 0; y < mapDetails.maxMapTileCount; y++)
            {
                //if(x == 0 && y == 0)
                //{
                //    map[x, y] = new Cell(x, y, new int[] { 5 });
                //    continue;
                //}
                //TODO: get the keys from the mapDetails;
                map[x, y] = new Cell(x, y, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
            }
        }
    }

    private void ExecuteWFC()
    {
        Queue<Cell> openCells = new Queue<Cell>();

        //select a random cell
        var randomStartPosition = new Vector2Int(13, 13);///*GetRandomPositionOnMap();*/
        var selectedCell = map[randomStartPosition.x, randomStartPosition.y];

        openCells.Enqueue(selectedCell);
        int safetyCounter = 0;
        while (openCells.Count > 0 && safetyCounter < 900)
        {
            var cellToCollapse = openCells.Dequeue();
            CollapseCell(cellToCollapse);
            if(cellToCollapse.possibleKeys.FirstOrDefault() == -1)
            {
                continue;
            }
            List<Cell> neighbours = new List<Cell>();
            PropagateChanges(cellToCollapse, ref neighbours);
            foreach (var n in neighbours)
            {
                if (!openCells.Contains(n) && !n.IsClosed)
                {
                    openCells.Enqueue(n);
                }
            }
            safetyCounter++;
        }
    }

    private void CollapseCell(Cell cellToCollapse)
    {
        var randomCellType = UnityEngine.Random.Range(0, cellToCollapse.possibleKeys.Count);
        if(cellToCollapse.possibleKeys.Count == 0)
        {
            //mark the cell as empty
            cellToCollapse.possibleKeys.Add(-1);
            return;
        }
        var selectedType = cellToCollapse.possibleKeys[randomCellType];
        cellToCollapse.possibleKeys.Clear();
        cellToCollapse.possibleKeys.Add(selectedType);
        cellToCollapse.IsClosed = true;
        closedCells.Add(cellToCollapse);
    }
    
    private List<Cell> PropagateChanges(Cell centerCell, ref List<Cell> neighbours)
    {
        var tileRules = mapDetails.mapTiles.FirstOrDefault(tile => tile.key == centerCell.possibleKeys[0]);
        neighbours.AddRange(GetNeighbours(centerCell));
        foreach (var neighbourCell in neighbours)
        {
            if(centerCell.GetOthercCellRelativeDirection(neighbourCell) == CellDirection.Left)
            {
                //if (neighbourCell.possibleKeys.Count == 1) continue;
                neighbourCell.possibleKeys = neighbourCell.possibleKeys.Intersect(tileRules.Left).ToList();
            }
            else if (centerCell.GetOthercCellRelativeDirection(neighbourCell) == CellDirection.Right)
            {
                //if (neighbourCell.possibleKeys.Count == 1) continue;
                neighbourCell.possibleKeys = neighbourCell.possibleKeys.Intersect(tileRules.Right).ToList();
            }
            else if (centerCell.GetOthercCellRelativeDirection(neighbourCell) == CellDirection.Up)
            {
                //if (neighbourCell.possibleKeys.Count == 1) continue;
                neighbourCell.possibleKeys = neighbourCell.possibleKeys.Intersect(tileRules.Up).ToList();
            }
            else if (centerCell.GetOthercCellRelativeDirection(neighbourCell) == CellDirection.Down)
            {
                //if (neighbourCell.possibleKeys.Count == 1) continue;
                neighbourCell.possibleKeys = neighbourCell.possibleKeys.Intersect(tileRules.Down).ToList();
            }
        }
        return neighbours.ToList();
    }

    private IEnumerator CreateMap()
    {
        foreach (var cell in closedCells)
        {
            Instantiate(mapDetails.mapTiles.FirstOrDefault(t => t.key == cell.possibleKeys.FirstOrDefault()).tile, new Vector3(cell.position.x, 0, cell.position.y), Quaternion.identity);
            yield return null;
        }

        //for (int i = 0; i < mapDetails.maxMapTileCount; i++)
        //{
        //    for (int j = 0; j < mapDetails.maxMapTileCount; j++)
        //    {
        //        if (map[i, j].possibleKeys.FirstOrDefault() == -1) continue;
        //        Instantiate(mapDetails.mapTiles.FirstOrDefault(t => t.key == map[i,j].possibleKeys.FirstOrDefault()).tile, new Vector3(i, 0, j), Quaternion.identity);
        //        yield return null;//new WaitForSeconds(0.025f);
        //    }
        //}
    }

    private List<Cell> GetNeighbours(Cell c)
    {
        List<Cell> neighbours = new List<Cell>();

        if(c.position.x + 1 < mapDetails.maxMapTileCount) neighbours.Add(map[c.position.x + 1, c.position.y]);
        if(c.position.x - 1 >= 0) neighbours.Add(map[c.position.x - 1, c.position.y]);
        if(c.position.y + 1 < mapDetails.maxMapTileCount) neighbours.Add(map[c.position.x, c.position.y + 1]);
        if(c.position.y - 1 >= 0) neighbours.Add(map[c.position.x, c.position.y - 1]);

        return neighbours;
    }

    private Vector2Int GetRandomPositionOnMap()
    {
        return new Vector2Int(UnityEngine.Random.Range(0, mapDetails.maxMapTileCount), UnityEngine.Random.Range(0, mapDetails.maxMapTileCount));
    }

    private Cell GetLowestEntropyCell()
    {
        int lowestEntropy = 9;
        Cell nextCell = null;
        foreach (var cell in map)
        {
            if(cell.possibleKeys.Count != 1 && cell.possibleKeys.Count < lowestEntropy)
            {
                lowestEntropy = cell.possibleKeys.Count;
                nextCell = cell;
            }
        }
        return nextCell;    
    }
}
