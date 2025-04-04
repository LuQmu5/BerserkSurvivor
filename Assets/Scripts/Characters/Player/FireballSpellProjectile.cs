using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;

public class FireballSpellProjectile : SpellProjectile
{
    private ICaster _caster;
    private Vector3 _direction;
    private float _speed = 10f; // Скорость полета фаербола
    private float _damage = 1f; // Урон
    private float _explosionRadius = 2f; // Радиус взрыва
    private float _lifeTime = 2f; // Время жизни фаербола
    private Vector3 _baseScale;

    private Rigidbody _rigidbody; // Для физического движения

    private void Start()
    {
        _baseScale = transform.localScale;
        _rigidbody = GetComponent<Rigidbody>(); // Инициализация Rigidbody
    }

    public override void Init(ICaster caster)
    {
        _caster = caster;
        _direction = caster.Transform.forward;
        transform.position = _caster.CastPoint.position;
        transform.forward = _caster.CastPoint.forward;

        gameObject.SetActive(true);
        ResetRigidbody();
        transform.localScale = _baseScale;

        _lifeTime = 2f;

        Creating();

        // Запускаем корутину после активации
        StartCoroutine(LifeTimeCoroutine());
    }

    private void Creating()
    {
        float spawnDuration = 0.2f;
        transform.localScale /= 2;

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, spawnDuration)
              .SetEase(Ease.OutBack)
              .OnComplete(() =>
              {
                  Fly();
              });
    }

    // Запуск полета фаербола
    public void Fly()
    {
        // Даем начальный импульс фаерболу через Rigidbody
        _rigidbody.linearVelocity = _direction * _speed;
    }

    // Coroutine для отслеживания времени жизни
    private System.Collections.IEnumerator LifeTimeCoroutine()
    {
        // Проводим проверку на активность объекта в цикле
        while (_lifeTime > 0 && gameObject.activeSelf)
        {
            _lifeTime -= Time.deltaTime;
            yield return null;
        }

        // Завершаем фаербол по истечении времени жизни
        if (gameObject.activeSelf)
        {
            DOTween.Kill(gameObject, true);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ICaster caster) && caster != _caster) // #config         
            return;

        Explode();
    }

    private void Explode()
    {
        // Радиус взрыва
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

        DOTween.Kill(gameObject, true);
        gameObject.SetActive(false);
    }

    // Этот метод будет сбрасывать скорость Rigidbody при возврате в пул
    public void ResetRigidbody()
    {
        if (_rigidbody != null)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero; // Если требуется сбросить угловую скорость
        }
    }
}
