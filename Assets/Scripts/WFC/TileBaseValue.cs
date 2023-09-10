using UnityEngine.Tilemaps;
using WFC.Interfaces;

namespace WFC
{
    public class TileBaseValue : IValue<TileBase>
    {
        private TileBase _tileBase;

        public TileBaseValue(TileBase tileBase)
        {
            _tileBase = tileBase;
        }

        public TileBase Value => throw new System.NotImplementedException();

        public bool Equals(IValue<TileBase> x, IValue<TileBase> y)
        {
            throw new System.NotImplementedException();
        }

        public bool Equals(IValue<TileBase> other)
        {
            throw new System.NotImplementedException();
        }

        public int GetHashCode(IValue<TileBase> obj)
        {
            throw new System.NotImplementedException();
        }
    }
}