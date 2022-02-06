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
	public class ActionDeleteAllProfiles : IAction 
	{

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{
			var dictionary = SaveLoadManager.Instance.savesData.profiles;
			
			for (int i = -99; i < 99; i++)
			{
				Debug.Log("Deleting Profile " + i);
				SaveLoadManager.Instance.DeleteProfile(i);
				

			}
            return true;
        }

		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public static new string NAME = "UI/Misc/Delete All Profiles";
		private const string NODE_TITLE = "Delete All Profiles";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

		private static readonly GUIContent GUICONTENT_PROFILE = new GUIContent("Profile");

		// PROPERTIES: ----------------------------------------------------------------------------


		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
		}

		protected override void OnDisableEditorChild ()
		{
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.LabelField("Delete All current Saved profiles");
			
			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}