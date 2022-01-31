namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using GameCreator.Core;

    [AddComponentMenu("")]
    public class IgniterContainerUI : Igniter 
	{
		#if UNITY_EDITOR
        public new static string NAME = "Inventory/On Container UI";
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
            InventoryManager.Instance.eventContainerUI.AddListener(this.OnChangeState);
        }

        private void OnChangeState(bool state, GameObject container)
        {
            if (state == true  && this.detect == State.OnOpen)  this.ExecuteTrigger(container);
            if (state == false && this.detect == State.OnClose) this.ExecuteTrigger(container);
        }
    }
}