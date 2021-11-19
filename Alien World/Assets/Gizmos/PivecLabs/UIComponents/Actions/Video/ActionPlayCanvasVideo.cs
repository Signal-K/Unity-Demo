namespace GameCreator.UIComponents
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using GameCreator.Core;
    using GameCreator.Variables;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    [AddComponentMenu("")]
    public class ActionPlayCanvasVideo : IAction
    {

        public GameObject targetObject;
        public NumberProperty firstframe = new NumberProperty(0.0f);
        public bool loopVideo = false;
        public bool waitforFirst = true;

        [Range(0.0f, 10.0f)] public float speed = 1.0f;
        [Range(0.0f, 1.0f)] public float volume = 1.0f;
        [Range(0.0f, 1.0f)] public float blend = 0.0f;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

            var audioSource = targetObject.GetComponent<AudioSource>();

            var vp = targetObject.GetComponent<UnityEngine.Video.VideoPlayer>();
            if (loopVideo)
            {
                vp.isLooping = true;
            }

            audioSource.spatialBlend = blend;
	        audioSource.priority = 0;
            audioSource.volume = volume;

            vp.waitForFirstFrame = waitforFirst;

            vp.playbackSpeed = speed;

            long value = (long)this.firstframe.GetValue(target);
         
            vp.frame = value;

            vp.Play();

            return true;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {

            return base.Execute(target, actions, index);
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

        public static new string NAME = "UI/Video/Play Video on Canvas";
        private const string NODE_TITLE = "Play Video on {0}";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptargetObject;
        private SerializedProperty spLoopVideo;
        private SerializedProperty spSpeed;
        private SerializedProperty spFirstFrame;
        private SerializedProperty spVolume;
        private SerializedProperty spBlend;
        private SerializedProperty spwaitforFirst;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {
            return string.Format(
                 NODE_TITLE,
                 (targetObject == null ? "none" : targetObject.name)
             );
        }

        protected override void OnEnableEditorChild()
        {
            this.sptargetObject = this.serializedObject.FindProperty("targetObject");
            this.spLoopVideo = this.serializedObject.FindProperty("loopVideo");
            this.spSpeed = this.serializedObject.FindProperty("speed");
            this.spFirstFrame = this.serializedObject.FindProperty("firstframe");
            this.spVolume = this.serializedObject.FindProperty("volume");
            this.spBlend = this.serializedObject.FindProperty("blend");
            this.spwaitforFirst = this.serializedObject.FindProperty("waitforFirst");

        }

        protected override void OnDisableEditorChild()
        {
            this.sptargetObject = null;
            this.spLoopVideo = null;
            this.spSpeed = null;
            this.spFirstFrame = null;
            this.spVolume = null;
            this.spBlend = null;
            this.spwaitforFirst = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.sptargetObject, new GUIContent("Target Object"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spwaitforFirst, new GUIContent("Wait for first Frame"));
            EditorGUILayout.PropertyField(this.spFirstFrame, new GUIContent("Start at Frame"));

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spLoopVideo, new GUIContent("Loop Video"));
       
            EditorGUILayout.PropertyField(this.spSpeed, new GUIContent("Set Playback Speed"));
            EditorGUILayout.Space();
            EditorGUILayout.Space();
             EditorGUILayout.PropertyField(this.spVolume, new GUIContent("Set Audio Volume"));

            EditorGUILayout.PropertyField(this.spBlend, new GUIContent("Set Spatial Blend"));

            Rect position = EditorGUILayout.GetControlRect(false, 2 * EditorGUIUtility.singleLineHeight); // Get two lines for the control
            position.height *= 0.5f;
           
            position.y += position.height - 15;
            position.x += EditorGUIUtility.labelWidth;
            position.width -= EditorGUIUtility.labelWidth + 54; //54 seems to be the width of the slider's float field
                                                                //sub-labels
            GUIStyle style = GUI.skin.label;
            style.fontSize = 8;
            style.alignment = TextAnchor.UpperLeft; EditorGUI.LabelField(position, "2D", style);
            style.alignment = TextAnchor.UpperRight; EditorGUI.LabelField(position, "3D", style);

            this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
