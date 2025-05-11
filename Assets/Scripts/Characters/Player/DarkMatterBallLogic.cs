using System.Collections;
using UnityEngine;

public class DarkMatterBallLogic : SpellView
{
    private float _flyTime = 2;
    private float _duration = 3;

    private float _speed = 2;
    private ICaster _caster;
    private CharacterStats _casterStats;
    private Vector3 _direction;

    public override void Init(ICaster caster, CharacterStats stats)
    {
        gameObject.SetActive(true);

        _caster = caster;
        _casterStats = stats;

        transform.position = caster.CastPoint.position;
        _direction = caster.CastPoint.forward;

        StartCoroutine(Launching());
    }

    private IEnumerator Launching()
    {
        float timer = _flyTime;

        while (timer > 0)
        {
            transform.Translate(_direction * _speed * Time.deltaTime);
            timer -= Time.deltaTime;

            yield return null;
        }

        timer = _duration;

        while (timer > 0)
        {
            // logic

            timer -= Time.deltaTime;

            yield return null;
        }

        gameObject.SetActive(false);    
    }
}
