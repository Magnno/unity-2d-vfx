#if UNITY_EDITOR

using System;
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

        //private float genWaveAmpScale;
        //private float genWaveBaseFreq;
        //private float genWaveBaseSpeed;

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

            void DrawToggle(string reference, string label, out MaterialProperty prop)
            {
                prop = FindProperty(reference, properties);
                if (prop != null)
                {
                    bool toggleValue = prop.floatValue != 0;
                    toggleValue = EditorGUILayout.Toggle(new GUIContent { text = label }, toggleValue);
                    prop.floatValue = toggleValue ? 1.0f : 0.0f;
                }
                else
                    Debug.LogError("Couldn't find reference in material: " + reference);
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

            void Space(float space = 5f)
            {
                EditorGUILayout.Space(space);
            }
            #endregion

            EditorUtils.DrawBox("Rendering", BoxStyle.Default, () =>
            {
                DrawToggle("_Enable_Lit", "Enable Lit", out _);
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
                DrawToggle("_Enable_Surface_Line", "Enable", out var prop);
                EditorGUI.BeginDisabledGroup(prop.floatValue != 1f);
                Space();
                DrawProp("_Surface_Line_Color", "Color", out _);
                DrawProp("_Surface_Line_Length", "Length", out _);
                EditorGUI.EndDisabledGroup();
            });

            EditorUtils.DrawFoldout(ref FUnderwater, "Underwater", FoldoutStyle.Default, () =>
            {
                DrawToggle("_Enable_Underwater_Render", "Enable", out var prop);
                EditorGUI.BeginDisabledGroup(prop.floatValue != 1f);

                Space();
                EditorUtils.Title("Opacity");
                DrawToggle("_Underwater_Enable_Opacity_Mask", "Enable Mask", out var maskProp);
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
                DrawToggle("_Enable_Reflection", "Enable", out var prop);
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

                DrawToggle("_Enable_Waves", "Enable", out var prop);

                if (!Application.isPlaying)
                    EditorGUI.BeginDisabledGroup(prop.floatValue != 1f);

                Space();
                DrawProp("_Global_Amplitude", "Global Amplitude", out _);
                DrawProp("_Global_Speed", "Global Speed", out _);

                // too dumb to make it work

                //BoxStyle boxStyle = BoxStyle.Default;
                //boxStyle.padding.left = 0;
                //boxStyle.padding.right = 0;
                //boxStyle.margin.top = 10;
                //EditorUtils.DrawBox("Generate Waves", boxStyle, () =>
                //{
                //    genWaveAmpScale = EditorGUILayout.FloatField("Amplitude Scale", genWaveAmpScale);
                //    genWaveBaseFreq = EditorGUILayout.FloatField("Base Frequency", genWaveBaseFreq);
                //    genWaveBaseSpeed = EditorGUILayout.FloatField("Base Speed", genWaveBaseSpeed);

                //    if (GUILayout.Button("Generate Wave Pattern"))
                //    {
                //        System.Random random = new();

                //        for (int i = 1; i < 7; i++)
                //        {
                //            float amplitude = 1f / i;
                //            float frequency = genWaveBaseFreq / amplitude;

                //            float speed = frequency;
                //            if (i == 6)
                //                speed = -frequency;

                //            FindProperty("_Amplitude" + i, properties).floatValue = amplitude;
                //            FindProperty("_Frequency" + i, properties).floatValue = frequency;
                //            FindProperty("_Speed" + i, properties).floatValue = speed;
                //        }
                //    }
                //});

                for (int i = 1; i < 7; i++)
                {
                    Space();
                    EditorGUILayout.LabelField("Wave " + i, EditorStyles.boldLabel);
                    DrawProp("_Amplitude" + i, "Amplitude", out _);
                    DrawProp("_Frequency" + i, "Frequency", out _);
                    DrawProp("_Speed" + i, "Speed", out _);
                }

                EditorGUI.EndDisabledGroup();
            });

            //base.OnGUI(editor, properties);
        }
    }
}

#endif
