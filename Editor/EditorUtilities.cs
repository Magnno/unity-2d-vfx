#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Maguinho
{
    public static class EditorUtilities
    {
        public delegate void InspectorContent();

        public static void DrawBox(string header, InspectorContent content)
        {
            EditorGUILayout.BeginVertical("HelpBox");
            EditorGUI.indentLevel++;

            // Separator style
            Texture2D pixel = new Texture2D(1, 1);
            pixel.SetPixel(0, 0, new Color(1, 1, 1, .1f));
            pixel.Apply();
            GUIStyle separatorStyle = new GUIStyle
            {
                margin = new RectOffset(0, 0, 4, 4),
                fixedHeight = 1
            };
            separatorStyle.normal.background = pixel;

            EditorGUILayout.LabelField(header, EditorStyles.boldLabel); // Header
            GUILayout.Box(GUIContent.none, separatorStyle); // Line separator

            content();

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }

        public static void DrawProperty(this SerializedObject so, string propertyRef, string label, out SerializedProperty property)
        {
            property = so.FindProperty(propertyRef);
            EditorGUILayout.PropertyField(property, new GUIContent(label));
        }
    }
}
#endif
