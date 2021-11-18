namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using UnityEngine.Events;
	using UnityEngine.UI;
    using UnityEngine.Serialization;
    using GameCreator.Core;

	public class MerchantUIManager : MonoBehaviour
	{
        private const int TIME_LAYER = 201;

        public static MerchantUIManager Instance { get; private set; }
        private static DatabaseInventory DATABASE_INVENTORY;

        private const string DEFAULT_UI_PATH = "GameCreator/Inventory/MerchantUI";

        [System.Serializable]
        public class MerchantEvent : UnityEvent<int> { }

		// PROPERTIES: ----------------------------------------------------------------------------

        public ScrollRect scrollMerchant;
        public ScrollRect scrollPlayer;

        [Space]
        public Text textTitle;
        public Text textDescription;

        [InventoryMultiItemType, SerializeField]
        private int playerTypes = ~0;

        [Space]
        public GameObject itemUIPrefabSeller;
        public GameObject itemUIPrefabPlayer;

        [HideInInspector]
        public Merchant currentMerchant;

		private Animator merchantAnimator;
		private GameObject merchantRoot;
		private bool isOpen = false;

        [Space]
        public MerchantEvent onBuy = new MerchantEvent();
        public MerchantEvent onCantBuy = new MerchantEvent();

        [Space]
        public MerchantEvent onSell = new MerchantEvent();
        public MerchantEvent onCantSell = new MerchantEvent();

        private Dictionary<int, MerchantUIItemSeller> merchantItems;
        private Dictionary<int, MerchantUIItemPlayer> playerItems;

        // INITIALIZERS: --------------------------------------------------------------------------

        private void Awake()
		{
            MerchantUIManager.Instance = this;

            this.merchantItems = new Dictionary<int, MerchantUIItemSeller>();
            this.playerItems = new Dictionary<int, MerchantUIItemPlayer>();

			if (transform.childCount >= 1) 
			{
				this.merchantRoot = transform.GetChild(0).gameObject;
                this.merchantAnimator = this.merchantRoot.GetComponent<Animator>();
			}
		}

		// PUBLIC METHODS: ------------------------------------------------------------------------

        public void Open(Merchant merchant)
		{
            this.currentMerchant = merchant;
            this.ChangeState(true);

			if (DATABASE_INVENTORY.inventorySettings.pauseTimeOnUI)
			{
                TimeManager.Instance.SetTimeScale(0f, TIME_LAYER);
            }

            this.BuildSellerItemsUI(merchant);
            this.UpdatePlayerItems();

            if (this.textTitle != null) this.textTitle.text = merchant.title;
            if (this.textDescription != null) this.textDescription.text = merchant.description;

            InventoryManager.Instance.eventChangePlayerCurrency.AddListener(this.UpdateItems);
            InventoryManager.Instance.eventChangePlayerInventory.AddListener(this.UpdateItems);
        }

		public void Close()
		{
			if (!this.isOpen) return;

			if (DATABASE_INVENTORY.inventorySettings.pauseTimeOnUI)
			{
                TimeManager.Instance.SetTimeScale(1f, TIME_LAYER);
            }

			this.ChangeState(false);

            InventoryManager.Instance.eventChangePlayerCurrency.RemoveListener(this.UpdateItems);
            InventoryManager.Instance.eventChangePlayerInventory.RemoveListener(this.UpdateItems);
        }

        public void ChangePlayerTypes(int itemTypes)
        {
            this.playerTypes = itemTypes;
            this.UpdateItems();
        }

        // STATIC METHODS: ------------------------------------------------------------------------

        public static void OpenMerchant(Merchant merchant)
		{
            MerchantUIManager.RequireInstance(merchant);
            MerchantUIManager.Instance.Open(merchant);
		}

		public static void CloseMerchant()
		{
            if (!IsMerchantOpen()) return;
			MerchantUIManager.Instance.Close();
		}

        public static bool IsMerchantOpen()
        {
            if (MerchantUIManager.Instance == null) return false;
            return MerchantUIManager.Instance.isOpen;
        }

        private static void RequireInstance(Merchant merchant)
		{
            if (DATABASE_INVENTORY == null) DATABASE_INVENTORY = DatabaseInventory.Load();

            if (MerchantUIManager.Instance != null)
            {
	            Destroy(MerchantUIManager.Instance.gameObject);
            }
            
            EventSystemManager.Instance.Wakeup();
            if (DATABASE_INVENTORY.inventorySettings == null)
            {
	            Debug.LogError("No inventory database found");
	            return;
            }

            GameObject prefab = merchant.merchantUI;
            if (prefab == null) prefab = DATABASE_INVENTORY.inventorySettings.merchantUIPrefab;
            if (prefab == null) prefab = Resources.Load<GameObject>(DEFAULT_UI_PATH);

            Instantiate(prefab, Vector3.zero, Quaternion.identity);
		}

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void ChangeState(bool toOpen)
        {
            if (this.merchantRoot == null)
            {
                Debug.LogError("Unable to find merchantRoot");
                return;
            }

            this.isOpen = toOpen;

            if (this.merchantAnimator == null)
            {
                this.merchantRoot.SetActive(toOpen);
                return;
            }

            this.merchantAnimator.SetBool("State", toOpen);
            InventoryManager.Instance.eventMerchantUI.Invoke(toOpen);
        }

        private void UpdateItems()
        {
            foreach (KeyValuePair<int, MerchantUIItemSeller> item in this.merchantItems)
            {
                if (item.Value != null) item.Value.UpdateUI();
            }

            this.UpdatePlayerItems();
        }

        // PRIVATE SELLER METHODS: ----------------------------------------------------------------

        private void BuildSellerItemsUI(Merchant merchant)
		{
            for (int i = this.scrollMerchant.content.childCount - 1; i >= 0; --i)
            {
                Destroy(this.scrollMerchant.content.GetChild(i).gameObject);
            }

            this.merchantItems = new Dictionary<int, MerchantUIItemSeller>();

            for (int i = 0; i < merchant.warehouse.wares.Length; ++i)
            {
                if (this.merchantItems.ContainsKey(merchant.warehouse.wares[i].item.item.uuid)) continue;

                GameObject instance = Instantiate(this.itemUIPrefabSeller, this.scrollMerchant.content);
                MerchantUIItemSeller item = instance.GetComponent<MerchantUIItemSeller>();
                this.merchantItems.Add(merchant.warehouse.wares[i].item.item.uuid, item);
                item.Setup(this, merchant.warehouse.wares[i]);
            }
		}

        // PRIVATE PLAYER METHODS: ----------------------------------------------------------------

        private void UpdatePlayerItems()
        {
            Dictionary<int, MerchantUIItemPlayer> remainingItems = null;
            remainingItems = new Dictionary<int, MerchantUIItemPlayer>(this.playerItems);

            foreach (KeyValuePair<int, int> entry in InventoryManager.Instance.playerInventory.items)
            {
                Item currentItem = InventoryManager.Instance.itemsCatalogue[entry.Key];
                int currentItemAmount = InventoryManager.Instance.playerInventory.items[currentItem.uuid];

                if (currentItemAmount <= 0) continue;
                if ((currentItem.itemTypes & this.playerTypes) == 0) continue;
                if (!currentItem.canBeSold) continue;

                if (this.playerItems != null && this.playerItems.ContainsKey(currentItem.uuid))
                {
                    this.playerItems[currentItem.uuid].UpdateUI();
                    remainingItems.Remove(currentItem.uuid);
                }
                else
                {
                    GameObject itemUIPrefab = this.itemUIPrefabPlayer;
                    GameObject itemUIAsset = Instantiate(itemUIPrefab, this.scrollPlayer.content);

                    MerchantUIItemPlayer itemUI = itemUIAsset.GetComponent<MerchantUIItemPlayer>();

                    itemUI.Setup(this, currentItem);
                    this.playerItems.Add(currentItem.uuid, itemUI);
                }
            }

            foreach (KeyValuePair<int, MerchantUIItemPlayer> entry in remainingItems)
            {
                this.playerItems.Remove(entry.Key);
                Destroy(entry.Value.gameObject);
            }
        }

    }
}