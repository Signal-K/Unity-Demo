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
	public class ConditionItem : ICondition
	{
		public ItemHolder item;
		public int minAmount;

		// EXECUTABLE: -------------------------------------------------------------------------------------------------

		public override bool Check()
		{
			int currentAmount = InventoryManager.Instance.GetInventoryAmountOfItem(this.item.item.uuid);
			return this.minAmount <= currentAmount;
		}

		// +-----------------------------------------------------------------------------------------------------------+
		// | EDITOR                                                                                                    |
		// +-----------------------------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Conditions/";

		public static new string NAME = "Inventory/Item in Inventory";
		private const string NODE_TITLE = "Player has {0} {1} item{2} or more";

		// PROPERTIES: -------------------------------------------------------------------------------------------------

		private SerializedProperty spItem;
		private SerializedProperty spMinAmount;

		// INSPECTOR METHODS: ------------------------------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(
				NODE_TITLE, 
				this.minAmount,
				(this.item.item == null ? "(none)" : this.item.item.itemName.content),
				(this.minAmount == 1 ? "" : "s")
			);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spItem = this.serializedObject.FindProperty("item");
			this.spMinAmount = this.serializedObject.FindProperty("minAmount");
		}

		protected override void OnDisableEditorChild ()
		{
			this.spItem = null;
			this.spMinAmount = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spItem);
			EditorGUILayout.PropertyField(this.spMinAmount);

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}