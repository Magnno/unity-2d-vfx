#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace PlaceHolderVFX
{
    [CustomEditor(typeof(VFXSpriteRendererExtension))]
    public sealed class VHXSpriteRendererExtensionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var script = (VFXSpriteRendererExtension)target;


            BeginBox("Warp");
            script.EnableWarpEffect = EditorGUILayout.Toggle("Enable", script.EnableWarpEffect);
            if (script.EnableWarpEffect)
            {
                script.WarpTilling = EditorGUILayout.Vector2Field("Tilling", script.WarpTilling);
                script.WarpIntensity = EditorGUILayout.Vector2Field("Intensity", script.WarpIntensity);
                script.WarpSpeed = EditorGUILayout.Vector2Field("Speed", script.WarpSpeed);
            }
            EndBox();

            BeginBox("Teste");
            EndBox();

            if (GUILayout.Button("Remove"))
            {
                var sr = script.GetComponent<SpriteRenderer>();
                sr.sharedMaterial = script.previousMat;
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

            GUIStyle boxStyle = new GUIStyle
            {
                margin = new RectOffset(4, 4, 10, 10),
                padding = new RectOffset(5, 5, 5, 5),
            };
            boxStyle.normal.background = MakeTex(600, 1, new Color(0.1f, 0.1f, 0.1f, 0.2f));

            EditorGUILayout.BeginVertical(boxStyle);
            EditorGUI.indentLevel++;

            EditorGUILayout.LabelField(header, titleStyle);
            GUILayout.Box(GUIContent.none, separatorStyle);
        }

        private void EndBox()
        {
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
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
