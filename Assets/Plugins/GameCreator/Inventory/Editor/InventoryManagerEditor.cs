namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(InventoryManager))]
	public class InventoryManagerEditor : Editor 
	{
		private const string MSG_NO_ITEM = "Item with uuid {0} could not be found in catalogue";
		private const string PROP_PLAYERINVENTORY = "playerInventory";

		// PROPERTIES: -------------------------------------------------------------------------------------------------

		private InventoryManager inventoryManager;

		private void OnEnable()
		{
			this.inventoryManager = (InventoryManager)target;
		}

		public override bool RequiresConstantRepaint ()
		{
			return true;
		}

		// GUI METHODS: ------------------------------------------------------------------------------------------------

		public override void OnInspectorGUI ()
		{
			serializedObject.Update();
			EditorGUI.BeginChangeCheck();

			InventoryManager.PlayerInventory playerInventory;
			playerInventory = (InventoryManager.PlayerInventory)this.inventoryManager.playerInventory;

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			EditorGUILayout.LabelField(
				"Player Currency", 
				playerInventory.currencyAmount.ToString()
			);
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			EditorGUILayout.LabelField("Player Inventory", EditorStyles.centeredGreyMiniLabel);

			foreach (KeyValuePair<int, int> entry in playerInventory.items)
			{
				if (!this.inventoryManager.itemsCatalogue.ContainsKey(entry.Key))
				{
					EditorGUILayout.HelpBox(string.Format(MSG_NO_ITEM, entry.Key), MessageType.Error);
					continue;
				}

				EditorGUILayout.BeginVertical(EditorStyles.helpBox);

				string uuid = "UUID (" + entry.Key + ")";
				string name = this.inventoryManager.itemsCatalogue[entry.Key].itemName.content;
				string amount = playerInventory.items[entry.Key].ToString();

				EditorGUILayout.LabelField(uuid, name + " (" + amount + ")", EditorStyles.boldLabel);
				EditorGUILayout.EndVertical();
			}

			EditorGUILayout.EndVertical();
			serializedObject.ApplyModifiedPropertiesWithoutUndo();
		}
	}
}