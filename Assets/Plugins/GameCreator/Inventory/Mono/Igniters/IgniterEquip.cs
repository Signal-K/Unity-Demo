namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using UnityEngine.EventSystems;
    using GameCreator.Core;

	[AddComponentMenu("")]
	public class IgniterEquip : Igniter 
	{
		#if UNITY_EDITOR
		public new static string NAME = "Inventory/On Equip Any";
        public new static string ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Igniters/";
        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Igniters/";
        #endif

        public TargetGameObject character = new TargetGameObject(TargetGameObject.Target.Player);

        private new void OnEnable()
        {
            base.OnEnable();
            InventoryManager.Instance.eventOnEquip.AddListener(this.OnCallback);
        }

        private void OnDisable()
        {
            if (this.isExitingApplication) return;
            InventoryManager.Instance.eventOnEquip.RemoveListener(this.OnCallback);
        }

        private void OnCallback(GameObject target, int item)
        {
            if (target == this.character.GetGameObject(gameObject))
            {
                this.ExecuteTrigger(target);
            }
        }
    }
}