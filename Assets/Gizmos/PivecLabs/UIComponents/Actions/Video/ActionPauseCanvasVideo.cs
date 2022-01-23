namespace GameCreator.UIComponents
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using GameCreator.Core;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    [AddComponentMenu("")]
	public class ActionPauseCanvasVideo : IAction
    {

        public GameObject targetObject;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {


            var vp = targetObject.GetComponent<UnityEngine.Video.VideoPlayer>();


            vp.Pause();

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

        public static new string NAME = "UI/Video/Pause Video on Canvas";
        private const string NODE_TITLE = "Pause Video on {0}";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptargetObject;

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
        }

        protected override void OnDisableEditorChild()
        {
            this.sptargetObject = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.sptargetObject, new GUIContent("Target Object"));

            this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
