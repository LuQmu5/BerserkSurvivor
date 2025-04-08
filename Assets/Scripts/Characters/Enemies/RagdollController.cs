using UnityEngine;

public class RagdollController : MonoBehaviour
{
    private Rigidbody[] _rigidbodies;
    private Collider[] _colliders;

    void Start()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();

        SetRagdollState(false);
    }

    public void SetRagdollState(bool isActive, float pushPower = 10)
    {
        foreach (var rb in _rigidbodies)
        {
            rb.isKinematic = !isActive;  // Включаем физику для регдолла
        }

        foreach (var col in _colliders)
        {
            col.enabled = isActive;  // Включаем коллайдеры
        }

        if (isActive)
        {
            ApplyPush(pushPower);  // Применяем толчок при активации ragdoll
        }

        if (!isActive)
        {
            ResetCharacterPosition();  // Сбрасываем положение, если деактивируем ragdoll
        }
    }

    // Восстановление исходного положения персонажа (например, лежащего на земле)
    private void ResetCharacterPosition()
    {
        // Установите персонажа в нужное положение после деактивации ragdoll
        // Например, установить его на землю или в конкретную позицию
    }

    // Применение толчка, например, в сторону источника урона
    private void ApplyPush(float pushPower)
    {
        // Пример: применяем силу толчка в сторону источника урона
        Vector3 pushDirection = transform.forward * - 1;  // Направление толчка (можно изменить)

        // Применяем силу ко всем rigidbody частям регдолла
        foreach (var rb in _rigidbodies)
        {
            // Применяем силу толчка
            rb.AddForce(pushDirection * pushPower, ForceMode.Impulse);
        }
    }
}
