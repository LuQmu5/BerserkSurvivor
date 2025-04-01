using System;
using UnityEngine;
using UnityEngine.UI;

public class SigilView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Image _activeBorder;

    [Header("Icons")]
    [SerializeField] private Sprite ArcaneSigil;
    [SerializeField] private Sprite DeathSigil;
    [SerializeField] private Sprite EarthSigil;
    [SerializeField] private Sprite FireSigil;
    [SerializeField] private Sprite FrostSigil;
    [SerializeField] private Sprite InfernoSigil;
    [SerializeField] private Sprite LifeSigil;
    [SerializeField] private Sprite WindSigil;
    [SerializeField] private Sprite NoSigil;

    private CanvasGroup _canvasGroup;

    private const float BaseAlpha = 1;
    private const float NotActiveAlpha = 0.1f;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetIconFor(MagicElements element)
    {
        SetAlpha(BaseAlpha);

        switch (element)
        {
            case MagicElements.Arcane:
                _image.sprite = ArcaneSigil;
                break;

            case MagicElements.Death:
                _image.sprite = DeathSigil;
                break;

            case MagicElements.Fire:
                _image.sprite = FireSigil;
                break;

            case MagicElements.Frost:
                _image.sprite = FrostSigil;
                break;

            case MagicElements.Inferno:
                _image.sprite = InfernoSigil;
                break;

            case MagicElements.Earth:
                _image.sprite = EarthSigil;
                break;

            case MagicElements.Life:
                _image.sprite = LifeSigil;
                break;

            case MagicElements.Wind:
                _image.sprite = WindSigil;
                break;

            default:
                ResetView();
                break;
        }
    }

    public void SetAlpha(float value)
    {
        _canvasGroup.alpha = value;
    }

    private void ResetView()
    {
        _image.sprite = NoSigil;
        SetAlpha(NotActiveAlpha);
    }

    public void ActivateBorder()
    {
        _activeBorder.gameObject.SetActive(true);
    }

    public void DeactivateBorder()
    {
        _activeBorder.gameObject.SetActive(false);
    }

    public void SetActiveAlpha()
    {
        SetAlpha(BaseAlpha);
    }
}