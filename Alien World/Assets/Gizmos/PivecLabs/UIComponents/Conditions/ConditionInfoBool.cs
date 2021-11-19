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
	public class ConditionInfoBool : ICondition
	{
		public SysInfo infoSwitch;
		public GameObject infoPanel;
		public bool satisfied = true;
		// EXECUTABLE: ----------------------------------------------------------------------------
		
		public override bool Check()
		{
		
			infoSwitch = infoPanel.GetComponentInChildren<SysInfo>();
			if (infoSwitch.fpsinfo) satisfied = true;
			else satisfied = false;
			return this.satisfied;
		}

		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public static new string NAME = "UI/Info Bool Condition";
		private const string NODE_TITLE = "Check Info Bool";

		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spSatisfied;
		private SerializedProperty spinfopanel;

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spSatisfied = this.serializedObject.FindProperty("satisfied");
			this.spinfopanel = this.serializedObject.FindProperty("infoPanel");
		}
	

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.PropertyField(this.spinfopanel, new GUIContent("Panel - SysInfo"));
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(this.spSatisfied, new GUIContent("Is Condition Satisfied?"));

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}