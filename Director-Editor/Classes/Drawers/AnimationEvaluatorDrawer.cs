using Tangzx.Director;
using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    [CustomPropertyDrawer(typeof(AnimationEvaluator))]
    class AnimationEvaluatorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty useCurve = property.FindPropertyRelative("useCurve");

            position.height = 20;
            EditorGUI.PropertyField(position, useCurve);

            if (useCurve.boolValue)
            {
                position.yMin += 20;
                position.height = 20;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("curve"));
            }
            else
            {
                position.yMin += 20;
                position.height = 20;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("easeType"));
                position.yMin += 20;
                position.height = 20;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("overshootOrAmplitude"));
                position.yMin += 20;
                position.height = 20;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("period"));
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty useCurve = property.FindPropertyRelative("useCurve");
            if (useCurve.boolValue)
                return 20 * 2;
            else
                return 20 * 4;
        }
    }
}
