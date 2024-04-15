#if UNITY_EDITOR
using UnityEditor;

namespace Maguinho.VFX
{
    public sealed class RippleEffectMaterialEditor : ShaderGUI
    {
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            EditorGUILayout.HelpBox("Edit properties using a Scriptable Object.\nCreate one via: Create > Maguinho > VFX > New Ripple Properties.", MessageType.Info);
        }
    }
}
#endif
