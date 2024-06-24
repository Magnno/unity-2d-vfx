using System.Collections.Generic;
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

        // Mesh
        [SerializeField, Min(2)] private int vertexCount = 100;
        [SerializeField] private float meshWidth = 20f;
        [SerializeField] private float meshHeight = 10f;
        [SerializeField] private Pivot pivot = Pivot.TopCenter;

        private Mesh waterMesh;
        private Vector3[] vertices;
        private int[] triangles;
        private Vector2[] uvs;
        private Color[] colors;

        // Ripple
        [SerializeField] private bool enableRipples = true;
        [SerializeField] private float rippleMaxAmplitude = 4f;
        [SerializeField, Min(0f)] private float rippleSpeed = 4f;
        [SerializeField, Min(0f)] private float rippleFrequency = 3f;
        [SerializeField, Min(0f)] private float rippleDampingOverDistance = .6f;
        [SerializeField, Min(0f)] private float rippleDampingOverTime = 1f;
        private readonly List<Ripple> ripples = new();

        private Wave[] waves;


        private enum Pivot
        {
            TopLeft,
            TopCenter
        }


        public Material Material => MeshRenderer.sharedMaterial;

        /// <summary>
        /// The amplitude multiplier of the waves.
        /// </summary>
        public float GlobalAmplitude
        {
            get => _globalAmplitude;
            set { _globalAmplitude = value; Material.SetFloat("_Global_Amplitude", _globalAmplitude); }
        }
        private float _globalAmplitude;

        /// <summary>
        /// The speed multiplier of the waves.
        /// </summary>
        public float GlobalSpeed
        {
            get => _globalSpeed;
            set { _globalSpeed = value; Material.SetFloat("_Global_Speed", _globalSpeed); }
        }
        private float _globalSpeed;


        /// <summary>
        /// Gets the height of the waves at a specified horizontal position.
        /// </summary>
        /// <param name="xPos">The horizontal position at which to get the height (in World Space).</param>
        /// <returns>
        /// The height of the waves at the specified position.<br/>
        /// Returns <c>Mathf.NegativeInfinity</c> if the position is outside the surface bounds.
        /// </returns>
        public float GetHeightAt(in float xPos)
        {
            Vector2 _pos = transform.position;
            float minX, maxX;

            switch (pivot)
            {
                case Pivot.TopLeft:
                    minX = _pos.x;
                    maxX = _pos.x + meshWidth;
                    break;

                case Pivot.TopCenter:
                    float halfWidth = meshWidth / 2f;
                    minX = _pos.x - halfWidth;
                    maxX = _pos.x + halfWidth;
                    break;

                default:
                    return Mathf.NegativeInfinity;
            }

            if (xPos < minX || xPos > maxX)
                return Mathf.NegativeInfinity;

            float time = Time.time;
            float displacement = 0f;
            foreach (var wave in waves)
            {
                displacement += wave.amplitude * Mathf.Sin(xPos * wave.frequency - time * wave.speed * GlobalSpeed);
            }

            return transform.position.y + displacement * GlobalAmplitude;
        }

        /// <summary>
        /// Adds a ripple effect to the water surface.
        /// </summary>
        /// <param name="xPos">The x-coordinate of the origin of the ripple.</param>
        /// <param name="initialAmplitude">The initial amplitude (height) of the ripple. This value is clamped to the range defined by <c>rippleMaxAmplitude</c>.</param>
        /// <param name="minDuration">
        /// The minimum duration for the ripple to persist. The ripple will be deleted when its maximum amplitude falls below the threshold defined by <c>Ripple.MIN_AMPLITUDE</c>.
        /// Setting a minimum duration ensures the ripple is not deleted immediately after it starts.
        /// </param>
        public void AddRipple(in float xPos, in float initialAmplitude, in float minDuration = 1f)
        {
            ripples.Add(new(xPos,
                            Mathf.Clamp(initialAmplitude, -rippleMaxAmplitude, rippleMaxAmplitude),
                            rippleSpeed,
                            rippleFrequency,
                            rippleDampingOverDistance,
                            rippleDampingOverTime,
                            minDuration));
        }

        private void UpdateRipples()
        {
            if (!enableRipples)
                return;

            // Clear vertices y pos
            for (int i = 0; i < vertexCount; i++)
            {
                vertices[i].y = 0f;
            }

            for (int i = ripples.Count - 1; i >= 0; i--)
            {
                Ripple ripple = ripples[i];
                float maxDisplacement = 0f;

                for (int j = 0; j < vertexCount; j++)
                {
                    float displacement = ripple.GetDisplacementAt(vertices[j].x + transform.position.x);

                    float absDisplacement = Mathf.Abs(displacement);
                    if (absDisplacement > maxDisplacement)
                        maxDisplacement = absDisplacement;

                    vertices[j].y += displacement;
                }

                if (ripple.CanDelete(maxDisplacement))
                    ripples.RemoveAt(i);
            }

            // Update the mesh
            waterMesh.vertices = vertices;
        }

        private void GenerateMesh()
        {
            waterMesh = new Mesh();
            vertices = new Vector3[vertexCount * 2];
            triangles = new int[(vertexCount - 1) * 6];
            uvs = new Vector2[vertexCount * 2];
            colors = new Color[vertexCount * 2];

            float startX;
            if (pivot == Pivot.TopLeft)
                startX = 0f;
            else
                startX = -meshWidth / 2;

            float stepX = meshWidth / (vertexCount - 1);

            float currentX = startX;

            for (int i = 0; i < vertexCount; i++)
            {
                vertices[i] = new Vector3(currentX, 0f, 0f);
                vertices[i + vertexCount] = new Vector3(currentX, -meshHeight, 0f);

                float xUVValue;
                if (pivot == Pivot.TopLeft)
                    xUVValue = currentX / meshWidth;
                else
                    xUVValue = currentX / meshWidth + .5f; // Remap from (-meshWidth / 2f, meshWidth / 2f) to (0, 1)

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

        private void UpdateRenderTexture()
        {
            Material.SetTexture("_Render_Texture", RenderCamera.targetTexture);
            Material.SetVector("_Render_Camera_Position", (Vector2)RenderCamera.transform.position);
            float YSize = RenderCamera.orthographicSize * 2f;
            Material.SetVector("_Render_Camera_Size", new Vector2(YSize * RenderCamera.aspect, YSize));
        }

        private void GetWavesFromMaterial()
        {
            bool enabled = Material.GetFloat("_Enable_Waves") == 1f;

            waves = new Wave[6];
            for (int i = 0; i < waves.Length; i++)
            {
                if (!enabled)
                {
                    waves[i].amplitude = 0f;
                    continue;
                }

                waves[i].amplitude = Material.GetFloat($"_Amplitude{i + 1}");
                waves[i].frequency = Material.GetFloat($"_Frequency{i + 1}");
                waves[i].speed = Material.GetFloat($"_Speed{i + 1}");
            }
            _globalAmplitude = Material.GetFloat("_Global_Amplitude");
            _globalSpeed = Material.GetFloat("_Global_Speed");
        }

        private bool IsMissingComponents()
        {
            bool hasAllComponents = MeshFilter && MeshRenderer && RenderCamera && Material && Material.shader.name == "Maguinho/2DWater";
            if (!hasAllComponents)
            {
#if UNITY_EDITOR
                Debug.LogError($"[GameObject: {gameObject.name}] There are missing components in WaterManager.");
#endif
                return true;
            }
            return false;
        }


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
            // Disable the go if its missing components
            if (IsMissingComponents())
            {
                gameObject.SetActive(false);
                return;
            }

            // Generate the water
            GenerateMesh();
            UpdateRenderTexture();
            GetWavesFromMaterial();
        }

        private void Update()
        {
#if UNITY_EDITOR
            // Called in the editor when not playing
            if (!Application.IsPlaying(gameObject) && !IsMissingComponents())
            {
                // Synchronize the render texture with the position of the water in the scene.
                UpdateRenderTexture();
                return;
            }
#endif
            UpdateRipples();
        }


        private struct Wave
        {
            public float amplitude;
            public float frequency;
            public float speed;
        }

        private sealed class Ripple
        {
            public const float MIN_DISPLACEMENT = .01f;

            private readonly float initialXPos;
            private readonly float initialAmplitude;

            private readonly float timeSpeed;
            private readonly float frequency;

            private readonly float dampingOverDistance;
            private readonly float dampingOverTime;

            private readonly float minDuration;

            private readonly float initialTime = Time.time;

            private float ElapsedTime => (Time.time - initialTime) * timeSpeed;

            public Ripple(float initialXPos, float initialAmplitude, float timeSpeed, float frequency, float dampingOverDistance, float dampingOverTime, float minDuration)
            {
                this.initialXPos = initialXPos;
                this.initialAmplitude = initialAmplitude;
                this.timeSpeed = Mathf.Max(timeSpeed, 0f);
                this.frequency = Mathf.Max(frequency, 0f);
                this.dampingOverDistance = Mathf.Max(dampingOverDistance, 0f);
                this.dampingOverTime = Mathf.Max(dampingOverTime, 0f);
                this.minDuration = minDuration;
            }

            public float GetDisplacementAt(in float xPos)
            {
                float distanceFromCenter = Mathf.Abs(xPos - initialXPos);

                float time = ElapsedTime - distanceFromCenter;
                if (time <= 0f)
                    return 0f;

                return
                    initialAmplitude *
                    Mathf.Exp(-dampingOverDistance * distanceFromCenter) *
                    Mathf.Exp(-dampingOverTime * time) *
                    Mathf.Sin(time * frequency);
            }

            public bool CanDelete(in float maxDisplacement)
            {
                return ElapsedTime > minDuration && maxDisplacement < MIN_DISPLACEMENT;
            }
        }
    }
}
