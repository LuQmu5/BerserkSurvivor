using DG.Tweening;
using System;
using UnityEngine;
using Zenject.SpaceFighter;

public abstract class Item : MonoBehaviour, ITypeable
{
    [Header("Item Settings")]
    [field: SerializeField] public ItemType Type { get; private set; }

    public Enum ObjType => Type;

    // Возможно как-то модульно сделать, чтобы закидывать сюда пресеты анимаций по названиям, в какой-нибудь массив разделенный по принципу в какой момент какую анимацию проигрывать
    // и они применяется, либо просто отдельный View скрипт сделать и связать его с этим
    [Space(20)]
    [Header("Animation Settings")]
    [SerializeField] private float _jumpHeight = 0.5f;  
    [SerializeField] private float _jumpDuration = 0.3f; 
    [SerializeField] private float _rotationSpeed = 50f; 
    [SerializeField] private float _pulseScaleFactor = 1.2f; 
    [SerializeField] private float _pulseDuration = 1f;

    private Collider _collider;
    private Vector3 _baseScale;
    private Vector3 _baseRotation;

    private void Awake()
    {
        _collider = GetComponent<Collider>();

        _baseScale = transform.localScale;
        _baseRotation = transform.eulerAngles;
    }

    public void Init(Vector3 position)
    {
        transform.position = position;
        transform.localScale = _baseScale;
        transform.eulerAngles = _baseRotation;
        _collider.enabled = true;
        gameObject.SetActive(true);
        OnDropped();
    }

    private void OnDisable()
    {
        DOTween.Kill(gameObject);
    }

    public virtual void OnDropped()
    {
        DoJump();
        DoPulsarRotate();
    }

    private void DoPulsarRotate()
    {
        transform.DORotate(new Vector3(0, 360, 0), _rotationSpeed / _pulseDuration, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        transform.DOScale(transform.localScale * _pulseScaleFactor, _pulseDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    private void DoJump()
    {
        transform.DOLocalMoveY(transform.position.y + _jumpHeight, _jumpDuration)
         .SetEase(Ease.OutQuad)
         .OnComplete(() =>
             transform.DOLocalMoveY(transform.position.y - _jumpHeight, _jumpDuration)
                      .SetEase(Ease.InQuad)
         );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IItemPicker itemPicker) && _collider.enabled)
        {
            itemPicker.PickUp(this);
            _collider.enabled = false;
        }
    }

    public virtual void OnPickedUp(IItemPicker picker)
    {
        ItemFactory.Instance.ReturnItem(this);
    }
}
