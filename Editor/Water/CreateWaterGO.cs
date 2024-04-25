#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.IO;

namespace Maguinho.VFX
{
    public sealed class CreateWaterGO : MonoBehaviour
    {
        [MenuItem("GameObject/2D Object/Water", false, 10)]
        static void Create()
        {
            GameObject GO = new GameObject("Water");
            GO.layer = 4; // Sets the water game object in the Water Layer

            var water = GO.AddComponent<WaterManager>();
            var filter = GO.AddComponent<MeshFilter>();
            var renderer = GO.AddComponent<MeshRenderer>();
            GO.AddComponent<SortingGroup>();

            GameObject camGO = new GameObject("Render Camera");
            camGO.transform.SetParent(GO.transform, false);
            camGO.transform.localPosition = new Vector3(0f, 0f, -10f);

            var cam = camGO.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 8f;
            cam.cullingMask &= ~(1 << 4); // Removes the Water Layer from rendering

            water.MeshFilter = filter;
            water.MeshRenderer = renderer;
            water.RenderCamera = cam;

            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;

            // Render texture
            RenderTexture renderTexture = new RenderTexture(960, 540, 0)
            {
                dimension = TextureDimension.Tex2D,
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp,
                name = "WaterRenderTexture"
            };
            cam.targetTexture = renderTexture;

            // Material
            Material material = new Material(Shader.Find("Maguinho/2DWater"));
            material.name = "WaterMaterial";
            renderer.sharedMaterial = material;

            // Save assets
            string scenePath = Path.GetDirectoryName(SceneManager.GetActiveScene().path);
            string sceneName = SceneManager.GetActiveScene().name;
            if (!AssetDatabase.IsValidFolder($"{scenePath}/{sceneName}"))
                AssetDatabase.CreateFolder(scenePath, sceneName);

            AssetDatabase.CreateAsset(renderTexture, $"{scenePath}/{sceneName}/WaterRenderTexture.renderTexture");
            AssetDatabase.CreateAsset(material, $"{scenePath}/{sceneName}/WaterMaterial.mat");


            water.Generate();

            Selection.activeGameObject = GO;
        }
    }
}
#endif
