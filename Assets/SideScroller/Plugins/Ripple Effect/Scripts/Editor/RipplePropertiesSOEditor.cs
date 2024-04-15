/*#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Magnno.RippleEffect
{
    using Utils = RippleEffectUtils;

    [CustomEditor(typeof(RipplePropertiesSO))]
    public class RipplePropertiesSOEditor : Editor
    {
        private RipplePropertiesSO properties;
        private FullScreenPassRendererFeature Feature => Utils.Feature;
        private bool isPlaying;
        private float normalizedTimer;
        private float timer;

        private void OnEnable()
        {
            if (!Feature)
                return;

            if (EditorApplication.isPlaying)
                return;

            properties = (RipplePropertiesSO)target;
            isPlaying = false;
            normalizedTimer = .5f;

            EditorApplication.update += PlayEffect;

            Utils.ToggleEffectActivation(0, false);
            Utils.ToggleEffectActivation(1, false);
            Utils.ToggleEffectActivation(2, false);
            Utils.ToggleEffectActivation(3, false);
            Utils.ToggleEffectActivation(4, false);
        }

        private void OnDisable()
        {
            if (!Feature)
                return;

            if (EditorApplication.isPlaying)
                return;

            EditorApplication.update -= PlayEffect;
            Utils.ToggleEffectActivation(0, false);
        }

        public override void OnInspectorGUI()
        {
            if (!EditorApplication.isPlaying)
            {
                Camera cam = null;
                if (SceneView.lastActiveSceneView)
                    cam = SceneView.lastActiveSceneView.camera;

                if (cam)
                {
                    normalizedTimer = EditorGUILayout.Slider(normalizedTimer, 0f, 1f);

                    if (GUILayout.Button("Play"))
                    {
                        timer = 0f;
                        isPlaying = true;
                    }

                    Utils.ToggleEffectActivation(0, true);
                    Utils.UpdateAllEffectProperties(0, normalizedTimer * properties.duration, cam.transform.position, properties);
                }
                else
                {
                    Utils.ToggleEffectActivation(0, false);
                    EditorGUILayout.HelpBox("No scene view camera found.", MessageType.Info);
                }
            }

            EditorGUILayout.Space();

            DrawDefaultInspector();
        }

        private void PlayEffect()
        {
            if (isPlaying)
            {
                timer = Mathf.Clamp(timer + Time.deltaTime * properties.velocity, 0f, properties.duration);
                normalizedTimer = Mathf.Clamp01(timer / properties.duration);
                Repaint();

                if (timer >= properties.duration)
                    isPlaying = false;
            }
        }
    }
}
#endif*/
