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
	public class ConditionAffordItem : ICondition
	{
		public ItemHolder item;
		public int amount = 1;

		// EXECUTABLE: -------------------------------------------------------------------------------------------------

		public override bool Check()
		{
			if (InventoryManager.Instance.GetCurrency() >= (this.item.item.price * this.amount))
			{
				int currentAmount = InventoryManager.Instance.GetInventoryAmountOfItem(this.item.item.uuid);
				if (currentAmount + this.amount <= this.item.item.maxStack) return true;
			}

			return false;
		}

		// +-----------------------------------------------------------------------------------------------------------+
		// | EDITOR                                                                                                    |
		// +-----------------------------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Conditions/";

		public static new string NAME = "Inventory/Can Buy Item";
		private const string NODE_TITLE = "Can buy {0} {1} item{2}";

		// PROPERTIES: -------------------------------------------------------------------------------------------------

		private SerializedProperty spItem;
		private SerializedProperty spAmount;

		// INSPECTOR METHODS: ------------------------------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(
				NODE_TITLE, 
				this.amount,
				(this.item.item == null ? "(none)" : this.item.item.itemName.content),
				(this.amount == 1 ? "" : "s")
			);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spItem = this.serializedObject.FindProperty("item");
			this.spAmount = this.serializedObject.FindProperty("amount");
		}

		protected override void OnDisableEditorChild ()
		{
			this.spItem = null;
			this.spAmount = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spItem);
			EditorGUILayout.PropertyField(this.spAmount);

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}