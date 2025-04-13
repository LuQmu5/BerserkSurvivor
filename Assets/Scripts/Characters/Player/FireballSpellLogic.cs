using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class FireballSpellLogic : SpellView
{
    [SerializeField] private GameObject _explosionVFX;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;

    private ICaster _caster;

    private Vector3 _baseScale = Vector3.one / 2;
    private Vector3 _direction;

    // # config
    private float _damage = 2f;
    private float _spawnDuration = 0.3f;

    private float _baseSpeed = 15f;
    private float _currentSpeed = 10f;

    private float _explosionRadius = 3f;
    private float _noExplosionDelay = 0.05f;

    private float _lifeTime = 2;
    private float _currenLlifeTime = 2f;

    private float _acceleration = 3f;
    private float _currentAcceleration = 2f;

    public float StartAccelerationTime => _lifeTime - _noExplosionDelay - _spawnDuration;

    public override void Init(ICaster caster)
    {
        ResetRigidbody();
        DOTween.Kill(gameObject, true);

        _caster = caster;
        _direction = caster.Transform.forward;

        transform.forward = caster.Transform.forward;
        transform.position = _caster.CastPoint.position;
        transform.localScale = _baseScale;

        _currentSpeed = _baseSpeed;
        _currentAcceleration = _acceleration;
        _currenLlifeTime = _lifeTime;

        Creating();
        StartCoroutine(LifeTimeCoroutine());

        gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (_currenLlifeTime > StartAccelerationTime || _currenLlifeTime <= 0)
            return;

        _currentSpeed += _currentAcceleration * Time.fixedDeltaTime;
        _rigidbody.linearVelocity = _direction * _currentSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsProjectileReady() == false)
            return;

        Explode();
    }

    private bool IsProjectileReady()
    {
        return _currenLlifeTime < _lifeTime - _noExplosionDelay && _currenLlifeTime >= 0;
    }

    private void Creating()
    {
        transform.localScale = Vector3.zero;

        transform.DOScale(_baseScale, _spawnDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(Fly);
    }

    private void Fly()
    {
        _collider.enabled = true;
        _rigidbody.linearVelocity = _direction * _currentSpeed;
    }

    private IEnumerator LifeTimeCoroutine()
    {
        while (_currenLlifeTime > 0 && gameObject.activeSelf)
        {
            _currenLlifeTime -= Time.deltaTime;
            yield return null;
        }

        if (gameObject.activeSelf)
        {
            DOTween.Kill(gameObject, true);
            gameObject.SetActive(false);
        }
    }

    private void Explode()
    {
        IHealth[] healthObjects = GetClosesHealthActors();

        Debug.Log("Actors: " + healthObjects.Length);

        foreach (IHealth health in healthObjects)
        {
            float distanceToExplosion = Vector3.Distance(transform.position, health.Transform.position);
            float damageFactor = Mathf.Clamp01(1 - distanceToExplosion / _explosionRadius); // Расчет урона по дистанции
            float bonusDamageFactor = 20; // #config
            float finalDamage = _damage * (bonusDamageFactor + damageFactor); // Урон увеличивается в зависимости от того, насколько близко цель

            health.ApplyDamage(finalDamage);
            Debug.Log(health.CurrentHealth);
        }

        GameObject explosion = Instantiate(_explosionVFX, transform.position - Vector3.down / 2, Quaternion.identity);
        float explosionLifeTime = 0.25f;
        Destroy(explosion, explosionLifeTime);

        DOTween.Kill(gameObject, true);
        gameObject.SetActive(false);
    }

    private IHealth[] GetClosesHealthActors()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
        IHealth[] healthObjects = colliders
            .Select(c => c.GetComponent<IHealth>())
            .Where(health => health != null)
            .ToArray();
        return healthObjects;
    }

    private void ResetRigidbody()
    {
        _currentAcceleration = _acceleration;

        if (_collider != null)
            _collider.enabled = false;

        if (_rigidbody == null)
            return;

        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
}
