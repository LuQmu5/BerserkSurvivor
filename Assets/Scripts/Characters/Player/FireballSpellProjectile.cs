using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireballSpellProjectile : SpellProjectile
{
    private ICaster _caster;
    private Vector3 _direction;
    private float _speed = 1;
    private float _damage = 1; // #config
    private float _explosionRadius = 2; // #config
    private Vector3 _baseScale;

    private void Start()
    {
        _baseScale = transform.localScale;
    }

    public override void Init(ICaster caster)
    {
        _caster = caster;
        _direction = caster.Transform.forward;
        transform.position = _caster.CastPoint.position;
        gameObject.SetActive(true);
        transform.localScale = _baseScale;

        Creating();
    }

    private void Creating()
    {
        float spawnDuration = 0.2f;
        float liveTime = 2f;
        transform.localScale /= 2;

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, spawnDuration)
              .SetEase(Ease.OutBack)
              .OnComplete(() =>
              {
                  Fly();
              });

        Destroy(gameObject, liveTime);
    }

    public void Fly()
    {
        float moveDelay = 0.2f;
        float distance = 50f;

        transform.DOMove(transform.position + _direction * _speed, moveDelay) // Маленький рывок для "заряда"
                         .SetEase(Ease.InOutQuad)
                         .OnComplete(() =>
                         {
                             transform.DOMove(transform.position + _direction * _speed * distance, distance / _speed)
                                      .SetEase(Ease.OutExpo);
                         });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ICaster caster) && caster != _caster) // #config         
            return;

        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
        IHealth[] healthObjects = colliders
            .Select(c => c.GetComponent<IHealth>())
            .Where(health => health != null)
            .ToArray();

        foreach (IHealth health in healthObjects)
        {
            if (health is ICaster caster)
                if (caster == _caster)
                    continue; // # config # BUG

            health.ApplyDamage(_damage);
        }

        Debug.Log($"Babah na {colliders.Length} units");

        DOTween.Kill(gameObject);
        Destroy(gameObject); // factory
    }
}
