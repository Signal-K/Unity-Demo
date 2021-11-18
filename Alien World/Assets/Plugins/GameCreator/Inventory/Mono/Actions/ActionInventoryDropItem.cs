namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
	using GameCreator.Core.Hooks;

    #if UNITY_EDITOR
	using UnityEditor;
    #endif

    [AddComponentMenu("")]
	public class ActionInventoryDropItem : IAction 
	{
		public ItemHolder itemHolder;
		public float distance = 1.0f;

        // EXECUTABLE: -------------------------------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
			Vector3 position = HookPlayer.Instance.transform.TransformPoint(Vector3.forward * this.distance);

			Instantiate(this.itemHolder.item.prefab, position, Quaternion.identity);
			InventoryManager.Instance.SubstractItemFromInventory(this.itemHolder.item.uuid, 1);

			return true;
        }

		// +-----------------------------------------------------------------------------------------------------------+
		// | EDITOR                                                                                                    |
		// +-----------------------------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Actions/";

		public static new string NAME = "Inventory/Drop Item";
		private const string NODE_TITLE = "Drop {0} before player";

		// INSPECTOR METHODS: ------------------------------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(
				NODE_TITLE, 
				this.itemHolder.item == null ? "nothing" : this.itemHolder.item.itemName.content
			);
		}

		#endif
	}
}