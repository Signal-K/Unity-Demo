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
	public class ActionDestroyUIClones : IAction 
	{

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{
			foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
			{
			 if ((gameObj.name == "UICloneObject") || (gameObj.name == "UI3DCamera"))
				 {
					Destroy(gameObj, 0);
				 
			  }
				
			}	
	
            return true;
        }

		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public static new string NAME = "UI/Models/Destroy UI Clones";
		private const string NODE_TITLE = "Destroy UI Clones";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";


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

			EditorGUILayout.LabelField("Destroy UI Clones");
			
			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}