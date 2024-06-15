#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Maguinho.VFX
{
    [CustomEditor(typeof(SpriteVFX))]
    sealed class SpriteVFXEditor : Editor
    {
        private SpriteVFX script;

        private bool colorFoldout;
        private bool UVFoldout;
        private bool vertexFoldout;

        private void OnEnable()
        {
            script = (SpriteVFX)target;

            colorFoldout = serializedObject.FindProperty("_enableColorAdjustments").boolValue;
            UVFoldout = serializedObject.FindProperty("_enableWarp").boolValue;
            vertexFoldout = serializedObject.FindProperty("_enableShake").boolValue
                         || serializedObject.FindProperty("_enableSineMovement").boolValue;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            BoxStyle boxStyle = BoxStyle.Default;
            boxStyle.padding.left = 0;
            boxStyle.padding.right = 0;

            // ____Content____ //

            EditorUtils.DrawFoldout(ref colorFoldout, "Color", FoldoutStyle.Default, () =>
            {
                EditorUtils.DrawBox("Color Adjustments", boxStyle, () =>
                {
                    DrawProp("_enableColorAdjustments", "Enable", out var propEnableColor);
                    if (propEnableColor.boolValue)
                    {
                        EditorGUILayout.Space(5f);
                        DrawProp("_hue", "Hue", out _);
                        DrawProp("_saturation", "Saturation", out _);
                        DrawProp("_contrast", "Contrast", out _);
                        DrawProp("_invertColors", "Invert Colors", out _);
                    }
                });
            });

            EditorUtils.DrawFoldout(ref UVFoldout, "UV", FoldoutStyle.Default, () =>
            {
                EditorUtils.DrawBox("Warp", boxStyle, () =>
                {
                    DrawProp("_enableWarp", "Enable", out var propEnableWarp);
                    if (propEnableWarp.boolValue)
                    {
                        EditorGUILayout.Space(5f);
                        DrawProp("_warpIntensity", "Intensity", out _);
                        DrawProp("_warpScale", "Scale", out _);
                        DrawProp("_warpSpeed", "Speed", out _);

                        EditorGUILayout.Space(5f);
                        EditorUtils.Title("Vertical Mask", "Simulate wind in vegetation");
                        DrawProp("_enableVerticalWarpMask", "Enable", out var propEnableWarpMask);
                        EditorGUI.BeginDisabledGroup(!propEnableWarpMask.boolValue);
                        DrawProp("_verticalWarpMask", "Start Height", out _);
                        DrawProp("_invertVerticalWarpMask", "Invert", out _);
                        EditorGUI.EndDisabledGroup();
                    }
                });
            });

            EditorUtils.DrawFoldout(ref vertexFoldout, "Vertex", FoldoutStyle.Default, () =>
            {
                EditorUtils.DrawBox("Shake", boxStyle, () =>
                {
                    DrawProp("_enableShake", "Enable", out var propEnableShake);
                    if (propEnableShake.boolValue)
                    {
                        EditorGUILayout.Space(5f);
                        DrawProp("_shakeIntensity", "Intensity", out _);
                        DrawProp("_shakeSpeed", "Speed", out _);
                    }
                });

                EditorUtils.DrawBox("Sine Movement", boxStyle, () =>
                {
                    DrawProp("_enableSineMovement", "Enable", out var propEnableSineMovement);
                    if (propEnableSineMovement.boolValue)
                    {
                        EditorGUILayout.Space(5f);
                        DrawProp("_sineMovementIntensity", "Intensity", out _);
                        DrawProp("_sineMovementSpeed", "Speed", out _);
                        DrawProp("_sineMovementSineAndCosine", "Use sine and cosine", out _);
                    }
                });
            });

            // ____End____ //

            serializedObject.ApplyModifiedProperties();

            // Button to remove component
            if (GUILayout.Button("Remove and Reset Material"))
            {
                var sr = script.GetComponent<SpriteRenderer>();
                sr.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>("Packages/com.unity.render-pipelines.universal/Runtime/Materials/Sprite-Lit-Default.mat");
                DestroyImmediate(script);
            }
        }

        private void DrawProp(string propertyRef, string label, out SerializedProperty property)
        {
            property = serializedObject.FindProperty(propertyRef);
            EditorGUILayout.PropertyField(property, new GUIContent(label));
        }
    }
}

#endif
