using UnityEngine;

public class BloodVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particles;

    private void Start()
    {
        Destroy(gameObject, 1);
    }

    public void Play()
    {
        foreach (ParticleSystem particle in _particles)
        {
            particle.Play();
        }
    }
}
