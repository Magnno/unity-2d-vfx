#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Maguinho.VFX
{
    [CustomEditor(typeof(VFXSpriteRendererExtension))]
    public sealed class VHXSpriteRendererExtensionEditor : Editor
    {
        private VFXSpriteRendererExtension script;

        private void OnEnable()
        {
            script = (VFXSpriteRendererExtension)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // ____Content____

            BeginBox("Warp");
            DisplayProperty("_enableWarpEffect", "Enable", out var propEnableWarp);
            if (propEnableWarp.boolValue)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.Space(5f);
                DisplayProperty("_warpTilling", "Tilling", out _);
                DisplayProperty("_warpIntensity", "Intensity", out _);
                DisplayProperty("_warpSpeed", "Speed", out _);
                EditorGUILayout.Space(5f);

                EditorGUILayout.BeginVertical("HelpBox");
                EditorGUI.indentLevel--;

                DisplayProperty("_enableVerticalWarpMask", "Enable Mask", out var propEnableWarpMask);
                if (propEnableWarpMask.boolValue)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.Space(5f);
                    DisplayProperty("_verticalWarpMask", "Mask", out _);
                    DisplayProperty("_invertVerticalWarpMask", "Invert", out _);
                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel++;
                EditorGUILayout.EndVertical();

                EditorGUI.indentLevel--;
            }
            EndBox();

            EditorGUILayout.Space(2f);

            BeginBox("Color Correction");
            DisplayProperty("_enableColorCorrection", "Enable", out var propEnableColor);
            if (propEnableColor.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                DisplayProperty("_hue", "Hue", out _);
                DisplayProperty("_saturation", "Saturation", out _);
                DisplayProperty("_contrast", "Contrast", out _);
                DisplayProperty("_invertColors", "Invert Colors", out _);
                EditorGUI.indentLevel--;
            }
            EndBox();

            EditorGUILayout.Space(2f);

            BeginBox("Shake");
            DisplayProperty("_enableShake", "Enable", out var propEnableShake);
            if (propEnableShake.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);

                DisplayProperty("_shakeIntensity", "Intensity", out _);
                DisplayProperty("_shakeSpeed", "Speed", out _);

                EditorGUI.indentLevel--;
            }
            EndBox();

            EditorGUILayout.Space(2f);

            BeginBox("Orbital Movement");
            DisplayProperty("_enableOrbitalMovement", "Enable", out var propEnableOrbitalMovement);
            if (propEnableOrbitalMovement.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);

                DisplayProperty("_orbitalMovementIntensity", "Intensity", out _);
                DisplayProperty("_orbitalMovementSpeedScale", "Scale", out _);

                EditorGUI.indentLevel--;
            }
            EndBox();

            EditorGUILayout.Space(2f);

            // ____End____

            serializedObject.ApplyModifiedProperties();

            // Button to remove component
            if (GUILayout.Button("Remove"))
            {
                var sr = script.GetComponent<SpriteRenderer>();
                sr.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>("Packages/com.unity.render-pipelines.universal/Runtime/Materials/Sprite-Lit-Default.mat");
                DestroyImmediate(script);
            }
        }

        private void BeginBox(string header)
        {
            GUIStyle titleStyle = new GUIStyle
            {
                fontStyle = FontStyle.Normal,
                fontSize = 14
            };
            titleStyle.normal.textColor = Color.white;

            GUIStyle separatorStyle = new GUIStyle
            {
                margin = new RectOffset(0, 0, 4, 4),
                fixedHeight = 1
            };
            separatorStyle.normal.background = MakeTex(600, 1, new Color(1f, 1f, 1f, 0.1f));

            /*GUIStyle boxStyle = new GUIStyle
            {
                margin = new RectOffset(4, 4, 10, 10),
                padding = new RectOffset(5, 5, 5, 5),
            };
            boxStyle.normal.background = MakeTex(600, 1, new Color(0.1f, 0.1f, 0.1f, 0.2f));*/

            EditorGUILayout.BeginVertical("HelpBox");
            EditorGUI.indentLevel++;

            EditorGUILayout.LabelField(header, titleStyle); // Header
            GUILayout.Box(GUIContent.none, separatorStyle); // Line
        }

        private void EndBox()
        {
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        private void DisplayProperty(string propertyRef, string label, out SerializedProperty property)
        {
            property = serializedObject.FindProperty(propertyRef);
            EditorGUILayout.PropertyField(property, new GUIContent(label));
        }

        private Texture2D MakeTex(int width, int height, Color color)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = color;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}
#endif