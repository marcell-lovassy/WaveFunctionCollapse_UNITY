using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public List<int> Left 
    {
        get
        {
            if (leftList != null && leftList.Count > 0) return leftList;
            leftList = new List<int>();
            if (!string.IsNullOrEmpty(left))
            {
                var splittedString = left.Split(',').ToList();
                foreach (var key in splittedString)
                {
                    leftList.Add(int.Parse(key));
                }
            }
            return leftList;
        }
    }

    public List<int> Right
    {
        get
        {
            if (rightList != null && rightList.Count > 0) return rightList;
            rightList = new List<int>();
            if (!string.IsNullOrEmpty(right))
            {
                var splittedString = right.Split(',').ToList();
                foreach (var key in splittedString)
                {
                    rightList.Add(int.Parse(key));
                }
            }
            return rightList;
        }
    }

    public List<int> Up
    {
        get
        {
            if (upList != null && upList.Count > 0) return upList;
            upList = new List<int>();
            if (!string.IsNullOrEmpty(up))
            {
                var splittedString = up.Split(',').ToList();
                foreach (var key in splittedString)
                {
                    upList.Add(int.Parse(key));
                }
            }
            return upList;
        }
    }

    public List<int> Down
    {
        get
        {
            if (downList != null && downList.Count > 0) return downList;
            downList = new List<int>();

            if (!string.IsNullOrEmpty(down))
            {
                var splittedString = down.Split(',').ToList();
                foreach (var key in splittedString)
                {
                    downList.Add(int.Parse(key));
                }
            }
            return downList;
        }
    }

    public int key;
    public GameObject tile;
    public string left;
    public string right;
    public string down;
    public string up;

    private List<int> leftList;
    private List<int> rightList;
    private List<int> upList;
    private List<int> downList;
}