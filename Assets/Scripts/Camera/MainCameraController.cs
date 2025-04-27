using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject.SpaceFighter;
using static UnityEngine.GraphicsBuffer;

public class MainCameraController : MonoBehaviour
{
    [SerializeField] private Vector3 _baseOffset = new Vector3(0, 15, -8);
    [SerializeField] private Vector3 _baseEuler = new Vector3(60, 0, 0);
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private Transform _target;

    private List<MeshRenderer> _obstacles = new List<MeshRenderer>();

    public void Init(Transform target)
    {
        _target = target;
        StartCoroutine(Following(_target));
    }

    private void Start()
    {
        if (_target != null)
            StartCoroutine(Following(_target));
    }

    private void FixedUpdate()
    {
        if (CheckWalls(out RaycastHit[] hits))
        {
            XRay(hits);
        }
        else if (_obstacles.Count != 0)
        {
            ClearObstacles();
        }
    }

    private void ClearObstacles()
    {
        foreach (var obstacle in _obstacles)
        {
            SetAlpha(obstacle, 1);
        }

        _obstacles.Clear();
    }

    private void XRay(RaycastHit[] hits)
    {
        List<MeshRenderer> temp = new List<MeshRenderer>();

        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out MeshRenderer renderer))
            {
                temp.Add(renderer);
                SetAlpha(renderer, 0.2f);
            }
        }

        var oldObstacles = _obstacles.Where(i => temp.Contains(i) == false);

        foreach (var obstacle in oldObstacles)
        {
            SetAlpha(obstacle, 1);
        }

        _obstacles = temp;
    }

    private static void SetAlpha(MeshRenderer renderer, float alpha)
    {
        Color currentColor = renderer.material.color;
        renderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
    }

    private bool CheckWalls(out RaycastHit[] hits)
    {
        float characterDistance = Vector3.Distance(transform.position, _target.position);
        Vector3 direction = _target.position - transform.position;
        hits = Physics.RaycastAll(transform.position, direction, characterDistance, _obstacleMask);
        // hits = Physics.BoxCastAll(transform.position, Vector3.back * 3, direction, Quaternion.Euler(0, 0, 0), characterDistance);

        return hits.Length > 0;
    }


    /// <summary>
    /// XRAY old
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    /*
    private void XRay()
    {
        float characterDistance = Vector3.Distance(transform.position, _target.position);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, _target.position - transform.position, characterDistance, _obstacleMask);

        if (hits.Length > 0)
        {
            int newHits = hits.Length - _oldHitsNumber;

            if (_obstacles != null && _obstacles.Length > 0 && newHits < 0)
            {
                for (int i = 0; i < _obstacles.Length; i++)
                {
                    MeshRenderer renderer = _obstacles[i].gameObject.GetComponent<MeshRenderer>();
                    Color currentColor = renderer.material.color;
                    renderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1);
                }
            }

            _obstacles = new Transform[hits.Length];

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
        {
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
    */

    private IEnumerator Following(Transform target)
    {
        while (true)
        {
            transform.position = target.position + _baseOffset;

            yield return null;
        }
    }
}
