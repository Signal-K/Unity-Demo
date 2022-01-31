namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using GameCreator.Core;

	[AddComponentMenu("")]
	public class IgniterDropToConsume : Igniter
	{
		#if UNITY_EDITOR
		public new static string NAME = "Inventory/On Drop to Consume";
		public new static bool REQUIRES_COLLIDER = true;
        public new static string ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Igniters/";
        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Igniters/";
        #endif

        public virtual void OnDrop(Item item)
        {
			if (item == null) return;
			InventoryManager.Instance.ConsumeItem(item.uuid, gameObject);
			this.ExecuteTrigger();
        }
	}
}
