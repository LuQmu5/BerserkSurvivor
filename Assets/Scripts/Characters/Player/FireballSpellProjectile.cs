using DG.Tweening;
using System;
using UnityEngine;

public class FireballSpellProjectile : SpellProjectile
{
    private ICaster _caster;
    private Vector3 _direction;
    private float _speed = 1;
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
        if (other.TryGetComponent(out ICaster caster))
            if (caster != _caster)
                Explode();
    }

    private void Explode()
    {
        Debug.Log("BABAH!");
        gameObject.SetActive(false); 
    }
}
