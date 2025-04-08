using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class StaticDataBrowser : EditorWindow
{
    private Vector2 scrollPos;
    private Dictionary<string, List<ScriptableObject>> groupedData;

    private const string StaticDataPath = "Assets/Resources/StaticData";

    [MenuItem("Tools/Static Data Browser")]
    public static void OpenWindow()
    {
        GetWindow<StaticDataBrowser>("Static Data Browser");
    }

    private void OnEnable()
    {
        RefreshData();
    }

    private void RefreshData()
    {
        groupedData = new Dictionary<string, List<ScriptableObject>>();
        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { StaticDataPath });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
            if (so == null) continue;

            string folder = Path.GetDirectoryName(path).Replace('\\', '/');
            folder = folder.Substring(StaticDataPath.Length).TrimStart('/');
            if (string.IsNullOrEmpty(folder)) folder = "Root";

            if (!groupedData.ContainsKey(folder))
                groupedData[folder] = new List<ScriptableObject>();

            groupedData[folder].Add(so);
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("🔄 Обновить"))
            RefreshData();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        foreach (var group in groupedData)
        {
            EditorGUILayout.LabelField($"📁 {group.Key}", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            foreach (var so in group.Value)
            {
                if (GUILayout.Button(so.name, EditorStyles.objectField))
                {
                    Selection.activeObject = so;
                    EditorGUIUtility.PingObject(so);
                }
            }

            EditorGUI.indentLevel--;
            GUILayout.Space(10);
        }
        EditorGUILayout.EndScrollView();
    }
}
