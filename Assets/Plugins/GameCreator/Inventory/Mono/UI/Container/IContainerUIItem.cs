namespace GameCreator.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public abstract class IContainerUIItem : MonoBehaviour
    {
        protected static DatabaseInventory DATABASE_INVENTORY;

        // PROPERTIES: ----------------------------------------------------------------------------

        protected ContainerUIManager containerUIManager;
        protected Item item = null;

        public Image image;
        public Text textName;
        public Text textDescription;

        [Space]
        public GameObject wrapAmount;
        public Text textAmount;

        // CONSTRUCTOR & UPDATER: -----------------------------------------------------------------

        public virtual void Setup(ContainerUIManager containerUIManager, object item)
        {
            this.containerUIManager = containerUIManager;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void UpdateUI()
        {
            if (this.item == null) return;

            if (this.image != null && this.item.sprite != null) this.image.sprite = this.item.sprite;
            if (this.textName != null) this.textName.text = this.item.itemName.GetText();
            if (this.textDescription != null) this.textDescription.text = this.item.itemDescription.GetText();

            int amount = this.GetAmount();
            if (this.wrapAmount != null)
            {
                this.wrapAmount.SetActive(amount != 1);
                if (this.textAmount != null) this.textAmount.text = amount.ToString();
            }
        }

        public abstract void OnClickButton();
        protected abstract int GetAmount();
    }
}