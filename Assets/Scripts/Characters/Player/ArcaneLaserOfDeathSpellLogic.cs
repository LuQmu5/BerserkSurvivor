using System.Collections;
using UnityEngine;

public class ArcaneLaserOfDeathSpellLogic : SpellView
{
    private Transform _startPoint;
    private Vector3 _endPoint;

    private float _laserDuration = 1f;
    private float _laserDistance = 10f;

    public override void Init(ICaster caster)
    {
        _startPoint = caster.CastPoint;
        _endPoint = _startPoint.position + caster.Transform.forward * _laserDistance;

        transform.position = _startPoint.position;
        transform.LookAt(_endPoint);

        gameObject.SetActive(true);

        StartCoroutine(LaserLifeCycle());
    }

    private IEnumerator LaserLifeCycle()
    {
        yield return new WaitForSeconds(_laserDuration);

        gameObject.SetActive(false);
    }
}
