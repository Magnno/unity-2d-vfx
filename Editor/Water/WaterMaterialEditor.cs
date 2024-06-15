#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Maguinho.VFX
{
    sealed class WaterMaterialEditor : ShaderGUI
    {
        private bool FColors = false;
        private bool FSurfaceLine = false;
        private bool FUnderwater = false;
        private bool FReflection = false;
        private bool FWaves = false;

        public override void OnGUI(MaterialEditor editor, MaterialProperty[] properties)
        {
            #region Methods

            void DrawProp(string reference, string label, out MaterialProperty prop)
            {
                prop = FindProperty(reference, properties);
                if (prop != null)
                    editor.ShaderProperty(prop, new GUIContent { text = label });
                else
                    Debug.LogError("Coldn't find reference in material: " + reference);
            }

            void DrawVector2Prop(string reference, string label)
            {
                MaterialProperty prop = FindProperty(reference, properties);
                if (prop != null)
                {
                    EditorGUI.BeginChangeCheck();
                    Vector2 value = EditorGUILayout.Vector2Field(label, prop.vectorValue);
                    prop.vectorValue = value;
                    EditorGUI.EndChangeCheck();
                }
            }

            void DrawWave(int id)
            {
                EditorGUILayout.LabelField("Wave " + id, EditorStyles.boldLabel);
                DrawProp("_Amplitude" + id, "Amplitude", out _);
                DrawProp("_Frequency" + id, "Frequency", out _);
                DrawProp("_Speed" + id, "Speed", out _);
            }

            void Space(float space = 5f)
            {
                EditorGUILayout.Space(space);
            }
            #endregion

            EditorUtils.DrawBox("Rendering", BoxStyle.Default, () =>
            {
                DrawProp("_Enable_Lit", "Enable Lit", out _);
                editor.RenderQueueField();
            });

            EditorUtils.DrawFoldout(ref FColors, "Colors", FoldoutStyle.Default, () =>
            {
                DrawProp("_Top_Color", "Top", out _);
                DrawProp("_Bottom_Color", "Bottom", out _);
                Space();
                DrawProp("_Color_Height", "Height", out _);
                DrawProp("_Color_Transition_Length", "Transition Length", out _);
                DrawProp("_Color_Transition_Fading", "Transition Fading", out _);
            });

            EditorUtils.DrawFoldout(ref FSurfaceLine, "Surface Line", FoldoutStyle.Default, () =>
            {
                DrawProp("_Enable_Surface_Line", "Enable", out var prop);
                EditorGUI.BeginDisabledGroup(prop.floatValue != 1f);
                Space();
                DrawProp("_Surface_Line_Color", "Color", out _);
                DrawProp("_Surface_Line_Length", "Length", out _);
                EditorGUI.EndDisabledGroup();
            });

            EditorUtils.DrawFoldout(ref FUnderwater, "Underwater", FoldoutStyle.Default, () =>
            {
                DrawProp("_Enable_Underwater_Render", "Enable", out var prop);
                EditorGUI.BeginDisabledGroup(prop.floatValue != 1f);

                Space();
                EditorUtils.Title("Opacity");
                DrawProp("_Underwater_Enable_Opacity_Mask", "Enable Mask", out var maskProp);
                if (maskProp.floatValue == 1f)
                {
                    DrawProp("_Underwater_Opacity", "Top Opacity", out _);
                    DrawProp("_Underwater_Bottom_Opacity", "Bottom Opacity", out _);
                    DrawProp("_Underwater_Mask_Length", "Mask Height", out _);
                    DrawProp("_Underwater_Mask_Fading", "Mask Fading", out _);
                }
                else
                    DrawProp("_Underwater_Opacity", "Opacity", out _);

                Space();
                //EditorGUILayout.LabelField("Refraction", EditorStyles.boldLabel);
                EditorUtils.Title("Refraction");
                DrawProp("_Underwater_Refraction_Intensity", "Intensity", out _);
                DrawProp("_Underwater_Refraction_Speed", "Speed", out _);
                DrawProp("_Underwater_Refraction_Scale", "Scale", out _);

                /*
                Space();
                EditorUtilities.DrawBox("Caustics", () =>
                {
                    DrawProp("_Enable_Caustics", "Enable", out var causticsProp);
                    if (causticsProp.floatValue == 1f)
                    {
                        Space();
                        DrawProp("_Caustics_Texture", "Texture", out _);
                        DrawProp("_Caustics_Tilling", "Tilling", out _);
                        DrawProp("_Caustics_Intensity", "Intensity", out _);
                        DrawProp("_Caustics_Speed", "Speed", out _);
                        DrawProp("_Caustics_Power", "Fading", out _);
                        DrawProp("_Caustics_Distortion_Intensity", "Distortion Intensity", out _);
                        DrawProp("_Caustics_Distortion_Scale", "Distortion Scale", out _);
                        DrawProp("_Caustics_Distortion_Speed", "Distortion Speed", out _);
                    }
                }, indentLvl: 0);
                */

                EditorGUI.EndDisabledGroup();
            });

            EditorUtils.DrawFoldout(ref FReflection, "Reflection", FoldoutStyle.Default, () =>
            {
                DrawProp("_Enable_Reflection", "Enable", out var prop);
                EditorGUI.BeginDisabledGroup(prop.floatValue != 1f);

                Space();
                DrawProp("_Reflection_Opacity", "Opacity", out _);
                DrawProp("_Reflection_Scale", "Scale", out _);

                Space();
                EditorUtils.Title("Mask");
                DrawProp("_Reflection_Mask_Height", "Height", out _);
                DrawProp("_Reflection_Mask_Fading", "Fading", out _);

                Space();
                EditorUtils.Title("Simple Turbulence");
                DrawProp("_Reflection_Turbulence_Intensity", "Intensity", out _);
                DrawProp("_Reflection_Turbulence_Speed", "Speed", out _);
                DrawProp("_Reflection_Turbulence_Scale", "Scale", out _);

                Space();
                EditorUtils.Title("Directional Turbulence");
                DrawProp("_Reflection_Warp_Intensity", "Intensity", out _);
                DrawVector2Prop("_Reflection_Warp_Speed", "Speed");
                DrawVector2Prop("_Reflection_Warp_Scale", "Scale");

                EditorGUI.EndDisabledGroup();
            });

            EditorUtils.DrawFoldout(ref FWaves, "Waves", FoldoutStyle.Default, () =>
            {
                if (Application.isPlaying)
                {
                    EditorGUILayout.HelpBox("Edit waves only when not in play mode.\nYou can still set 'Global Amplitude' and 'Global Speed' properties by code.", MessageType.Info);
                    EditorGUI.BeginDisabledGroup(true);
                }

                DrawProp("_Enable_Waves", "Enable", out var prop);

                if (!Application.isPlaying)
                    EditorGUI.BeginDisabledGroup(prop.floatValue != 1f);

                Space();
                DrawProp("_Global_Amplitude", "Global Amplitude", out _);
                DrawProp("_Global_Speed", "Global Speed", out _);

                Space();
                DrawWave(1);

                Space();
                DrawWave(2);

                Space();
                DrawWave(3);

                Space();
                DrawWave(4);

                Space();
                DrawWave(5);

                Space();
                DrawWave(6);

                EditorGUI.EndDisabledGroup();
            });

            //base.OnGUI(editor, properties);
        }
    }
}

#endif
