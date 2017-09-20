using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TreePlanterWindow : EditorWindow {

    [MenuItem("Window/TreePlacer")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        TreePlanterWindow window = (TreePlanterWindow)EditorWindow.GetWindow(typeof(TreePlanterWindow));
        window.Show();
    }

    private List<GameObject> prefabs;
    private float minDis = 2;
    private float maxDis = 10;
    private bool randomFlipX = true;
    private Transform startPoint;
    private Transform endPoint;
    private Transform root;
    private string copyName;
    private Vector2 scrollPos;

    private void OnEnable()
    {
        prefabs = new List<GameObject>();
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUILayout.LabelField("To Copy");
        int spriteLength = EditorGUILayout.IntField("Prefabs Count", prefabs.Count);
        while (prefabs.Count > spriteLength)
        {
            prefabs.RemoveAt(prefabs.Count - 1);
        }
        while (prefabs.Count < spriteLength)
        {
            prefabs.Add(null);
        }
        for (int i = 0; i < prefabs.Count; i++)
        {
            prefabs[i] = (GameObject)EditorGUILayout.ObjectField("Prefab", prefabs[i], typeof(GameObject), true);
        }
       

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Config");
        root = (Transform)EditorGUILayout.ObjectField("Root", root, typeof(Transform), true);
        startPoint = (Transform)EditorGUILayout.ObjectField("Start Point", startPoint, typeof(Transform), true);
        endPoint = (Transform)EditorGUILayout.ObjectField("End Point", endPoint, typeof(Transform), true);
        randomFlipX = EditorGUILayout.Toggle("Random Flip", randomFlipX);
        minDis = EditorGUILayout.FloatField("Min Dis Between Objs", minDis);
        maxDis = EditorGUILayout.FloatField("Max Dis Between Objs", maxDis);
        copyName = EditorGUILayout.TextField("Name", copyName);

        GUI.enabled = prefabs.Count >= 1 && minDis >= 0 && maxDis >= minDis
            && startPoint != null && endPoint != null;

        if (GUILayout.Button("Build"))
        {
            float cX = startPoint.position.x;
            int counter = 1;
            while (cX < endPoint.position.x)
            {
                cX += Random.Range(minDis, maxDis);
                if (cX > endPoint.position.x)
                    break;
                GameObject copy = (GameObject)PrefabUtility.InstantiatePrefab(prefabs[(int)Random.Range(0, prefabs.Count)]);
                copy.transform.parent = root;
                copy.transform.localPosition = new Vector3(cX, 0);
                if (randomFlipX)
                    copy.GetComponentInChildren<SpriteRenderer>().flipX = Random.value > 0.5f;
                copy.name = copy.name + counter++;
            }
        }
        GUI.enabled = true;
        EditorGUILayout.EndScrollView();
    }
}
