namespace GameCreator.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public abstract class IMerchantUIItem : MonoBehaviour
    {
        protected static DatabaseInventory DATABASE_INVENTORY;

        // PROPERTIES: ----------------------------------------------------------------------------

        protected MerchantUIManager merchantUIManager;

        public Image image;
        public Text textName;
        public Text textDescription;
        public Text textPrice;

        // CONSTRUCTOR & UPDATER: -----------------------------------------------------------------

        public virtual void Setup(MerchantUIManager merchantUIManager, params object[] parameters)
        {
            this.merchantUIManager = merchantUIManager;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public abstract void UpdateUI();
        public abstract void OnClickButton();
    }
}