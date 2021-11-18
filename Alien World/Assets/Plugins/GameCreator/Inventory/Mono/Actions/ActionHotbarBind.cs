namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;

	[AddComponentMenu("")]
	public class ActionHotbarBind : IAction
	{
        public TargetGameObject hotbar = new TargetGameObject(TargetGameObject.Target.GameObject);
        public ItemHolder item = new ItemHolder();

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            if (this.item.item == null) return true;

            GameObject targetHotbar = this.hotbar.GetGameObject(target);
            if (target == null) return true;

            HotbarUI hotbarUI = targetHotbar.GetComponent<HotbarUI>();
            if (hotbarUI == null) return true;

            hotbarUI.BindItem(this.item.item);
            return true;
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

        #if UNITY_EDITOR

        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Actions/";

        public static new string NAME = "Inventory/Bind Item to Hotbar";
        private const string NODE_TITLE = "Bind {0} on {1}";

		public override string GetNodeTitle()
		{
			return string.Format(
                NODE_TITLE, 
                this.item.item == null ? "(none)" : this.item.item.itemName.content,
                this.hotbar
            );
		}

		#endif
	}
}
