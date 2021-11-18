namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
    using GameCreator.Core;
    using UnityEngine;
	using UnityEngine.Events;

	[AddComponentMenu("")]
	public class ConditionInventoryUI : ICondition
	{
		public enum State
        {
            IsOpen,
            IsClosed
        }

        public State inventory = State.IsOpen;

		public override bool Check(GameObject target)
		{
			switch (this.inventory)
            {
                case State.IsOpen:
                    return InventoryUIManager.IsInventoryOpen();

                case State.IsClosed:
                    return !InventoryUIManager.IsInventoryOpen();
            }

            return false;
		}
        
		#if UNITY_EDITOR

        public static new string NAME = "Inventory/Inventory UI";
        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Conditions/";

        private const string NODE_TITLE = "Inventory UI {0}";

        public override string GetNodeTitle()
        {
            return string.Format(NODE_TITLE, this.inventory);
        }

        #endif
    }
}
