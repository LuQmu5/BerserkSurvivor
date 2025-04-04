using UnityEngine;
using static UnityEditor.Progress;

public class DevScript_ObjectSpawner : MonoBehaviour
{
    private void Update()
    {
        SpawnEntitiesCheck();
        CollectAllItemsCheck();
    }

    private void SpawnEntitiesCheck()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnItems(ItemFactory.Instance, Random.Range(5, 25));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            SpawnItems(EnemyFactory.Instance, 1);
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
        float spawnMinRadius = 5f; 
        float spawnMaxRadius = 10f;

        for (int i = 0; i < itemsToSpawn; i++)
        {
            float randomAngle = Random.Range(0f, 2f * Mathf.PI);
            float randomRadius = Random.Range(spawnMinRadius, spawnMaxRadius);

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
}