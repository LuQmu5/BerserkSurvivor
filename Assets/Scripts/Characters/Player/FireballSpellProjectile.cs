using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class FireballSpellProjectile : SpellProjectile
{
    // #config
    private Rigidbody _rigidbody;
    private Collider _collider;

    private ICaster _caster;

    private Vector3 _baseScale = Vector3.one / 2;
    private Vector3 _direction;

    private float _damage = 1f; 
    private float _spawnDuration = 0.3f; 

    private float _baseSpeed = 10f; 
    private float _currentSpeed = 10f;

    private float _explosionRadius = 2f; 
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

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
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
        if (_currenLlifeTime > _lifeTime - _noExplosionDelay || _currenLlifeTime <= 0)
            return;

        if (other.TryGetComponent(out ICaster caster) && caster != _caster) // #config  
            return;

        Explode();
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
