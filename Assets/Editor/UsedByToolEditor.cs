using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UsableObject), true)]
public class UsedByToolEditor : Editor
{
    private UsableObject usableObject;
    private SerializedObject usableSerialized;
    private SerializedProperty usableByToolsProperty;

    private void OnEnable()
    {
        usableObject = (UsableObject)target;
        usableSerialized = new SerializedObject(usableObject);
        if (target == null)
        {
            DestroyImmediate(this);
            return;
        }
        usableByToolsProperty = usableSerialized.FindProperty("usableByTools");

    }

    public override void OnInspectorGUI()
    {
        usableSerialized.Update();

        DrawDefaultInspector();

        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.LabelField("Available tools that can use the object");

        EditorGUILayout.BeginVertical();
        for (int i = 0; i < usableByToolsProperty.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(usableByToolsProperty.GetArrayElementAtIndex(i), new GUIContent("Tool Type"));
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        Rect plusButtonPosition = GUILayoutUtility.GetLastRect();
        plusButtonPosition.position = new Vector2(plusButtonPosition.xMin + plusButtonPosition.width - 25, plusButtonPosition.position.y);
        plusButtonPosition.width = 25;
        plusButtonPosition.height = 25;
        if (GUI.Button(plusButtonPosition, "+"))
        {
            usableObject.usableByTools.Add(new ToolType());
        }
        if(usableByToolsProperty.arraySize > 0)
        {
            Rect minusButtonPosition = plusButtonPosition;
            minusButtonPosition.position = new Vector2(plusButtonPosition.xMin - 25, plusButtonPosition.yMin);
            if (GUI.Button(minusButtonPosition, "-"))
            {
                usableByToolsProperty.DeleteArrayElementAtIndex(usableByToolsProperty.arraySize - 1);
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();

        usableSerialized.ApplyModifiedProperties();
    }

}
