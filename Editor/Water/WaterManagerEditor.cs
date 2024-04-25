#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Maguinho.VFX
{
    using Utils = EditorUtilities;

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
            var so = serializedObject;

            so.Update();

            bool hasAllComponents = true;
            Utils.DrawBox("Components", () =>
            {
                so.DrawProperty("MeshFilter", "Mesh Filter", out var filter);
                so.DrawProperty("MeshRenderer", "Mesh Renderer", out var renderer);
                so.DrawProperty("RenderCamera", "Render Camera", out var camera);

                if (!filter.objectReferenceValue || !renderer.objectReferenceValue || !camera.objectReferenceValue)
                    hasAllComponents = false;
            });

            if (!hasAllComponents)
            {
                EditorGUILayout.HelpBox("Assign all the components!", MessageType.Warning);
                so.ApplyModifiedProperties();
                return;
            }

            {
                var mat = script.MeshRenderer.sharedMaterial;
                if (mat == null || mat.shader.name != "Maguinho/2DWater")
                {
                    EditorGUILayout.HelpBox("The mesh renderer component doesn't have a water material assigned!", MessageType.Warning);
                    so.ApplyModifiedProperties();
                    return;
                }
            }


            Utils.DrawBox("Mesh", () =>
            {
                so.DrawProperty("vertexCount", "Vertices Count", out _);
                so.DrawProperty("meshWidth", "Width", out _);
                so.DrawProperty("meshHeight", "Height", out _);
            });

            so.ApplyModifiedProperties();

            //base.OnInspectorGUI();
        }
    }
}
#endif
