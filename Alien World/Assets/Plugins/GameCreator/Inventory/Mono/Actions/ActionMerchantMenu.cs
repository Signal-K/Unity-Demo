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
	public class ActionMerchantMenu : IAction 
	{
		public enum ACTION_TYPE
		{
			Open,
			Close
		}

		public ACTION_TYPE actionType = ACTION_TYPE.Open;
        public Merchant merchant;

        // EXECUTABLE: -------------------------------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            if (this.actionType == ACTION_TYPE.Open) MerchantUIManager.OpenMerchant(this.merchant);
            if (this.actionType == ACTION_TYPE.Close) MerchantUIManager.CloseMerchant();

            return true;
        }

		// +-----------------------------------------------------------------------------------------------------------+
		// | EDITOR                                                                                                    |
		// +-----------------------------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Actions/";

		public static new string NAME = "Inventory/Merchant UI";
        private const string NODE_TITLE = "{0} merchant {1}";

		// PROPERTIES: -------------------------------------------------------------------------------------------------

		private SerializedProperty spActionType;
        private SerializedProperty spMerchant;

		// INSPECTOR METHODS: ------------------------------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(
				NODE_TITLE, 
				this.actionType.ToString(),
                (this.merchant == null ? "(none)" : this.merchant.name)
			);
		}

		protected override void OnEnableEditorChild ()
		{
            this.spMerchant = this.serializedObject.FindProperty("merchant");
			this.spActionType = this.serializedObject.FindProperty("actionType");
		}

		protected override void OnDisableEditorChild ()
		{
            this.spMerchant = null;
			this.spActionType = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spActionType);
            EditorGUILayout.PropertyField(this.spMerchant);

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}