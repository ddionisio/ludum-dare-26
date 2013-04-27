using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(AttackSensor))]
public class AttackSensorInspector : Editor {
    private string[] mMasks = null;

    public override void OnInspectorGUI() {
        if(mMasks == null) {
            mMasks = M8.Editor.Utility.GenerateGenericMaskString();
        }

        AttackSensor input = target as AttackSensor;

        input.hostileFlags = EditorGUILayout.MaskField("Hostile Flags", input.hostileFlags, mMasks);

        input.minRange = EditorGUILayout.FloatField("Min Range", input.minRange);
        input.maxRange = EditorGUILayout.FloatField("Max Range", input.maxRange);
        input.angleCheck = GUILayout.Toggle(input.angleCheck, "Angle Check");

        if(input.angleCheck)
            input.angle = EditorGUILayout.Slider("Angle", input.angle, 0.0f, 359.0f);
    }
}
