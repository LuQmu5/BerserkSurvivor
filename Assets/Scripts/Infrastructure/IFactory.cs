
using System;

public interface IFactory<T>
{
    T GetItem(Enum type);
}
