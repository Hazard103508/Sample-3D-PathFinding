using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(Data.Size))]
    public class SizeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty x = property.FindPropertyRelative("width");
            SerializedProperty y = property.FindPropertyRelative("height");
            SerializedProperty z = property.FindPropertyRelative("depth");

            EditorGUI.BeginProperty(position, label, property);

            float labelSizeFactor = 0.2f;
            EditorGUI.LabelField(new Rect(position.x, position.y, position.width * labelSizeFactor, position.height), "Size");

            float[] labelFixedSize = new float[] { 40, 45, 40 };
            float controlsWidh = ((position.width * (1 - labelSizeFactor)) - labelFixedSize.Sum()) / 3;

            float cellWidh = (position.width * (1 - labelSizeFactor)) / 6;
            float posLabelX = position.x + position.width * labelSizeFactor;
            float posInputX = posLabelX + labelFixedSize[0];

            float posLabelY = posInputX + controlsWidh;
            float posInputY = posLabelY + labelFixedSize[1];

            float posLabelZ = posInputY + controlsWidh;
            float posInputZ = posLabelZ + labelFixedSize[2];

            EditorGUI.LabelField(new Rect(posLabelX, position.y, labelFixedSize[0], position.height), "Width");
            x.intValue = EditorGUI.IntField(new Rect(posInputX, position.y, controlsWidh, position.height), x.intValue);
            EditorGUI.LabelField(new Rect(posLabelY, position.y, labelFixedSize[1], position.height), " Height");
            y.intValue = EditorGUI.IntField(new Rect(posInputY, position.y, controlsWidh, position.height), y.intValue);
            EditorGUI.LabelField(new Rect(posLabelZ, position.y, labelFixedSize[2], position.height), " Depth");
            z.intValue = EditorGUI.IntField(new Rect(posInputZ, position.y, controlsWidh, position.height), z.intValue);

            EditorGUI.EndProperty();
        }
    }
}
