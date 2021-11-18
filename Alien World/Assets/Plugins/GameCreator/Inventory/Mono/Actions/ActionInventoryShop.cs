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
	public class ActionInventoryShop : IAction 
	{
		public enum ACTION
		{
			Buy,
			Sell
		}

		public ACTION action = ACTION.Buy;
		public ItemHolder item;
		public int amount = 1;

        // EXECUTABLE: -------------------------------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            switch (this.action)
            {
                case ACTION.Buy: InventoryManager.Instance.BuyItem(this.item.item.uuid, this.amount); break;
                case ACTION.Sell: InventoryManager.Instance.SellItem(this.item.item.uuid, this.amount); break;
            }

            return true;
        }

		// +-----------------------------------------------------------------------------------------------------------+
		// | EDITOR                                                                                                    |
		// +-----------------------------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Actions/";

		public static new string NAME = "Inventory/Shop";
		private const string NODE_TITLE = "{0} {1} {2} item{3}";

		// PROPERTIES: -----------------------------------------------------------------------------------------------------

		private SerializedProperty spAction;
		private SerializedProperty spItem;
		private SerializedProperty spAmount;

		// INSPECTOR METHODS: ----------------------------------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(
				NODE_TITLE, 
				this.action.ToString(),
				this.amount,
				(this.item.item == null ? "(none)" : this.item.item.itemName.content),
				(this.amount == 1 ? "" : "s")
			);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spAction = this.serializedObject.FindProperty("action");
			this.spItem = this.serializedObject.FindProperty("item");
			this.spAmount = this.serializedObject.FindProperty("amount");
		}

		protected override void OnDisableEditorChild ()
		{
			this.spAction = null;
			this.spItem = null;
			this.spAmount = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spAction);
			EditorGUILayout.PropertyField(this.spItem);
			EditorGUILayout.PropertyField(this.spAmount);

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}