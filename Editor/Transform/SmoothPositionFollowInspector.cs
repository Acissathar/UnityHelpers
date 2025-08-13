using UnityEditor;
using UnityEngine;
using UnityHelpers.Runtime.Transform;

namespace UnityHelpers.Editor.Transform
{
    [CustomEditor(typeof(SmoothPositionFollow))]
    public class SmoothPositionFollowInspector : UnityEditor.Editor
    {
        private SerializedProperty followTargetProp;
        private SerializedProperty lerpSpeedProp;
        private SerializedProperty smoothDampTimeProp;
        private SerializedProperty extrapolatePositionProp;
        private SerializedProperty smoothTypeProp;
        private SerializedProperty updateTypeProp;
        
        private void OnEnable()
        {
            followTargetProp = serializedObject.FindProperty("followTarget");
            lerpSpeedProp = serializedObject.FindProperty("lerpSpeed");
            smoothDampTimeProp = serializedObject.FindProperty("smoothDampTime");
            extrapolatePositionProp = serializedObject.FindProperty("extrapolatePosition");
            smoothTypeProp = serializedObject.FindProperty("smoothType");
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
            
            EditorGUILayout.PropertyField(smoothTypeProp, new GUIContent("Smooth Type", "Lerp: Linearly interpolates between two points.\n" +
                "SmoothDamp: Gradually changes a vector towards a desired goal over time.\nThe vector is smoothed by some spring-damper like function, which will never overshoot."));
            
            if (smoothTypeProp.enumValueIndex == 0)
            {
                EditorGUILayout.PropertyField(lerpSpeedProp, new GUIContent("Smooth Speed"));
            }
            else
            {
                EditorGUILayout.PropertyField(smoothDampTimeProp, new GUIContent("Smooth Damp Time"));
            }
            
            EditorGUILayout.PropertyField(extrapolatePositionProp, new GUIContent("Extrapolate Position", "Should position values be extrapolated to compensate for delay caused by smoothing."));
            EditorGUILayout.PropertyField(updateTypeProp, new GUIContent("Update Type", "Which update loop to run smoothing in."));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
