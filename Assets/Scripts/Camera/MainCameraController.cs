using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    [SerializeField] private Vector3 _baseOffset = new Vector3(0, 15, -8);
    [SerializeField] private Vector3 _baseEuler = new Vector3(60, 0, 0);
    [SerializeField] private Transform _target;

    private void Start()
    {
        StartCoroutine(Following(_target));
    }

    private IEnumerator Following(Transform target)
    {
        while (true)
        {
            transform.position = target.position + _baseOffset;

            yield return null;
        }
    }
}
