// Mico.Scripts.Editor C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using Mico.Context;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Mico.Editor
{
    [CustomEditor(typeof(GameObjectContext))]
    public class GameObjectContextEditor : UnityEditor.Editor
    {
        private ReorderableList _reorderableList;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("parentContext"));
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