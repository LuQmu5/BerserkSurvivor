using UnityEditor;
using UnityEngine;
using System.IO;

public class PrefabEditorWindow : EditorWindow
{
    private GameObject selectedPrefab;

    [MenuItem("Window/Prefab Editor")]
    public static void ShowWindow()
    {
        GetWindow<PrefabEditorWindow>("Prefab Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Editor", EditorStyles.boldLabel);

        // Показываем префабы, разделенные по папкам
        string[] folders = AssetDatabase.GetSubFolders("Assets/Resources/Prefabs"); // Получаем все папки в директории
        foreach (var folder in folders)
        {
            // Заголовок для каждой папки
            GUILayout.Label(Path.GetFileName(folder), EditorStyles.boldLabel);

            // Получаем префабы в каждой папке
            string[] prefabPaths = AssetDatabase.FindAssets("t:Prefab", new[] { folder });
            foreach (string guid in prefabPaths)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (GUILayout.Button(prefab.name)) // Кнопка для выбора префаба
                {
                    selectedPrefab = prefab;
                }
            }

            // Добавляем отступ между папками
            GUILayout.Space(10);
        }

        // Если префаб выбран, отображаем кнопку для его размещения на сцене
        if (selectedPrefab != null)
        {
            GUILayout.Label("Selected Prefab: " + selectedPrefab.name);
            if (GUILayout.Button("Place Prefab"))
            {
                PlacePrefabAtMousePosition();
            }
        }
    }

    private void PlacePrefabAtMousePosition()
    {
        if (selectedPrefab != null)
        {
            Vector3 mousePosition = GetMousePositionInScene();
            GameObject instance = Instantiate(selectedPrefab, mousePosition, Quaternion.identity);
            instance.transform.SetParent(null);
        }
    }

    private Vector3 GetMousePositionInScene()
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
