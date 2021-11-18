namespace GameCreator.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using GameCreator.Core;
    using UnityEngine.EventSystems;
    using GameCreator.Core.Hooks;

    public class ContainerUIItemPlayer : IContainerUIItem
    {
        // CONSTRUCTOR & UPDATER: -----------------------------------------------------------------

        public override void Setup(ContainerUIManager containerUIManager, object item)
        {
            base.Setup(containerUIManager, item);
            this.item = item as Item;

            this.UpdateUI();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        protected override int GetAmount()
        {
            return InventoryManager.Instance.GetInventoryAmountOfItem(this.item.uuid);
        }

        public override void OnClickButton()
        {
            Container container = this.containerUIManager.currentContainer;

            int subtract = InventoryManager.Instance.SubstractItemFromInventory(this.item.uuid, 1);
            if (subtract <= 0) return;

            container.AddItem(this.item.uuid, subtract);

            this.containerUIManager.UpdateItems();
            if (this.containerUIManager.onAdd != null)
            {
                this.containerUIManager.onAdd.Invoke(this.item.uuid);
            }
        }
    }
}