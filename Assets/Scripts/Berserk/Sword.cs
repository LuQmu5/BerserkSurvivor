using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private Collider _collider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IHealth health) && other.TryGetComponent(out BerserkController berserk) == false)
        {
            health.ApplyDamage(1);
        }
    }
}