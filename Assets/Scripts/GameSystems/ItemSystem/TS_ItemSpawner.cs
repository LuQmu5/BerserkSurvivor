using UnityEngine;

public class TS_ItemSpawner : MonoBehaviour
{
    public Item itemPrefab;    // Префаб предмета
    public float spawnMinRadius = 5f; // Минимальное расстояние от игрока
    public float spawnMaxRadius = 10f; // Максимальное расстояние от игрока
    public int itemsToSpawn = 10;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Если нажата левая кнопка мыши
        {
            SpawnItems();
        }
    }

    // Метод для спавна предметов
    private void SpawnItems()
    {
        for (int i = 0; i < itemsToSpawn; i++)
        {
            // Вычисляем случайную позицию вокруг игрока в пределах заданного радиуса
            float randomAngle = Random.Range(0f, 2f * Mathf.PI); // Случайный угол
            float randomRadius = Random.Range(spawnMinRadius, spawnMaxRadius); // Случайное расстояние от игрока

            // Переводим полярные координаты в мировые
            Vector3 offset = new Vector3(Mathf.Cos(randomAngle) * randomRadius, 1f, Mathf.Sin(randomAngle) * randomRadius);
            Vector3 spawnPosition = Vector3.zero + offset; // Позиция для спавна

            // Создаём предмет
            Item item = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            item.OnDropped();
        }
    }
}