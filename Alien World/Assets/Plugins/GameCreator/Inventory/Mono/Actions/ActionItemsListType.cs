namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
    using GameCreator.Characters;

	#if UNITY_EDITOR
	using UnityEditor;
	#endif

	[AddComponentMenu("")]
    public class ActionItemsListType : IAction
	{
        public ItemsListUI itemsListUI;

        [InventoryMultiItemType]
        public int itemTypes = ~0;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            if (this.itemsListUI != null)
            {
                this.itemsListUI.SetItemTypes(this.itemTypes);
            }

            return true;
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

        #if UNITY_EDITOR

        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Actions/";

        public static new string NAME = "Inventory/Items List UI";
        private const string NODE_TITLE = "Change ItemsListUI types";

		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spItemsListUI;
        private SerializedProperty spItemTypes;

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
            return NODE_TITLE;
		}

		protected override void OnEnableEditorChild ()
		{
            this.spItemsListUI = this.serializedObject.FindProperty("itemsListUI");
            this.spItemTypes = this.serializedObject.FindProperty("itemTypes");
		}

		protected override void OnDisableEditorChild ()
		{
            this.spItemsListUI = null;
            this.spItemTypes = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.spItemsListUI);
            EditorGUILayout.PropertyField(this.spItemTypes);

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
