using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(FlockFilter))]
public class FlockFilterInspector : Editor {

    private string[] mMasks = null;

    public override void OnInspectorGUI() {
        if(mMasks == null) {
            mMasks = M8.Editor.Utility.GenerateGenericMaskString();
        }

        FlockFilter input = target as FlockFilter;

        input.id = EditorGUILayout.IntSlider("id", input.id, 1, 32);

        input.avoidTypeFilter = EditorGUILayout.MaskField("Avoid Filter", input.avoidTypeFilter, mMasks);
    }
}
