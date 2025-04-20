using UnityEngine;
using UnityEngine.UI;

public class SigilView : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _activeBorderImage;
    [SerializeField] private CanvasGroup _canvasGroup;

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

    private const float ActiveAlpha = 1;
    private const float NotActiveAlpha = 0.1f;

    public void UpdateIconWith(MagicElements element)
    {
        switch (element)
        {
            case MagicElements.Arcane:
                _iconImage.sprite = ArcaneSigil;
                break;

            case MagicElements.Death:
                _iconImage.sprite = DeathSigil;
                break;

            case MagicElements.Fire:
                _iconImage.sprite = FireSigil;
                break;

            case MagicElements.Frost:
                _iconImage.sprite = FrostSigil;
                break;

            case MagicElements.Inferno:
                _iconImage.sprite = InfernoSigil;
                break;

            case MagicElements.Earth:
                _iconImage.sprite = EarthSigil;
                break;

            case MagicElements.Life:
                _iconImage.sprite = LifeSigil;
                break;

            case MagicElements.Wind:
                _iconImage.sprite = WindSigil;
                break;

            case MagicElements.None:
            default:
                _iconImage.sprite = NoSigil;
                break;
        }
    }

    public void SetActive(bool isActive)
    {
        _canvasGroup.alpha = isActive ? ActiveAlpha : NotActiveAlpha;
        _activeBorderImage.gameObject.SetActive(isActive);
    }
}
