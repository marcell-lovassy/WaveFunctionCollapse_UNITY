using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map_", menuName = "MapGenerator/Create Map Details")]
public class MapDetailsSO : ScriptableObject
{
    public string mapName;
    public int maxMapTileCount;
    public int minMapTileCount;
    public TileAndRules[] mapTiles;
}

[Serializable]
public class TileAndRules
{
    public int key;
    public GameObject tile;
    public int[] keysOfValidNeighboursLeft;
    public int[] keysOfValidNeighboursRight;
    public int[] keysOfValidNeighboursBottom;
    public int[] keysOfValidNeighboursTop;
}

[Serializable]
public class TileData
{
    public GameObject tile;
    public float probability;
}
