namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using UnityEngine.EventSystems;
    using GameCreator.Core;

	[AddComponentMenu("")]
    public class IgniterUnequipType : Igniter 
	{
		#if UNITY_EDITOR
		public new static string NAME = "Inventory/On Unequip Type";
        public new static string ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Igniters/";
        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Igniters/";
        #endif

        public TargetGameObject character = new TargetGameObject(TargetGameObject.Target.Player);

        [InventoryMultiItemType]
        public int itemTypes = -1;

        private new void OnEnable()
        {
            base.OnEnable();
            InventoryManager.Instance.eventOnUnequip.AddListener(this.OnCallback);
        }

        private void OnDisable()
        {
            if (this.isExitingApplication) return;
            InventoryManager.Instance.eventOnUnequip.RemoveListener(this.OnCallback);
        }

        private void OnCallback(GameObject target, int item)
        {
            if (target == this.character.GetGameObject(gameObject) &&
                (InventoryManager.Instance.itemsCatalogue[item].itemTypes & this.itemTypes) > 0)
            {
                this.ExecuteTrigger(target);
            }
        }
    }
}