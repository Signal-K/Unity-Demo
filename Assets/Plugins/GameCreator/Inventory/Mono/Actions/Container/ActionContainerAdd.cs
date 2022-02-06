namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
    using GameCreator.Core;
    using GameCreator.Variables;

    [AddComponentMenu("")]
	public class ActionContainerAdd : IActionContainer
	{
        [Space]
        public ItemHolder item = new ItemHolder();
        public NumberProperty amount = new NumberProperty(1f);

		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            Container cont = this.GetContainer(target);
            if (cont == null) return true;

            if (this.item.item == null) return true;
            cont.AddItem(this.item.item.uuid, this.amount.GetInt(target));

            return true;
        }

        #if UNITY_EDITOR
        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Actions/";
        public static new string NAME = "Inventory/Container/Add Item Container";

        private const string NODE_TITLE = "Add {0} to {1}";

        public override string GetNodeTitle()
        {
            return string.Format(NODE_TITLE, this.item, this.container);
        }

        #endif
    }
}
