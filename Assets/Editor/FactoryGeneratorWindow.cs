using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

public class FactoryGeneratorWindow : EditorWindow
{
    private string factoryName = "NewFactory";
    private string savePath = "Assets/Scripts/Infrastructure";
    private string enumName = "NewEnum";
    private string[] enumElements = new string[0];
    private MonoScript baseScript;

    [MenuItem("Tools/Factory Generator")]
    public static void ShowWindow()
    {
        GetWindow<FactoryGeneratorWindow>("Factory Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Factory Generator", EditorStyles.boldLabel);

        // Имя фабрики
        factoryName = EditorGUILayout.TextField("Factory Name:", factoryName);

        // Путь сохранения
        savePath = EditorGUILayout.TextField("Save Path:", savePath);

        // Имя перечисления
        enumName = EditorGUILayout.TextField("Enum Name:", enumName);

        // Поле для добавления элементов в перечисление
        EditorGUILayout.LabelField("Enum Elements:");
        for (int i = 0; i < enumElements.Length; i++)
        {
            enumElements[i] = EditorGUILayout.TextField($"Element {i + 1}:", enumElements[i]);
        }

        if (GUILayout.Button("Add Enum Element"))
        {
            ArrayUtility.Add(ref enumElements, "");
        }

        // Поле для выбора MonoBehaviour
        baseScript = (MonoScript)EditorGUILayout.ObjectField("Base MonoBehaviour Script:", baseScript, typeof(MonoScript), false);

        if (GUILayout.Button("Create Factory"))
        {
            CreateFactoryScript(factoryName, savePath, enumName, enumElements, baseScript);
        }
    }

    private void CreateFactoryScript(string name, string path, string enumName, string[] enumElements, MonoScript baseScript)
    {
        string fullPath = Path.Combine(path, name + ".cs");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        if (File.Exists(fullPath))
        {
            Debug.LogError("Factory script already exists!");
            return;
        }

        string scriptContent = GenerateFactoryCode(name, enumName, enumElements, baseScript);
        File.WriteAllText(fullPath, scriptContent);
        AssetDatabase.Refresh();
        Debug.Log("Factory script created at " + fullPath);
    }

    private string GenerateFactoryCode(string name, string enumName, string[] enumElements, MonoScript baseScript)
    {
        // Сгенерируем перечисление
        string enumDefinition = $"public enum {enumName}\n{{\n";
        foreach (var element in enumElements)
        {
            enumDefinition += $"    {element},\n";
        }
        enumDefinition += "}";

        // Получаем тип скрипта, который был выбран через MonoScript
        string baseScriptName = baseScript ? baseScript.name : "MonoBehaviour"; // Если скрипт не выбран, используем MonoBehaviour как базовый класс

        return $@"
using System.Collections.Generic;
using UnityEngine;

public class {name}
{{
    private readonly Dictionary<{enumName}, {baseScriptName}> _itemPrefabs = new Dictionary<{enumName}, {baseScriptName}>();
    private Dictionary<{enumName}, ObjectPool<{baseScriptName}>> _pools = new Dictionary<{enumName}, ObjectPool<{baseScriptName}>>();

    public static {name} Instance {{ get; private set; }}

    public {name}({baseScriptName}[] itemPrefabsList, Transform parent)
    {{
        Instance = this;

        foreach ({baseScriptName} item in itemPrefabsList)
        {{
            _itemPrefabs[item.Type] = item;  // Используем свойство Type для доступа к типу объекта
            _pools[item.Type] = new ObjectPool<{baseScriptName}>(parent);
        }}
    }}

    public {baseScriptName} GetItem({enumName} type)
    {{
        if (_itemPrefabs.ContainsKey(type))
        {{
            {baseScriptName} prefab = _itemPrefabs[type];
            return _pools[type].GetObject(prefab);
        }}
        else
        {{
            Debug.LogWarning($""Prefab for item type {{type}} not found!"");
            return null;
        }}
    }}

    public void ReturnItem({baseScriptName} item)
    {{
        if (_pools.ContainsKey(item.Type))
        {{
            _pools[item.Type].ReturnObject(item);
        }}
    }}
}}

{enumDefinition}
";
    }
}
