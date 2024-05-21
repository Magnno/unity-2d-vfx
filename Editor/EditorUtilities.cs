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
            GUIStyle headerStyle = new();
            headerStyle.fontSize = 14;
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.normal.textColor = new Color(.85f, .85f, .85f, 1f);

            if (boxStyle == BoxSytle.WithBorder)
                EditorGUILayout.BeginVertical("HelpBox");
            else if (boxStyle == BoxSytle.NoBorder)
                EditorGUILayout.BeginVertical("Box");

            EditorGUI.indentLevel += indentLvl;

            if (!string.IsNullOrEmpty(header))
                EditorGUILayout.LabelField(header, headerStyle); // Header

            if (useLine)
                DrawSeparatorLine();

            content(); // Draw the inspector

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel -= indentLvl;
        }

        public static void DrawFoldoutStyle1(ref bool enable, string header, Color headerColor, InspectorContent content)
        {
            GUIStyle boxStyle = new(EditorStyles.helpBox);
            boxStyle.padding = new RectOffset(0, 0, 0, 0);
            if (enable)
                boxStyle.padding = new RectOffset(0, 0, 0, 5);

            EditorGUILayout.BeginVertical(boxStyle);
            EditorGUI.indentLevel++;

            GUIStyle foldoutStyle = new(EditorStyles.foldout);
            foldoutStyle.fontSize = 14;
            foldoutStyle.fontStyle = FontStyle.Bold;
            foldoutStyle.normal.textColor = new Color(.85f, .85f, .85f, 1f);

            GUIStyle boxStyle2 = new(EditorStyles.helpBox);
            boxStyle2.padding = new RectOffset(17, 17, 5, 5);
            boxStyle2.margin = new RectOffset(0, 0, 0, 0);
            Texture2D text = new Texture2D(1, 1);
            text.SetPixel(0, 0, headerColor);
            text.Apply();
            boxStyle2.normal.background = text;

            EditorGUILayout.BeginVertical(boxStyle2);
            enable = EditorGUILayout.BeginFoldoutHeaderGroup(enable, header, style: foldoutStyle);
            EditorGUILayout.EndVertical();

            if (enable)
            {
                content(); // Draw the inspector
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }

        public static void DrawSeparatorLine()
        {
            Texture2D pixel = new(1, 1);
            pixel.SetPixel(0, 0, new Color(1, 1, 1, .1f));
            pixel.Apply();

            GUIStyle separatorStyle = new();
            separatorStyle.margin = new RectOffset(0, 0, 4, 4);
            separatorStyle.fixedHeight = 1;
            separatorStyle.normal.background = pixel;

            GUILayout.Box(GUIContent.none, separatorStyle);
        }
    }
}

#endif
