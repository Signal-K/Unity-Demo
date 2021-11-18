namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
	using GameCreator.Core;
    using GameCreator.Core.Hooks;

    public class ContainerUIItemBox : IContainerUIItem 
	{
        private Container.ItemData containerData;

        // CONSTRUCTOR & UPDATER: -----------------------------------------------------------------

        public override void Setup(ContainerUIManager containerUIManager, object item)
        {
            base.Setup(containerUIManager, item);
            this.containerData = item as Container.ItemData;
            this.item = InventoryManager.Instance.itemsCatalogue[this.containerData.uuid];

            this.UpdateUI();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        protected override int GetAmount()
        {
            return this.containerData.amount;
        }

        public override void OnClickButton()
		{
            Container container = this.containerUIManager.currentContainer;

            int addAmount = InventoryManager.Instance.AddItemToInventory(this.item.uuid, 1);
            if (addAmount <= 0) return;

            container.RemoveItem(this.item.uuid, addAmount);

            this.containerUIManager.UpdateItems();
            if (this.containerUIManager.onRemove != null)
            {
                this.containerUIManager.onRemove.Invoke(this.item.uuid);
            }
        }
	}
}