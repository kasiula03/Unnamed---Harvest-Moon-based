using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor (typeof (Crop))]
public class CropStageEditor : Editor {

    public bool showCrop;

    private Crop crop;
    private SerializedObject GetTarget;
    private SerializedProperty stagesProperty;
    private SerializedProperty availableSeasonProperty;
    private int listSize;

    private void OnEnable()
    {
        crop = (Crop) target;
        GetTarget = new SerializedObject(crop);
        if (target == null)
        {
            DestroyImmediate(this);
            return;
        }

        stagesProperty = GetTarget.FindProperty("stages");
        availableSeasonProperty = GetTarget.FindProperty("availableSeasonToGrow");

    }

    public override void OnInspectorGUI()
    {
        GetTarget.Update();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("Crop object");
        EditorGUILayout.BeginHorizontal();
        SerializedProperty cropObjectProperty = GetTarget.FindProperty("crop");
        cropObjectProperty.objectReferenceValue = EditorGUILayout.ObjectField(cropObjectProperty.objectReferenceValue, typeof(PickableObject), false);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        SerializedProperty waterProps = GetTarget.FindProperty("isWatered");
         EditorGUILayout.PropertyField(waterProps);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("current crop day");
        SerializedProperty c1 = GetTarget.FindProperty("currentCropDay");
        c1.intValue = EditorGUILayout.IntField(c1.intValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("current crop stage");
        SerializedProperty c2 = GetTarget.FindProperty("currentStage");
        c2.intValue = EditorGUILayout.IntField(c2.intValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField("Stages");
        EditorGUI.indentLevel--;

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        GUILayout.Label("Add a new available season");
        if (GUILayout.Button("Add New"))
        {
            crop.availableSeasonToGrow.Add(new Season());
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical();
        for(int i = 0; i < availableSeasonProperty.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(availableSeasonProperty.GetArrayElementAtIndex(i), new GUIContent("Season"));
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        GUILayout.Label("Add a new item with a button");
        if (GUILayout.Button("Add New"))
        {
            crop.stages.Add(new Crop.Stage());
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        for (int i = 0; i < stagesProperty.arraySize; i++)
        {
            SerializedProperty MyListRef = stagesProperty.GetArrayElementAtIndex(i);
            SerializedProperty dayOfStageProperty = MyListRef.FindPropertyRelative("dayOfStage");
            SerializedProperty modelProperty = MyListRef.FindPropertyRelative("stageModel");

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Stage model");
            modelProperty.objectReferenceValue = EditorGUILayout.ObjectField( modelProperty.objectReferenceValue, typeof(GameObject), true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Day of stage");
            dayOfStageProperty.intValue = EditorGUILayout.IntField(dayOfStageProperty.intValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Remove this stage"))
            {
                stagesProperty.DeleteArrayElementAtIndex(i);
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();

        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        GetTarget.ApplyModifiedProperties();
    }

}
