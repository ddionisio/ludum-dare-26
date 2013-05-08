using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(FlockFilter))]
public class FlockFilterInspector : Editor {

    private string[] mMasks = null;

    public override void OnInspectorGUI() {
        GUI.changed = false;

        if(mMasks == null) {
            mMasks = M8.Editor.Utility.GenerateGenericMaskString();
        }

        FlockFilter input = target as FlockFilter;

        input.id = EditorGUILayout.IntField("id", input.id);

        input.avoidTypeFilter = EditorGUILayout.MaskField("Avoid Filter", input.avoidTypeFilter, mMasks);

        if(GUI.changed)
            EditorUtility.SetDirty(target);
    }
}
