#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Maguinho.VFX
{
    static class CreateGOEditor
    {
        const string PATH = "GameObject/2D Object/Maguinho/";

        [MenuItem(PATH + "Sprite VFX")]
        static void CreateSprite()
        {
            var go = new GameObject("Sprite VFX");

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/Square.png");

            go.AddComponent<SpriteVFX>();

            Selection.activeGameObject = go;
        }

        [MenuItem(PATH + "Water")]
        static void CreateWater()
        {
            // Create render texture
            RenderTexture renderTexture = new RenderTexture(960, 540, 0)
            {
                dimension = TextureDimension.Tex2D,
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp,
                name = "WaterRenderTexture"
            };

            // Create water material
            Material material = new Material(Shader.Find("Maguinho/2DWater"));
            material.name = "WaterMaterial";

            // Create water game object
            GameObject waterGO = new GameObject("Water");
            waterGO.layer = LayerMask.NameToLayer("Water");

            // Add components to water game object
            var waterManager = waterGO.AddComponent<WaterManager>();
            var filter = waterGO.AddComponent<MeshFilter>();
            var renderer = waterGO.AddComponent<MeshRenderer>();
            waterGO.AddComponent<SortingGroup>();

            // Configure mesh renderer component
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            renderer.sharedMaterial = material;

            // Create camera game object
            GameObject camGO = new GameObject("Render Camera");
            camGO.transform.SetParent(waterGO.transform, false);
            camGO.transform.localPosition = new Vector3(0f, 0f, -10f);

            // Add camera component and configure
            var cam = camGO.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 8f;
            cam.cullingMask &= ~(1 << LayerMask.NameToLayer("Water")); // Removes the water layer from rendering in the render texture camera
            cam.targetTexture = renderTexture;

            // Configure water manager component
            waterManager.MeshFilter = filter;
            waterManager.MeshRenderer = renderer;
            waterManager.RenderCamera = cam;

            // Save assets to new folder
            {
                string scenePath = Path.GetDirectoryName(SceneManager.GetActiveScene().path);
                string sceneName = SceneManager.GetActiveScene().name;
                string sceneFolderPath = Path.Combine(scenePath, sceneName);

                if (!AssetDatabase.IsValidFolder(sceneFolderPath))
                    AssetDatabase.CreateFolder(scenePath, sceneName);

                int i = 0;
                while (AssetDatabase.IsValidFolder(Path.Combine(sceneFolderPath, "Water" + i)))
                {
                    i++;
                }
                AssetDatabase.CreateFolder(sceneFolderPath, "Water" + i);
                string savePath = Path.Combine(sceneFolderPath, "Water" + i);

                AssetDatabase.CreateAsset(renderTexture, Path.Combine(savePath, "WaterRenderTexture.renderTexture"));
                AssetDatabase.CreateAsset(material, Path.Combine(savePath, "WaterMaterial.mat"));

                Debug.Log("Created new 'Water' folder: " + savePath);
            }

            Selection.activeGameObject = waterGO;
        }
    }
}

#endif
