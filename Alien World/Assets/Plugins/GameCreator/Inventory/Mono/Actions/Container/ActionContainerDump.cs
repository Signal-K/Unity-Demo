namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
    using GameCreator.Core;

	[AddComponentMenu("")]
	public class ActionContainerDump : IAction
	{
        public TargetGameObject container = new TargetGameObject(TargetGameObject.Target.Invoker);

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            Container containerInstance = this.GetContainer(target);
            if (containerInstance == null) return true;

            int[] playerInventory = new int[InventoryManager.Instance.playerInventory.items.Count];
            int playerInventoryIndex = 0;

            foreach (KeyValuePair<int, int> element in InventoryManager.Instance.playerInventory.items)
            {
                playerInventory[playerInventoryIndex] = element.Key;
                playerInventoryIndex += 1;
            }

            for (int i = 0; i < playerInventory.Length; ++i)
            {
                int itemID = playerInventory[i];
                int itemAmount = InventoryManager.Instance.GetInventoryAmountOfItem(itemID);

                itemAmount = InventoryManager.Instance.SubstractItemFromInventory(itemID, itemAmount);
                containerInstance.AddItem(itemID, itemAmount);
            }

            return true;
        }

        private Container GetContainer(GameObject target)
        {
            GameObject containerGo = this.container.GetGameObject(target);
            if (containerGo == null) return null;

            return containerGo.GetComponent<Container>();
        }

        #if UNITY_EDITOR

        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Actions/";
        public static new string NAME = "Inventory/Container/Dump Inventory to Container";

        private const string NODE_TITLE = "Dump inventory to {0}";

        public override string GetNodeTitle()
        {
            return string.Format(NODE_TITLE, this.container);
        }

        #endif
    }
}
