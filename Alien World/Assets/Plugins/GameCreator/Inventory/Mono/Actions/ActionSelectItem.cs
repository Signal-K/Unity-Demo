namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;

	[AddComponentMenu("")]
	public class ActionSelectItem : IAction
	{
		public ItemHolder item = new ItemHolder();

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            if (this.item.item == null) return true;

            SelectedItemUI.Select(this.item.item.uuid);
            return true;
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

        #if UNITY_EDITOR

        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Actions/";

        public static new string NAME = "Inventory/Select Item";
        private const string NODE_TITLE = "Selects Item {0}";

		public override string GetNodeTitle()
		{
			return string.Format(
                NODE_TITLE, 
                this.item.item == null ? "(none)" : this.item.item.itemName.content
			);
		}

		#endif
	}
}
