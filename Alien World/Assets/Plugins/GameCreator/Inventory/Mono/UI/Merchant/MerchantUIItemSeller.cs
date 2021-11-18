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

    public class MerchantUIItemSeller : IMerchantUIItem 
	{
        private Merchant.Ware ware;

        [Space]
        public CanvasGroup canvasGroup;

        [Space]
        public GameObject wrapAmount;
        public Text textCurrentAmount;
        public Text textMaxAmount;

        // CONSTRUCTOR & UPDATER: -----------------------------------------------------------------

        public override void Setup(MerchantUIManager merchantUIManager, params object[] parameters)
        {
            base.Setup(merchantUIManager, parameters);
            this.ware = parameters[0] as Merchant.Ware;

            this.UpdateUI();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override void UpdateUI()
		{
            if (this.ware == null) return;
            Item item = this.ware.item.item;
            Merchant merchant = this.merchantUIManager.currentMerchant;

            int curAmount = MerchantManager.Instance.GetMerchantAmount(
                merchant,
                this.ware.item.item
            );

			if (this.image != null && item.sprite != null) this.image.sprite = item.sprite;
            if (this.textName != null) this.textName.text = item.itemName.GetText();
            if (this.textDescription != null) this.textDescription.text = item.itemDescription.GetText();

            GameObject player = HookPlayer.Instance != null ? HookPlayer.Instance.gameObject : null;
            float percent = merchant.purchasePercent.GetValue(player);
            int price = Mathf.FloorToInt(item.price * percent);

            if (this.textPrice != null) this.textPrice.text = price.ToString();

            this.wrapAmount.SetActive(this.ware.limitAmount);

            if (this.textCurrentAmount != null)
            {
                this.textCurrentAmount.text = curAmount.ToString();
            }

            if (this.textMaxAmount != null)
            {
                this.textMaxAmount.text = this.ware.maxAmount.ToString();
            }

            this.canvasGroup.interactable = (
                InventoryManager.Instance.GetCurrency() >= item.price && 
                (!this.ware.limitAmount || curAmount > 0)
            );
		}

		public override void OnClickButton()
		{
            Merchant merchant = this.merchantUIManager.currentMerchant;
            if (MerchantManager.Instance.BuyFromMerchant(merchant, this.ware.item.item, 1))
            {
                this.UpdateUI();
                if (this.merchantUIManager.onBuy != null)
                {
                    this.merchantUIManager.onBuy.Invoke(this.ware.item.item.uuid);
                }
            }
            else
            {
                if (this.merchantUIManager.onCantBuy != null)
                {
                    this.merchantUIManager.onCantBuy.Invoke(this.ware.item.item.uuid);
                }
            }
		}
	}
}