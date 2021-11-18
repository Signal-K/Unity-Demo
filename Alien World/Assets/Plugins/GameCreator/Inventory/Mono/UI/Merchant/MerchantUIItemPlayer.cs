namespace GameCreator.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using GameCreator.Core;
    using UnityEngine.EventSystems;
    using GameCreator.Core.Hooks;

    public class MerchantUIItemPlayer : IMerchantUIItem
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        private Item item = null;

        public GameObject wrapAmount;
        public Text textAmount;

        // CONSTRUCTOR & UPDATER: -----------------------------------------------------------------

        public override void Setup(MerchantUIManager merchantUIManager, params object[] parameters)
        {
            base.Setup(merchantUIManager, parameters);
            this.item = parameters[0] as Item;

            this.UpdateUI();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override void UpdateUI()
        {
            if (this.item == null) return;

            if (this.merchantUIManager.currentMerchant == null) return;
            Merchant merchant = this.merchantUIManager.currentMerchant;

            if (this.image != null && this.item.sprite != null) this.image.sprite = this.item.sprite;
            if (this.textName != null) this.textName.text = this.item.itemName.GetText();
            if (this.textDescription != null) this.textDescription.text = this.item.itemDescription.GetText();

            GameObject player = HookPlayer.Instance != null ? HookPlayer.Instance.gameObject : null;
            float percent = merchant.sellPercent.GetValue(player);
            int price = Mathf.FloorToInt(item.price * percent);

            if (this.textPrice != null) this.textPrice.text = price.ToString();

            int curAmount = InventoryManager.Instance.GetInventoryAmountOfItem(this.item.uuid);
            if (this.wrapAmount != null)
            {
                this.wrapAmount.SetActive(curAmount != 1);
                if (this.textAmount != null) this.textAmount.text = curAmount.ToString();
            }
        }

        public override void OnClickButton()
        {
            Merchant merchant = this.merchantUIManager.currentMerchant;
            if (MerchantManager.Instance.SellToMerchant(merchant, this.item, 1))
            {
                this.UpdateUI();
                if (this.merchantUIManager.onSell != null)
                {
                    this.merchantUIManager.onSell.Invoke(this.item.uuid);
                }
            }
            else
            {
                if (this.merchantUIManager.onCantSell != null)
                {
                    this.merchantUIManager.onCantSell.Invoke(this.item.uuid);
                }
            }
        }
    }
}