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
	public class ActionCurrentProfilefromVar : IAction 
	{
		public NumberProperty profile = new NumberProperty(0.0f);


        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{
			int saveslot = (int)profile.GetValue(target);
	        SaveLoadManager.Instance.SetCurrentProfile(saveslot);
            return true;
        }

		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "UI/Misc/Current Profile from Variable";
		private const string NODE_TITLE = "Set Current Profile from Variable";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";


		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spProfile;

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spProfile = this.serializedObject.FindProperty("profile");
		}

		protected override void OnDisableEditorChild ()
		{
			this.spProfile = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spProfile, new GUIContent("Profile from Variable"));

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}