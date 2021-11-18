namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
    using GameCreator.Core;
    using UnityEngine;
	using UnityEngine.Events;

	[AddComponentMenu("")]
	public class ConditionMerchantUI : ICondition
	{
		public enum State
        {
            IsOpen,
            IsClosed
        }

        public State merchant = State.IsOpen;

		public override bool Check(GameObject target)
		{
			switch (this.merchant)
            {
                case State.IsOpen:
                    return MerchantUIManager.IsMerchantOpen();

                case State.IsClosed:
                    return !MerchantUIManager.IsMerchantOpen();
            }

            return false;
		}
        
		#if UNITY_EDITOR

        public static new string NAME = "Inventory/Merchant UI";
        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Conditions/";

        private const string NODE_TITLE = "Merchant UI {0}";

        public override string GetNodeTitle()
        {
            return string.Format(NODE_TITLE, this.merchant);
        }

        #endif
    }
}
