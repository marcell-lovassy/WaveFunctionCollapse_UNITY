using System;
using System.Collections.Generic;

namespace WFC.Interfaces
{
    public interface IValue<T> : IEqualityComparer<IValue<T>>, IEquatable<IValue<T>>
    {
        T Value { get; }
    }
}