namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using GameCreator.Core;
    using GameCreator.Core.Hooks;

    [AddComponentMenu("")]
    public class IgniterCurrency : Igniter 
	{
		#if UNITY_EDITOR
        public new static string NAME = "Inventory/On Currency Change";
        public new static string ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Igniters/";
        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Igniters/";
        #endif

        private void Start()
        {
            InventoryManager.Instance.eventChangePlayerCurrency.AddListener(this.OnCurrencyChange);
        }

        private void OnCurrencyChange()
        {
            GameObject invoker = HookPlayer.Instance != null
                ? HookPlayer.Instance.gameObject
                : gameObject;

            this.ExecuteTrigger(invoker);
        }
    }
}