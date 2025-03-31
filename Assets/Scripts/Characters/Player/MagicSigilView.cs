using UnityEngine;

public class MagicSigilView : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;

    public void SetMaterial(Material material)
    {
        _renderer.material = material;
    }
}