using System;
using UnityEngine.Tilemaps;
using WCF.Helpers;
using WFC.Interfaces;

namespace WFC
{
    public class InputReader : IInputReader<TileBase>
    {
        private Tilemap _inputTilemap;

        public InputReader(Tilemap input)
        {
            _inputTilemap = input;
        }

        public IValue<TileBase>[][] ReadInputToGrid()
        {
            var grid = ReadInputTilemap();

            TileBaseValue[][] gridOfValues = null;

            if(grid != null)
            {
                gridOfValues = MyCollectionExtension.CreateJaggedArray<TileBaseValue[][]>(grid.Length, grid[0].Length);

                for (int row = 0; row < grid.Length; row++)
                {
                    for (int col = 0; col < grid[0].Length; col++)
                    {
                        gridOfValues[row][col] = new TileBaseValue(grid[row][col]);
                    }
                }
            }

            return gridOfValues;
        }

        private TileBase[][] ReadInputTilemap()
        {
            throw new NotImplementedException();
        }
    }
}

