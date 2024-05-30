#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Maguinho.VFX
{
    [CustomEditor(typeof(WaterManager))]
    public sealed class WaterManagerEditor : Editor
    {
        private WaterManager script;

        private void OnEnable()
        {
            script = (WaterManager)target;
        }

        public override void OnInspectorGUI()
        {
            void DrawProp(string reference, string label, out SerializedProperty prop)
            {
                prop = serializedObject.FindProperty(reference);
                EditorGUILayout.PropertyField(prop, new GUIContent { text = label });
            }

            void Space(float space = 5f)
            {
                EditorGUILayout.Space(space);
            }

            serializedObject.Update();

            bool hasAllComponents = script.MeshFilter && script.MeshRenderer && script.RenderCamera && script.MeshRenderer.sharedMaterial && script.MeshRenderer.sharedMaterial.shader.name == "Maguinho/2DWater";
            if (!hasAllComponents)
            {
                EditorGUILayout.HelpBox("Missing components!", MessageType.Error);
                Space(1f);
            }

            EditorUtilities.DrawBox("Components", () =>
            {
                DrawProp("MeshFilter", "Mesh Filter", out var filter);
                DrawProp("MeshRenderer", "Mesh Renderer", out var renderer);
                DrawProp("RenderCamera", "Render Camera", out var camera);
            });

            Space(1f);

            EditorUtilities.DrawBox("Mesh", () =>
            {
                DrawProp("vertexCount", "Vertices Count", out _);
                DrawProp("meshWidth", "Width", out _);
                DrawProp("meshHeight", "Height", out _);
                DrawProp("pivot", "Pivot", out _);
            });

            Space(1f);

            EditorUtilities.DrawBox("Ripple", () =>
            {
                DrawProp("enableRipples", "Enable", out var prop);
                EditorGUI.BeginDisabledGroup(!prop.boolValue);
                Space(2f);

                DrawProp("rippleMaxAmplitude", "Max Amplitude", out _);
                DrawProp("rippleSpeed", "Speed", out _);
                DrawProp("rippleFrequency", "Frequency", out _);
                DrawProp("rippleDampingOverDistance", "Damping Over Distance", out _);
                DrawProp("rippleDampingOverTime", "Damping Over Time", out _);
                EditorGUI.EndDisabledGroup();
            });

            serializedObject.ApplyModifiedProperties();

            //base.OnInspectorGUI();
        }
    }
}

#endif
