using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class MainCameraController : MonoBehaviour
{
    [SerializeField] private Vector3 _baseOffset = new Vector3(0, 15, -8);
    [SerializeField] private Vector3 _baseEuler = new Vector3(60, 0, 0);
    [SerializeField] private Transform _target;
    [SerializeField] private LayerMask _obstacleMask;

    private Transform[] _obstacles;
    private int _oldHitsNumber = 0;

    private void Start()
    {
        StartCoroutine(Following(_target));
    }

    private void LateUpdate()
    {
        // XRay();
    }

    private void XRay()
    {
        float characterDistance = Vector3.Distance(transform.position, _target.position);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, _target.position - transform.position, characterDistance, _obstacleMask);

        if (hits.Length > 0)
        {
            int newHits = hits.Length - _oldHitsNumber;

            if (_obstacles != null && _obstacles.Length > 0 && newHits < 0)
            {
                // Repaint all the previous obstructions. Because some of the stuff might be not blocking anymore
                for (int i = 0; i < _obstacles.Length; i++)
                {
                    MeshRenderer renderer = _obstacles[i].gameObject.GetComponent<MeshRenderer>();
                    Color currentColor = renderer.material.color;
                    renderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1);
                }
            }

            _obstacles = new Transform[hits.Length];

            // Hide the current obstructions 
            for (int i = 0; i < hits.Length; i++)
            {
                Transform obstacle = hits[i].transform;
                MeshRenderer renderer = obstacle.gameObject.GetComponent<MeshRenderer>();
                Color currentColor = renderer.material.color;
                renderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.2f);

                _obstacles[i] = obstacle;
            }

            _oldHitsNumber = hits.Length;
        }
        else
        {   // Mean that no more stuff is blocking the view and sometimes all the stuff is not blocking as the same time
            if (_obstacles != null && _obstacles.Length > 0)
            {
                for (int i = 0; i < _obstacles.Length; i++)
                {
                    MeshRenderer renderer = _obstacles[i].gameObject.GetComponent<MeshRenderer>();
                    Color currentColor = renderer.material.color;
                    renderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1);
                }

                _oldHitsNumber = 0;
                _obstacles = null;
            }
        }
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
