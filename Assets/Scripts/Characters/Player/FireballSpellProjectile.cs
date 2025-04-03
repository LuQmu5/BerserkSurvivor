using System;
using UnityEngine;

public class FireballSpellProjectile : SpellProjectile
{
    private ICaster _caster;
    private Vector3 _direction;
    private float _speed = 5;

    public override void Init(ICaster caster)
    {
        _caster = caster;
        _direction = caster.Transform.forward;
        transform.position = _caster.CastPoint.position;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode();
    }

    private void Explode()
    {
        Debug.Log("BABAH!");
        gameObject.SetActive(false); 
    }
}
