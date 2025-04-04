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

        // ���������� �������, ����������� �� ������
        string[] folders = AssetDatabase.GetSubFolders("Assets/Resources/Prefabs"); // �������� ��� ����� � ����������
        foreach (var folder in folders)
        {
            // ��������� ��� ������ �����
            GUILayout.Label(Path.GetFileName(folder), EditorStyles.boldLabel);

            // �������� ������� � ������ �����
            string[] prefabPaths = AssetDatabase.FindAssets("t:Prefab", new[] { folder });
            foreach (string guid in prefabPaths)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (GUILayout.Button(prefab.name)) // ������ ��� ������ �������
                {
                    selectedPrefab = prefab;
                }
            }

            // ��������� ������ ����� �������
            GUILayout.Space(10);
        }

        // ���� ������ ������, ���������� ������ ��� ��� ���������� �� �����
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
