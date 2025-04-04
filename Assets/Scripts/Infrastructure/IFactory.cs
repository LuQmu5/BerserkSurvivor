
using System;

public interface IFactory<T>
{
    public static IFactory<T> Instance { get; }
    T GetItem(Enum type);
}
