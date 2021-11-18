namespace GameCreator.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using GameCreator.Core;

    [AddComponentMenu("Game Creator/UI/Items List")]
    public class ItemsListUI : MonoBehaviour
    {
        public RectTransform container;

        [Space]
        public GameObject prefabItem;

        [InventoryMultiItemType, SerializeField]
        protected int itemTypes = ~0;

        private Dictionary<int, ItemUI> currentItems = new Dictionary<int, ItemUI>();
        private bool isExitingApplication = false;

        // INITIALIZERS: --------------------------------------------------------------------------

        private void OnEnable()
        {
            InventoryManager.Instance.eventChangePlayerInventory.AddListener(this.UpdateItems);
            this.UpdateItems();
        }

        private void OnDisable()
        {
            if (this.isExitingApplication) return;
            InventoryManager.Instance.eventChangePlayerInventory.RemoveListener(this.UpdateItems);
        }

        private void OnApplicationQuit()
        {
            this.isExitingApplication = true;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public virtual void SetItemTypes(int itemTypes)
        {
            this.itemTypes = itemTypes;
            this.UpdateItems();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        protected virtual void UpdateItems()
        {
            Dictionary<int, ItemUI> remainingItems = new Dictionary<int, ItemUI>(this.currentItems);

            foreach (KeyValuePair<int, int> entry in InventoryManager.Instance.playerInventory.items)
            {
                Item currentItem = InventoryManager.Instance.itemsCatalogue[entry.Key];
                int currentItemAmount = InventoryManager.Instance.playerInventory.items[currentItem.uuid];

                if (currentItemAmount <= 0) continue;
                if ((currentItem.itemTypes & this.itemTypes) == 0) continue;

                if (this.currentItems != null && this.currentItems.ContainsKey(currentItem.uuid))
                {
                    this.currentItems[currentItem.uuid].UpdateUI(currentItem, currentItemAmount);
                    remainingItems.Remove(currentItem.uuid);
                }
                else
                {
                    GameObject itemUIPrefab = this.prefabItem;
                    if (itemUIPrefab == null)
                    {
                        string error = "No inventory item UI prefab found. Fill the required field at {0}";
                        string errorPath = "GameCreator/Preferences and head to Inventory -> Settings";
                        Debug.LogErrorFormat(error, errorPath);
                        return;
                    }

                    GameObject itemUIAsset = Instantiate(itemUIPrefab, this.container);
                    ItemUI itemUI = itemUIAsset.GetComponent<ItemUI>();
                    itemUI.Setup(currentItem, currentItemAmount);
                    this.currentItems.Add(currentItem.uuid, itemUI);
                }
            }

            foreach (KeyValuePair<int, ItemUI> entry in remainingItems)
            {
                this.currentItems.Remove(entry.Key);
                Destroy(entry.Value.gameObject);
            }
        }
    }
}