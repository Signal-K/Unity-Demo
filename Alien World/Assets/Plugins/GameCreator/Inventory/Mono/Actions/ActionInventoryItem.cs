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
	public class ActionInventoryItem : IAction 
	{
		public enum ITEM_ACTION
		{
			Add,
			Substract,
			Consume
		}

		public ITEM_ACTION operation = ITEM_ACTION.Add;
		public ItemHolder itemHolder;
		public int amount = 1;

        // EXECUTABLE: -------------------------------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            switch (this.operation)
            {
                case ITEM_ACTION.Add:
                    InventoryManager.Instance.AddItemToInventory(this.itemHolder.item.uuid, this.amount);
                    break;

                case ITEM_ACTION.Substract:
                    InventoryManager.Instance.SubstractItemFromInventory(this.itemHolder.item.uuid, this.amount);
                    break;

                case ITEM_ACTION.Consume:
                    InventoryManager.Instance.ConsumeItem(this.itemHolder.item.uuid);
                    break;
            }

            return true;
        }

		// +-----------------------------------------------------------------------------------------------------------+
		// | EDITOR                                                                                                    |
		// +-----------------------------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Actions/";

		public static new string NAME = "Inventory/Item";
		private const string NODE_TITLE = "{0} {1} {2} item{3}";

		// PROPERTIES: -------------------------------------------------------------------------------------------------

		private SerializedProperty spOperation;
		private SerializedProperty spItemHolder;
		private SerializedProperty spAmount;

		// INSPECTOR METHODS: ------------------------------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(
				NODE_TITLE, 
				this.operation.ToString(),
				this.amount.ToString(),
				(this.itemHolder.item == null ? "nothing" : this.itemHolder.item.itemName.content),
				(this.amount != 1 ? "s" : "")
			);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spOperation = this.serializedObject.FindProperty("operation");
			this.spItemHolder = this.serializedObject.FindProperty("itemHolder");
			this.spAmount = this.serializedObject.FindProperty("amount");
		}

		protected override void OnDisableEditorChild ()
		{
			this.spOperation = null;
			this.spItemHolder = null;
			this.spAmount = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.PropertyField(this.spOperation);
			EditorGUILayout.PropertyField(this.spItemHolder);

			EditorGUILayout.PropertyField(this.spAmount);
			this.spAmount.intValue = Mathf.Max(0, this.spAmount.intValue);

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}