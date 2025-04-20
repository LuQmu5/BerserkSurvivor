using UnityEngine;
using Zenject;
using System.Collections.Generic;
using System.Text;

public class DevScript_ObjectSpawner : MonoBehaviour
{
    private EnemyFactory _enemyFactory;
    private ItemFactory _itemFactory;

    private StringBuilder _inputBuffer = new StringBuilder();
    private float _inputResetTime = 2f;
    private float _lastInputTime;

    [Inject]
    public void Construct(EnemyFactory enemyFactory, ItemFactory itemFactory)
    {
        _enemyFactory = enemyFactory;
        _itemFactory = itemFactory;
    }

    private void Update()
    {
        HandleCheatInput();
        CollectAllItemsCheck();
    }

    private void HandleCheatInput()
    {
        foreach (char c in Input.inputString)
        {
            if (char.IsLetter(c))
            {
                _inputBuffer.Append(char.ToLower(c));
                _lastInputTime = Time.time;
            }
        }

        if (_inputBuffer.Length > 0 && Time.time - _lastInputTime > _inputResetTime)
        {
            _inputBuffer.Clear(); // сбрасываем если слишком долго ничего не вводили
        }

        string input = _inputBuffer.ToString();

        if (input.EndsWith("enemy"))
        {
            SpawnEnemies(_enemyFactory, 5);
            Debug.Log("Чит-код активирован: ENEMY");
            _inputBuffer.Clear();
        }
        else if (input.EndsWith("items"))
        {
            SpawnItems(_itemFactory, Random.Range(10, 25));
            Debug.Log("Чит-код активирован: ITEMS");
            _inputBuffer.Clear();
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
        Transform playerTransform = FindObjectOfType<CharacterBehaviour>().transform;

        for (int i = 0; i < itemsToSpawn; i++)
        {
            float randomAngle = rnd.Next(0, 360) * Mathf.Deg2Rad;
            float randomRadius = rnd.Next(spawnMinRadius, spawnMaxRadius);
            Vector3 offset = new Vector3(Mathf.Cos(randomAngle) * randomRadius, 0, Mathf.Sin(randomAngle) * randomRadius);
            Vector3 spawnPosition = playerTransform.position + offset;

            if (factory is ItemFactory itemFactory)
            {
                Item item = itemFactory.GetItem(new System.Random().GetRandomEnumValue<ItemType>());
                item.Init(spawnPosition + Vector3.up * item.transform.localPosition.y);
            }
            else if (factory is EnemyFactory enemyFactory)
            {
                EnemyBehaviour enemy = enemyFactory.GetItem(new System.Random().GetRandomEnumValue<EnemyType>());
                enemy.Init(playerTransform, spawnPosition);
            }
        }
    }

    private void SpawnEnemies(EnemyFactory enemyFactory, int enemiesToSpawn)
    {
        CharacterBehaviour player = FindObjectOfType<CharacterBehaviour>();
        if (player == null) return;

        float spawnRadius = 50f;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 randomPosition = player.transform.position + Random.insideUnitSphere * spawnRadius;
            randomPosition.y = player.transform.position.y; // для ровного спавна по земле

            EnemyBehaviour enemy = enemyFactory.GetItem(new System.Random().GetRandomEnumValue<EnemyType>());
            enemy.Init(player.transform, randomPosition);
        }
    }
}
