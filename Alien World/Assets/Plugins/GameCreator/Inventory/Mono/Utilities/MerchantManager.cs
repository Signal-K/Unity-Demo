namespace GameCreator.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using GameCreator.Core;
    using System;

    [AddComponentMenu("Game Creator/Managers/Merchant Manager", 100)]
    public class MerchantManager : Singleton<MerchantManager>, IGameSave
    {
        [Serializable]
        public class MerchantData
        {
            public string merchantID = "";
            public string itemID = "";
            public int remainingAmount = 0;

            public MerchantData(string merchantID, string itemID, int remainingAmount)
            {
                this.merchantID = merchantID;
                this.itemID = itemID;
                this.remainingAmount = remainingAmount;
            }

            public static string GetKey(string merchantID, string itemID)
            {
                return string.Format("{0}:{1}", merchantID, itemID);
            }
        }

        [Serializable]
        public class MerchantSaveData
        {
            public List<MerchantData> warehouses = new List<MerchantData>();
        }

        // PROPERTIES: ----------------------------------------------------------------------------

        private Dictionary<string, MerchantData> wares = new Dictionary<string, MerchantData>();

        // INITIALIZERS: --------------------------------------------------------------------------

        protected override void OnCreate()
        {
            base.OnCreate();
            SaveLoadManager.Instance.Initialize(this);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public int GetMerchantAmount(Merchant merchant, Item item)
        {
            string key = MerchantData.GetKey(merchant.uuid, item.uuid.ToString());
            if (!this.wares.ContainsKey(key))
            {
                int maxAmount = -1;
                for (int i = 0; maxAmount == -1 && i < merchant.warehouse.wares.Length; ++i)
                {
                    if (merchant.warehouse.wares[i].item.item != null &&
                        merchant.warehouse.wares[i].item.item.uuid == item.uuid)
                    {
                        if (merchant.warehouse.wares[i].limitAmount)
                        {
                            maxAmount = merchant.warehouse.wares[i].maxAmount;
                        }
                        else
                        {
                            maxAmount = int.MaxValue;
                        }
                    }
                }

                this.wares.Add(key, new MerchantData(merchant.uuid, item.uuid.ToString(), maxAmount));
            }

            return this.wares[key].remainingAmount;
        }

        public bool BuyFromMerchant(Merchant merchant, Item item, int buyAmount)
        {
            int curAmount = this.GetMerchantAmount(merchant, item);

            int remainingAmount = curAmount - buyAmount;
            if (remainingAmount < 0) return false;

            if (InventoryManager.Instance.BuyItem(item.uuid, buyAmount, merchant))
            {
                string key = MerchantData.GetKey(merchant.uuid, item.uuid.ToString());
                if (!this.wares.ContainsKey(key))
                {
                    this.wares.Add(
                        key,
                        new MerchantData(merchant.uuid, item.uuid.ToString(), remainingAmount)
                    );
                }
                else
                {
                    this.wares[key].remainingAmount = remainingAmount;
                }

                return true;
            }

            return false;
        }

        public bool SellToMerchant(Merchant merchant, Item item, int sellAmount)
        {
            return InventoryManager.Instance.SellItem(item.uuid, sellAmount, merchant);
        }

        // IGAMESAVE INTERFACE: -------------------------------------------------------------------

        public object GetSaveData()
        {
            MerchantSaveData data = new MerchantSaveData();
            foreach (KeyValuePair<string, MerchantData> item in this.wares)
            {
                data.warehouses.Add(item.Value);
            }

            return data;
        }

        public Type GetSaveDataType()
        {
            return typeof(MerchantSaveData);
        }

        public string GetUniqueName()
        {
            return "merchant";
        }

        public void OnLoad(object generic)
        {
            MerchantSaveData saveData = generic as MerchantSaveData;
            if (saveData == null) saveData = new MerchantSaveData();

            this.wares = new Dictionary<string, MerchantData>();

            for (int i = 0; i < saveData.warehouses.Count; ++i)
            {
                MerchantData saveDataItem = saveData.warehouses[i];
                string key = MerchantData.GetKey(saveDataItem.merchantID, saveDataItem.itemID);

                if (this.wares.ContainsKey(key)) continue;
                this.wares.Add(key, saveDataItem);
            }
        }

        public void ResetData()
        {
            this.wares = new Dictionary<string, MerchantData>();
        }
    }
}