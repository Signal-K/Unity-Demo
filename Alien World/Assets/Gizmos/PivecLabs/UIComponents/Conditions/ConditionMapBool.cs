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
	public class ConditionMapBool : ICondition
	{
		public MapManager fullscreen;
		public GameObject mapManager;
		public bool satisfied = true;
		// EXECUTABLE: ----------------------------------------------------------------------------
		
		public override bool Check()
		{
		
			fullscreen = mapManager.GetComponent<MapManager>();
			if (fullscreen.miniMapshowing) satisfied = true;
			else satisfied = false;
			return this.satisfied;
		}

		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public static new string NAME = "UI/Map Bool Condition";
		private const string NODE_TITLE = "Check Map Bool";

		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spSatisfied;
		private SerializedProperty spmapmanager;

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spSatisfied = this.serializedObject.FindProperty("satisfied");
			this.spmapmanager = this.serializedObject.FindProperty("mapManager");
		}
	

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.PropertyField(this.spmapmanager, new GUIContent("Map Manager"));
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(this.spSatisfied, new GUIContent("Is Condition Satisfied?"));

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}