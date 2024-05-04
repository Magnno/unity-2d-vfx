#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Maguinho
{
    public static class EditorUtilities
    {
        public delegate void InspectorContent();

        public enum BoxSytle
        {
            WithBorder,
            NoBorder,
        }

        public static void DrawBox(string header, InspectorContent content, BoxSytle boxStyle = BoxSytle.WithBorder, bool useLine = true, int indentLvl = 1)
        {
            // Header Style
            GUIStyle headerStyle = new GUIStyle();
            headerStyle.fontSize = 14;
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.normal.textColor = new Color(.85f, .85f, .85f, 1f);

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

            if (boxStyle == BoxSytle.WithBorder)
                EditorGUILayout.BeginVertical("HelpBox");
            else if (boxStyle == BoxSytle.NoBorder)
                EditorGUILayout.BeginVertical("Box");

            EditorGUI.indentLevel += indentLvl;

            if (!string.IsNullOrEmpty(header))
                EditorGUILayout.LabelField(header, headerStyle); // Header

            if (useLine)
                GUILayout.Box(GUIContent.none, separatorStyle); // Line separator

            content(); // Draw the inspector

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel -= indentLvl;
        }
    }
}

#endif
