namespace GameCreator.UIComponents
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
	using UnityEngine.UI;

	#if UNITY_EDITOR
	using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionUIDigitalClock : IAction
	{
    
        public GameObject textObject;
        public bool seconds;
		private Text textdata;
  

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

            CancelInvoke("systemTime");
            if (seconds == true)
			{
                InvokeRepeating("systemTime", 0, 1.0f);
            }
            else
			{
                InvokeRepeating("systemTime", 0, 60.0f);
            }
           

           

            return true;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            return base.Execute(target, actions, index);
        }

        private void systemTime()
        {

            System.DateTime time = System.DateTime.Now;
	        textdata = textObject.GetComponent<Text>();
            if (seconds == true)
	            textdata.text = time.ToString("HH:mm:ss");
	    
            else
	            textdata.text = time.ToString("HH:mm");

        }

        public void StopRepeating()
        {
            CancelInvoke("systemTime");
        }
            // +--------------------------------------------------------------------------------------+
            // | EDITOR                                                                               |
            // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "UI/Time/UI Digital Clock";
		private const string NODE_TITLE = "Display System Time - Digital";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptextmesh;
        private SerializedProperty spseconds;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.sptextmesh = this.serializedObject.FindProperty("textObject");
            this.spseconds = this.serializedObject.FindProperty("seconds");
        }

        protected override void OnDisableEditorChild ()
		{
			this.sptextmesh = null;
            this.spseconds = null;
        }

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

	        EditorGUILayout.PropertyField(this.sptextmesh, new GUIContent("Text Object"));
            EditorGUI.indentLevel++;
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.spseconds, new GUIContent("show seconds"));
            EditorGUI.indentLevel--;

            this.serializedObject.ApplyModifiedProperties();
		}

#endif
        }
}
