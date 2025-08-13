using UnityEditor;
using UnityEngine;
using UnityHelpers.Runtime.Transform;

namespace UnityHelpers.Editor.Transform
{
    [CustomEditor(typeof(SmoothRotationFollow))]
    public class SmoothRotationFollowInspector : UnityEditor.Editor
    {
        private SerializedProperty followTargetProp;
        private SerializedProperty smoothSpeedProp;
        private SerializedProperty extrapolateRotationProp;
        private SerializedProperty updateTypeProp;
        
        private void OnEnable()
        {
            followTargetProp = serializedObject.FindProperty("followTarget");
            smoothSpeedProp = serializedObject.FindProperty("smoothSpeed");
            extrapolateRotationProp = serializedObject.FindProperty("extrapolateRotation");
            updateTypeProp = serializedObject.FindProperty("updateType");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(followTargetProp, new GUIContent("Follow Target"));
            if (followTargetProp.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("If no transform is specified, component will default to setting its own transform as the target.", MessageType.Warning);
            }
            EditorGUILayout.Space();
            
            EditorGUILayout.PropertyField(smoothSpeedProp, new GUIContent("Smooth Speed"));
            
            EditorGUILayout.PropertyField(extrapolateRotationProp, new GUIContent("Extrapolate Rotation", "Should rotation values be extrapolated to compensate for delay caused by smoothing."));
            EditorGUILayout.PropertyField(updateTypeProp, new GUIContent("Update Type", "Which update loop to run smoothing in."));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
