namespace GameCreator.Inventory
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
	public class ActionInventoryMenu : IAction 
	{
		public enum MENU_TYPE
		{
			Inventory
		}

		public enum ACTION_TYPE
		{
			Open,
			Close
		}

		public MENU_TYPE menuType = MENU_TYPE.Inventory;
		public ACTION_TYPE actionType = ACTION_TYPE.Open;

        // EXECUTABLE: -------------------------------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            switch (this.menuType)
            {
                case MENU_TYPE.Inventory:
                    if (this.actionType == ACTION_TYPE.Open) InventoryUIManager.OpenInventory();
                    if (this.actionType == ACTION_TYPE.Close) InventoryUIManager.CloseInventory();
                    break;
            }

            return true;
        }

		// +-----------------------------------------------------------------------------------------------------------+
		// | EDITOR                                                                                                    |
		// +-----------------------------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Actions/";

		public static new string NAME = "Inventory/Inventory UI";
		private const string NODE_TITLE = "{0} {1} menu";

		// PROPERTIES: -------------------------------------------------------------------------------------------------

		private SerializedProperty spMenuType;
		private SerializedProperty spActionType;

		// INSPECTOR METHODS: ------------------------------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(
				NODE_TITLE, 
				this.actionType.ToString(),
				this.menuType.ToString()
			);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spMenuType = this.serializedObject.FindProperty("menuType");
			this.spActionType = this.serializedObject.FindProperty("actionType");
		}

		protected override void OnDisableEditorChild ()
		{
			this.spMenuType = null;
			this.spActionType = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spMenuType);
			EditorGUILayout.PropertyField(this.spActionType);

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}