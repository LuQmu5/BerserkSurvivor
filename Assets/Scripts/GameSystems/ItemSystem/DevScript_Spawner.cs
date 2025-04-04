using UnityEngine;

public class DevScript_Spawner : MonoBehaviour
{
    public float spawnMinRadius = 5f; // Минимальное расстояние от игрока
    public float spawnMaxRadius = 10f; // Максимальное расстояние от игрока
    public int itemsToSpawn = 10;

    private void Update()
    {
        SpawnItemsCheat();
        CollectAllItemsCheat();
    }

    private void SpawnItemsCheat()
    {
        if (Input.GetMouseButtonDown(0)) // Если нажата левая кнопка мыши
        {
            SpawnItems();
        }
    }

    private static void CollectAllItemsCheat()
    {
        CharacterBehaviour player = FindObjectOfType<CharacterBehaviour>();

        if (Input.GetKeyDown(KeyCode.P) && player != null)
        {
            Item[] items = FindObjectsOfType<Item>();

            foreach (var item in items)
            {
                player.PickUp(item);
            }
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
            Item item = ItemFactory.Instance.GetItem(new System.Random().GetRandomItemType()); // new System.Random().GetRandomItemType()
            item.transform.position = spawnPosition;
            item.OnDropped();
        }
    }
}