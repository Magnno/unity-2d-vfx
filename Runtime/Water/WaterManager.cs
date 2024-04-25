using UnityEngine;

namespace Maguinho.VFX
{
    public sealed class WaterManager : MonoBehaviour
    {
        public MeshFilter MeshFilter;
        public MeshRenderer MeshRenderer;
        public Camera RenderCamera;

        public Material Material => MeshRenderer.sharedMaterial;

        [SerializeField, Min(2)] private int vertexCount = 20;
        [SerializeField] private float meshWidth = 20f;
        [SerializeField] private float meshHeight = 10f;

        private Mesh waterMesh;

        private Vector3[] vertices;
        private int[] triangles;
        private Vector2[] uvs;
        private Color[] colors;


        private struct Wave
        {
            public float amplitude;
            public float frequency;
            public float speed;
        }

        private Wave[] waves;
        private float wavesGlobalAmplitude;


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!MeshFilter || !MeshRenderer || !RenderCamera)
                return;

            if (!Material || Material.shader.name != "Maguinho/2DWater")
                return;

            Generate();
        }
#endif

        private void Start()
        {
            Generate();
        }

        public void Generate()
        {
            // MATERIAL MESH INFO
            Material.SetFloat("_Mesh_Width", meshWidth);
            Material.SetFloat("_Mesh_Height", meshHeight);

            // RENDER TEXTURE
            {
                Material.SetTexture("_Render_Texture", RenderCamera.targetTexture);
                Material.SetVector("_Render_Camera_Position", (Vector2)RenderCamera.transform.position);
                float YSize = RenderCamera.orthographicSize * 2f;
                Material.SetVector("_Render_Camera_Size", new Vector2(YSize * RenderCamera.aspect, YSize));
            }

            // WAVES
            waves = new Wave[6];
            for (int i = 0; i < waves.Length; i++)
            {
                waves[i].amplitude = Material.GetFloat($"_Amplitude{i + 1}");
                waves[i].frequency = Material.GetFloat($"_Frequency{i + 1}");
                waves[i].speed = Material.GetFloat($"_Speed{i + 1}");
            }
            wavesGlobalAmplitude = Material.GetFloat("_Global_Amplitude");

            // MESH
            {
                waterMesh = new Mesh();
                vertices = new Vector3[vertexCount * 2];
                triangles = new int[(vertexCount - 1) * 6];
                uvs = new Vector2[vertexCount * 2];
                colors = new Color[vertexCount * 2];

                float startX = -meshWidth / 2;
                float stepX = meshWidth / (vertexCount - 1);

                float currentX = startX;

                for (int i = 0; i < vertexCount; i++)
                {
                    vertices[i] = new Vector3(currentX, 0f, 0f);
                    vertices[i + vertexCount] = new Vector3(currentX, -meshHeight, 0f);

                    float xUVValue = currentX / meshWidth + .5f; // Remap from (-meshWidth / 2f, meshWidth / 2f) to (0, 1)
                    uvs[i] = new Vector2(xUVValue, 1f);
                    uvs[i + vertexCount] = new Vector2(xUVValue, 0f);

                    colors[i] = Color.red;
                    colors[i + vertexCount] = Color.black;

                    if (i < vertexCount - 1)
                    {
                        triangles[i * 6] = i;
                        triangles[i * 6 + 1] = i + 1;
                        triangles[i * 6 + 2] = i + vertexCount;

                        triangles[i * 6 + 3] = i + vertexCount;
                        triangles[i * 6 + 4] = i + 1;
                        triangles[i * 6 + 5] = i + vertexCount + 1;
                    }

                    currentX += stepX;
                }

                waterMesh.vertices = vertices;
                waterMesh.triangles = triangles;
                waterMesh.uv = uvs;

                // All vertices have white color
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = Color.white;
                }
                waterMesh.colors = colors;

                waterMesh.RecalculateNormals();
                MeshFilter.sharedMesh = waterMesh;
            }
        }

        /// <summary>
        /// Returns the height of the water surface at a specified horizontal position.
        /// </summary>
        /// <param name="xPos">The horizontal position at which to get the height.</param>
        public float GetHeightAtPosition(in float xPos)
        {
            float time = Time.time;
            float displacement = 0f;
            for (int i = 0; i <= waves.Length; i++)
            {
                displacement += waves[i].amplitude * Mathf.Sin(xPos * waves[i].frequency - time * waves[i].speed);
            }
            return transform.position.y + displacement * wavesGlobalAmplitude;
        }
    }
}
