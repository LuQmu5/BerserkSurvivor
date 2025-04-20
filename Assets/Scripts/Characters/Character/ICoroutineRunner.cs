using System.Collections;
using UnityEngine;

public interface ICoroutineRunner
{
    public Coroutine StartCoroutine(IEnumerator routineName);
    public void StopCoroutine(Coroutine coroutine);
}
