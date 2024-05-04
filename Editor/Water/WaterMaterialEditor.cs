#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Maguinho.VFX
{
    public sealed class WaterMaterialEditor : ShaderGUI
    {
        public override void OnGUI(MaterialEditor editor, MaterialProperty[] properties)
        {
            void DrawLabel(string label)
            {
                GUIStyle style = new GUIStyle();
                style.fontSize = 14;
                //style.fontStyle = FontStyle.Bold;
                style.normal.textColor = new Color(.85f, .85f, .85f, 1f);
                EditorGUILayout.LabelField(label, style);
            }

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
                    if (EditorGUI.EndChangeCheck())
                    {
                        prop.vectorValue = value;
                    }
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


            EditorUtilities.DrawBox("Colors", () =>
            {
                DrawProp("_Top_Color", "Top", out _);
                DrawProp("_Bottom_Color", "Bottom", out _);
                Space();
                DrawProp("_Color_Fading", "Fading", out _);
                DrawProp("_Color_Norm_Height", "Height", out _);
            });

            Space(1f);

            EditorUtilities.DrawBox("Surface Line", () =>
            {
                DrawProp("_Enable_Surface_Line", "Enable", out var prop);
                if (prop.floatValue == 1f)
                {
                    Space();
                    DrawProp("_Surface_Line_Color", "Color", out _);
                    DrawProp("_Surface_Line_Length", "Length", out _);
                }
            });

            Space(1f);

            EditorUtilities.DrawBox("Underwater", () =>
            {
                DrawProp("_Enable_Underwater_Render", "Enable", out var prop);
                if (prop.floatValue == 1f)
                {
                    Space();
                    DrawProp("_Underwater_Opacity", "Opacity", out _);

                    Space();
                    //EditorGUILayout.LabelField("Refraction", EditorStyles.boldLabel);
                    DrawLabel("Refraction");
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
                }
            });

            Space(1f);

            EditorUtilities.DrawBox("Reflection", () =>
            {
                DrawProp("_Enable_Reflection", "Enable", out var prop);
                if (prop.floatValue == 1f)
                {
                    Space();
                    DrawProp("_Reflection_Opacity", "Opacity", out _);
                    DrawProp("_Reflection_Norm_Height", "Height", out _);
                    DrawProp("_Reflection_Fading", "Fading", out _);
                    DrawProp("_Reflection_Scale", "Scale", out _);

                    Space();
                    DrawLabel("Warp");
                    EditorGUILayout.LabelField("X Axis", EditorStyles.boldLabel);
                    DrawProp("_Reflection_X_Warp_Intensity", "Intensity", out _);
                    DrawProp("_Reflection_X_Warp_Frequency", "Frequency", out _);
                    DrawProp("_Reflection_X_Warp_Speed", "Speed", out _);

                    Space();
                    EditorGUILayout.LabelField("Y Axis", EditorStyles.boldLabel);
                    DrawProp("_Reflection_Y_Warp_Intensity", "Intensity", out _);
                    DrawProp("_Reflection_Y_Warp_Frequency", "Frequency", out _);
                    DrawProp("_Reflection_Y_Warp_X_Multiplier", "X Speed Multiplier", out _);
                    DrawVector2Prop("_Reflection_Y_Warp_Speed", "Speed");
                }
            });

            Space(1f);

            EditorUtilities.DrawBox("Waves", () =>
            {
                DrawProp("_Enable_Waves", "Enable", out var prop);
                if (prop.floatValue == 1f)
                {
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
                }
            });

            //base.OnGUI(editor, properties);
        }
    }
}

#endif
