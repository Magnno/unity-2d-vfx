#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Maguinho.VFX
{
    static class EditorUtils
    {
        public delegate void InspectorContent();

        public static void DrawBox(string header, BoxStyle style, InspectorContent content)
        {
            GUIStyle boxStyle = new GUIStyle(EditorStyles.helpBox);
            boxStyle.margin = style.margin;
            boxStyle.padding = style.padding;
            EditorGUILayout.BeginVertical(boxStyle);

            if (!string.IsNullOrEmpty(header))
            {
                // Header Style
                GUIStyle headerStyle = new();
                headerStyle.fontSize = 14;
                headerStyle.fontStyle = FontStyle.Bold;
                headerStyle.normal.textColor = new Color(.85f, .85f, .85f, 1f);

                EditorGUILayout.LabelField(header, headerStyle); // Header
                if (style.drawHeaderLine)
                    DrawSeparator(style.padding.left, style.padding.right);
            }

            content(); // Draw the inspector

            EditorGUILayout.EndVertical();
        }

        public static void DrawFoldout(ref bool enable, string header, FoldoutStyle style, InspectorContent content)
        {
            GUIStyle boxStyle = new(EditorStyles.helpBox);
            boxStyle.margin = new RectOffset(0, 0, 0, 5);
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
            boxStyle2.padding = new RectOffset(20, 20, 5, 5);
            boxStyle2.margin = new RectOffset(0, 0, 0, 0);

            Texture2D text = new Texture2D(1, 1);
            text.SetPixel(0, 0, style.headerColor);
            text.Apply();
            boxStyle2.normal.background = text;

            EditorGUILayout.BeginVertical(boxStyle2);
            enable = EditorGUILayout.BeginFoldoutHeaderGroup(enable, header, style: foldoutStyle);
            EditorGUILayout.EndVertical();

            if (enable)
                content(); // Draw the inspector

            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }

        public static void Title(string title, string description = "")
        {
            GUIStyle style = new(EditorStyles.boldLabel);
            EditorGUILayout.LabelField(title, style);
            if (description != "")
            {
                style = new(EditorStyles.label);
                style.normal.textColor = new Color(.6f, .6f, .6f, 1f);
                EditorGUILayout.LabelField(description, style);
            }
        }

        private static void DrawSeparator(float leftPadding, float rightPadding)
        {
            Rect rect = EditorGUILayout.GetControlRect(
                GUILayout.ExpandWidth(true),
                GUILayout.Height(1f));
            rect.xMin += -leftPadding + 1f;
            rect.xMax += rightPadding - 1f;
            EditorGUI.DrawRect(rect, new Color(1f, 1f, 1f, .1f));
        }
    }

    struct BoxStyle
    {
        public bool drawHeaderLine;
        public RectOffset margin;
        public RectOffset padding;

        public static BoxStyle Default
        {
            get => new()
            {
                drawHeaderLine = true,
                margin = new RectOffset(10, 10, 5, 5),
                padding = new RectOffset(20, 20, 5, 5)
            };
        }
    }

    struct FoldoutStyle
    {
        public Color headerColor;

        public static FoldoutStyle Default
        {
            get => new()
            {
                headerColor = new Color(0f, 0f, 0f, .15f)
            };
        }
    }
}

#endif
