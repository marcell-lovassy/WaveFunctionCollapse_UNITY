using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    public MapDetailsSO mapDetails;
    Dictionary<Vector3, TileAndRules> InstantiatedTiles = new Dictionary<Vector3, TileAndRules>();
    int[,][] map;

    private void Start()
    {
        map = new int[mapDetails.maxMapTileCount, mapDetails.maxMapTileCount][];
        InitEmptyMap();
        EvaluateInitialMap();
        StartCoroutine(CreateMap());
    }
    private void InitEmptyMap()
    {
        for (int i = 0; i < mapDetails.maxMapTileCount; i++)
        {
            for (int j = 0; j < mapDetails.maxMapTileCount; j++)
            {
                if (i == 0 && j == 0)
                {
                    map[i, j] = new int[] { 5 };
                    continue;
                }
                map[i, j] = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            }
        }
    }

    private void EvaluateInitialMap()
    {
        UpdatePossibleTilesAtNeighbours(0, 0, map[0, 0]);
    }

    private void UpdatePossibleTilesAtNeighbours(int i, int j, int[] possibleTiles)
    {
        Vector2Int top = new Vector2Int (i, j + 1);
        Vector2Int bottom = new Vector2Int(i, j - 1);
        Vector2Int left = new Vector2Int(i - 1, j);
        Vector2Int right = new Vector2Int(i + 1, j);

        if(top.y < mapDetails.maxMapTileCount)
        {
            var possibleList = GetPossibleTopTiles(possibleTiles);
            for (int k = 0; k < map[top.x, top.y].Length; k++)
            {
                if(!possibleList.Contains(map[top.x, top.y][k]))
                {
                    possibleList.Remove(map[top.x, top.y][k]);
                }
            }
            map[top.x, top.y] = possibleList.ToArray();
        }
    }

    private List<int> GetPossibleTopTiles(int[] possibleTiles)
    {
        List<int> possibleTilesList = new List<int>();

        foreach (int tile in possibleTiles)
        {
            var rules = mapDetails.mapTiles.FirstOrDefault(t => t.key == tile);
            foreach(var possibleKey in rules.keysOfValidNeighboursTop)
            {
                possibleTilesList.Add(possibleKey);
            }
        }

        return possibleTilesList;
    }

  

    private IEnumerator CreateMap()
    {
        for (int i = 0; i < mapDetails.maxMapTileCount; i++)
        {
            for (int j = 0; j < mapDetails.maxMapTileCount; j++)
            {
                Instantiate(mapDetails.mapTiles.FirstOrDefault(t => t.key == map[i,j].FirstOrDefault()).tile, new Vector3(i, 0, j), Quaternion.identity);
                yield return new WaitForSeconds(0.025f);
            }
        }

        //var mapTiles = new List<TileAndRules>(mapDetails.mapTiles);
        //foreach (var tile in mapTiles)
        //{
        //    var tileInstance = Instantiate(tile.tile, Vector3.zero, Quaternion.identity);
        //    InstantiatedTiles[tileInstance.transform.position] = tile;
        //}
    }
}
