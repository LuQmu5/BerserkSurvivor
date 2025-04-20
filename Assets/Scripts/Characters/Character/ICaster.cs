using UnityEngine;

public interface ICaster
{
    public Transform Transform { get; }
    public Transform CastPoint { get; }

    public void Cast();
}