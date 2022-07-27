using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Data.Map))]
    public class MapEditor : UnityEditor.Editor
    {
        SerializedProperty size;
        SerializedProperty characterLocation;
        SerializedProperty boxLocation;
        SerializedProperty targetLocation;
        SerializedProperty tiles;

        void OnEnable()
        {
            if (Application.isPlaying)
                return;

            size = serializedObject.FindProperty("size");
            characterLocation = serializedObject.FindProperty("characterLocation");
            boxLocation = serializedObject.FindProperty("boxLocations");
            targetLocation = serializedObject.FindProperty("targetLocations");
            tiles = serializedObject.FindProperty("tiles");
        }
        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
                return;

            serializedObject.Update();

            var propHeight = size.FindPropertyRelative("height");
            var propWidth = size.FindPropertyRelative("width");
            var propDepth = size.FindPropertyRelative("depth");

            if (propHeight.intValue < 1) propHeight.intValue = 1;
            if (propWidth.intValue < 1) propWidth.intValue = 1;
            if (propDepth.intValue < 1) propDepth.intValue = 1;

            EditorGUILayout.PropertyField(size);
            EditorGUILayout.PropertyField(characterLocation);
            EditorGUILayout.PropertyField(boxLocation);
            EditorGUILayout.PropertyField(targetLocation);
            LoadTiles();

            serializedObject.ApplyModifiedProperties();
        }

        private void LoadTiles()
        {
            EditorGUILayout.LabelField("Map");

            var height = size.FindPropertyRelative("height").intValue;
            var width = size.FindPropertyRelative("width").intValue;
            var depth = size.FindPropertyRelative("depth").intValue;

            int currentSize = tiles.arraySize;
            int newSize = width * height * depth;

            if (tiles.arraySize != newSize)
                tiles.arraySize = newSize;

            for (int y = 0; y < height; y++)
            {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.LabelField($"Floor {y}");
                for (int z = depth - 1; z >= 0; z--)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int x = 0; x < width; x++)
                    {
                        int index = (z * width) + x + (width * depth * y);
                        var propValue = tiles.GetArrayElementAtIndex(index);
                        propValue.intValue = EditorGUILayout.IntField(propValue.intValue);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }
}