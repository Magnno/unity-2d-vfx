#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Maguinho.VFX
{
    sealed class SpriteVFXMaterialEditor : ShaderGUI
    {
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            GUILayout.Label("Use the 'SpriteVFX' script inspector to set parameters.", new GUIStyle(EditorStyles.largeLabel));

            //base.OnGUI(materialEditor, properties);
        }
    }
}

#endif
