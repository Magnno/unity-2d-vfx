using UnityEngine;

namespace Maguinho.VFX
{
    [ExecuteAlways]
    public sealed class WaterManager : MonoBehaviour
    {
        // Components
        public MeshFilter MeshFilter;
        public MeshRenderer MeshRenderer;
        public Camera RenderCamera;

        public Material Material => MeshRenderer.sharedMaterial;

        // Mesh
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

        private float _globalAmplitude;
        /// <summary>
        /// The amplitude multiplier of the waves.
        /// </summary>
        public float GlobalAmplitude
        {
            get { return _globalAmplitude; }
            set { _globalAmplitude = value; Material.SetFloat("_Global_Amplitude", _globalAmplitude); }
        }

        private float _globalSpeed;
        /// <summary>
        /// The speed multiplier of the waves.
        /// </summary>
        public float GlobalSpeed
        {
            get { return _globalSpeed; }
            set { _globalSpeed = value; Material.SetFloat("_Global_Speed", _globalSpeed); }
        }


        // Editor only
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (IsMissingComponents())
                return;

            GenerateMesh();
        }
#endif

        private void Start()
        {
            if (IsMissingComponents())
            {
#if UNITY_EDITOR
                Debug.LogError($"There are missing components in Water Manager component ({gameObject.name})");
#endif
                return;
            }

            GenerateMesh();
            SetRenderTexture();
            GetWavesFromMaterial();
        }

        // Editor only
#if UNITY_EDITOR
        private void Update()
        {
            // Called in the editor when not playing
            if (!Application.IsPlaying(gameObject) && !IsMissingComponents())
            {
                // Synchronize the render texture with the position of the water in the scene.
                SetRenderTexture();
                return;
            }
        }
#endif

        private void GenerateMesh()
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

            Material.SetFloat("_Mesh_Width", meshWidth);
            Material.SetFloat("_Mesh_Height", meshHeight);
        }

        private void SetRenderTexture()
        {
            Material.SetTexture("_Render_Texture", RenderCamera.targetTexture);
            Material.SetVector("_Render_Camera_Position", (Vector2)RenderCamera.transform.position);
            float YSize = RenderCamera.orthographicSize * 2f;
            Material.SetVector("_Render_Camera_Size", new Vector2(YSize * RenderCamera.aspect, YSize));
        }

        private void GetWavesFromMaterial()
        {
            waves = new Wave[6];
            for (int i = 0; i < waves.Length; i++)
            {
                waves[i].amplitude = Material.GetFloat($"_Amplitude{i + 1}");
                waves[i].frequency = Material.GetFloat($"_Frequency{i + 1}");
                waves[i].speed = Material.GetFloat($"_Speed{i + 1}");
            }
            GlobalAmplitude = Material.GetFloat("_Global_Amplitude");
            GlobalSpeed = Material.GetFloat("_Global_Speed");
        }

        private bool IsMissingComponents()
        {
            bool hasAllComponents = MeshFilter && MeshRenderer && RenderCamera && Material && Material.shader.name == "Maguinho/2DWater";
            //if (!hasAllComponents)
            //    Debug.LogError($"There are missing components in '{gameObject.name} / Water Manager'.");
            return !hasAllComponents;
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
                displacement += waves[i].amplitude * Mathf.Sin(xPos * waves[i].frequency - time * waves[i].speed * GlobalSpeed);
            }
            return transform.position.y + displacement * GlobalAmplitude;
        }
    }
}
