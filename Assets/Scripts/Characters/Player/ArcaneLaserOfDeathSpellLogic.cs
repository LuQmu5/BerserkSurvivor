using System.Collections;
using UnityEngine;

public class ArcaneLaserOfDeathSpellLogic : SpellView
{
    private Transform _startPoint;
    private Vector3 _endPoint;

    // #config

    private float _laserDuration = 0.25f;
    private float _laserDistance = 10f;
    private float _hitDamage = 10;
    private float _afterDamage = 3;

    public override void Init(ICaster caster, CharacterStats stats)
    {
        _startPoint = caster.CastPoint;

        _endPoint = _startPoint.position + caster.Transform.forward * _laserDistance;

        transform.position = _startPoint.position;
        transform.LookAt(_endPoint);
        gameObject.SetActive(true);

        StartCoroutine(MoveLaser());
    }

    private IEnumerator MoveLaser()
    {
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = _endPoint;

        while (elapsedTime < _laserDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / _laserDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IHealth healthActors))
        {
            healthActors.ApplyDamage(_afterDamage);
            print(healthActors.CurrentHealth);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IHealth healthActors))
        {
            healthActors.ApplyDamage(_hitDamage);
            print(healthActors.CurrentHealth);
        }
    }
}
