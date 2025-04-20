using UnityEngine;
using Zenject;

public class DevScript_ObjectSpawner : MonoBehaviour
{
    private EnemyFactory _enemyFactory;
    private ItemFactory _itemFactory;

    [Inject]
    public void Construct(EnemyFactory enemyFactory, ItemFactory itemFactory)
    {
        _enemyFactory = enemyFactory;
        _itemFactory = itemFactory;
    }

    private void Update()
    {
        SpawnEntitiesCheck();
        CollectAllItemsCheck();
    }

    private void SpawnEntitiesCheck()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnItems(_itemFactory, Random.Range(10, 25));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnItems(_enemyFactory, 1);  // Спавн врагов
        }
    }

    private static void CollectAllItemsCheck()
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

    private void SpawnItems<T>(IFactory<T> factory, int itemsToSpawn)
    {
        int spawnMinRadius = 5;
        int spawnMaxRadius = 10;

        System.Random rnd = new System.Random();

        for (int i = 0; i < itemsToSpawn; i++)
        {
            float randomAngle = rnd.Next(0, 2) * Mathf.PI;
            float randomRadius = rnd.Next(spawnMinRadius, spawnMaxRadius);

            Vector3 offset = new Vector3(Mathf.Cos(randomAngle) * randomRadius, 0, Mathf.Sin(randomAngle) * randomRadius);
            Vector3 spawnPosition = Vector3.zero + offset;
            Transform playerTransform = FindObjectOfType<CharacterBehaviour>().transform;

            if (factory is ItemFactory itemFactory)
            {
                Item item = itemFactory.GetItem(new System.Random().GetRandomEnumValue<ItemType>());
                item.Init(playerTransform.position + spawnPosition + Vector3.up * item.transform.localPosition.y);
            }
            else if (factory is EnemyFactory enemyFactory)
            {
                EnemyBehaviour enemy = enemyFactory.GetItem(new System.Random().GetRandomEnumValue<EnemyType>());
                enemy.Init(playerTransform, playerTransform.position + offset);
            }
        }
    }

    // Новый метод для спавна врагов внутри сферы вокруг персонажа
    private void SpawnEnemies(EnemyFactory enemyFactory, int enemiesToSpawn)
    {
        CharacterBehaviour player = FindObjectOfType<CharacterBehaviour>();
        if (player == null) return;

        float spawnRadius = 50f;  // Радиус сферы вокруг персонажа

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Случайная точка внутри сферы вокруг персонажа
            Vector3 randomPosition = player.transform.position + Random.insideUnitSphere * spawnRadius;

            // Генерация врага в случайной позиции
            EnemyBehaviour enemy = enemyFactory.GetItem(new System.Random().GetRandomEnumValue<EnemyType>());
            enemy.Init(player.transform, randomPosition);
        }
    }
}
