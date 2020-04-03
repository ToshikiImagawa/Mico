// Mico.Scripts.Editor C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using Mico.Context;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Mico.Editor
{
    [CustomEditor(typeof(SceneContext))]
    public class SceneContextEditor : UnityEditor.Editor
    {
        private ReorderableList _reorderableList;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var parentScenePathProperty = serializedObject.FindProperty("parentScenePath");
            var parentSceneAssetProperty = serializedObject.FindProperty("parentSceneAsset");

            if (Application.isPlaying)
            {
                var sceneContext = target as SceneContext;
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Parent Scene Context");
                    EditorGUILayout.ObjectField(sceneContext != null ? sceneContext.ParentSceneContext : null,
                        typeof(SceneContext), true);
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.PropertyField(parentSceneAssetProperty);
            }

            var sceneAsset = parentSceneAssetProperty.objectReferenceValue as SceneAsset;
            parentScenePathProperty.stringValue =
                sceneAsset != null ? AssetDatabase.GetAssetPath(sceneAsset) : string.Empty;
            if (_reorderableList == null)
            {
                var listProp = serializedObject.FindProperty("installers");
                _reorderableList = new ReorderableList(serializedObject, listProp)
                {
                    drawHeaderCallback = rect => EditorGUI.LabelField(rect, "MonoInstallers"),
                    drawElementCallback = (rect, index, isActive, isFocused) =>
                    {
                        var elementProperty = listProp.GetArrayElementAtIndex(index);
                        rect.height = EditorGUIUtility.singleLineHeight;
                        EditorGUI.PropertyField(rect, elementProperty, GUIContent.none);
                    }
                };
            }

            _reorderableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}