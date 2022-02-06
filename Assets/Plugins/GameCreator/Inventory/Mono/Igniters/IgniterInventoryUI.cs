namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using GameCreator.Core;
    using GameCreator.Core.Hooks;

    [AddComponentMenu("")]
    public class IgniterInventoryUI : Igniter 
	{
		#if UNITY_EDITOR
        public new static string NAME = "Inventory/On Inventory UI";
        public new static string ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Igniters/";
        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Igniters/";
        #endif

        public enum State
        {
            OnOpen,
            OnClose
        }

        public State detect = State.OnOpen;

        private void Start()
        {
            InventoryManager.Instance.eventInventoryUI.AddListener(this.OnChangeState);
        }

        private void OnChangeState(bool state)
        {
            GameObject target = HookPlayer.Instance != null
                ? HookPlayer.Instance.gameObject
                : gameObject;

            if (state == true  && this.detect == State.OnOpen)  this.ExecuteTrigger(target);
            if (state == false && this.detect == State.OnClose) this.ExecuteTrigger(target);
        }
    }
}