namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using UnityEngine.Events;
	using UnityEngine.UI;
    using UnityEngine.Serialization;
    using GameCreator.Core;
    using GameCreator.Variables;

    public class ContainerUIManager : MonoBehaviour
	{
        private const int TIME_LAYER = 202;

        public static ContainerUIManager Instance { get; private set; }
        private static DatabaseInventory DATABASE_INVENTORY;

        private const string DEFAULT_UI_PATH = "GameCreator/Inventory/ContainerUI";

        [System.Serializable]
        public class ContainerEvent : UnityEvent<int> { }

        [System.Serializable]
        public class ContainerTakeAllEvent : UnityEvent { }

        // PROPERTIES: ----------------------------------------------------------------------------

        public ScrollRect scrollContainer;
        public ScrollRect scrollPlayer;

        [Space, InventoryMultiItemType, SerializeField]
        private int playerTypes = ~0;

        [Space]
        public GameObject itemUIPrefabContainer;
        public GameObject itemUIPrefabPlayer;

        [HideInInspector]
        public Container currentContainer;

		private Animator containerAnimator;
		private GameObject containerRoot;
		private bool isOpen = false;

        [Space]
        public ContainerEvent onAdd = new ContainerEvent();
        public ContainerEvent onRemove = new ContainerEvent();
        public ContainerTakeAllEvent onTakeAll = new ContainerTakeAllEvent();

        private Dictionary<int, ContainerUIItemBox> containerItems;
        private Dictionary<int, ContainerUIItemPlayer> playerItems;

        [Space]
        public Button buttonTakeAll;

        // INITIALIZERS: --------------------------------------------------------------------------

        private void Awake()
		{
            ContainerUIManager.Instance = this;

            this.containerItems = new Dictionary<int, ContainerUIItemBox>();
            this.playerItems = new Dictionary<int, ContainerUIItemPlayer>();

			if (transform.childCount >= 1) 
			{
				this.containerRoot = transform.GetChild(0).gameObject;
                this.containerAnimator = this.containerRoot.GetComponent<Animator>();
			}

            if (this.buttonTakeAll != null)
            {
                this.buttonTakeAll.onClick.AddListener(this.GetAllItemsFromContainer);
            }
		}

		// PUBLIC METHODS: ------------------------------------------------------------------------

        public void Open(Container container)
		{
            this.currentContainer = container;
            this.ChangeState(true);

			if (DATABASE_INVENTORY.inventorySettings.pauseTimeOnUI)
			{
                TimeManager.Instance.SetTimeScale(0f, TIME_LAYER);
            }

            this.UpdateItems();

            InventoryManager.Instance.eventChangePlayerCurrency.AddListener(this.UpdatePlayerItems);
            InventoryManager.Instance.eventChangePlayerInventory.AddListener(this.UpdatePlayerItems);

            this.currentContainer.AddOnAddListener(this.UpdateContainerItems);
            this.currentContainer.AddOnRemoveListener(this.UpdateContainerItems);
        }

		public void Close()
		{
			if (!this.isOpen) return;

			if (DATABASE_INVENTORY.inventorySettings.pauseTimeOnUI)
			{
                TimeManager.Instance.SetTimeScale(1f, TIME_LAYER);
            }

			this.ChangeState(false);

            InventoryManager.Instance.eventChangePlayerCurrency.RemoveListener(this.UpdatePlayerItems);
            InventoryManager.Instance.eventChangePlayerInventory.RemoveListener(this.UpdatePlayerItems);

            this.currentContainer.RemoveOnAddListener(this.UpdateContainerItems);
            this.currentContainer.RemoveOnRemoveListener(this.UpdateContainerItems);
        }

        public void ChangePlayerTypes(int itemTypes)
        {
            this.playerTypes = itemTypes;
            this.UpdateItems();
        }

        public void UpdateItems()
        {
            this.UpdateContainerItems();
            this.UpdatePlayerItems();
        }

        // STATIC METHODS: ------------------------------------------------------------------------

        public static void OpenContainer(Container container)
		{
            ContainerUIManager.RequireInstance(container);
            ContainerUIManager.Instance.Open(container);
		}

		public static void CloseContainer()
		{
            if (!IsContainerOpen()) return;
            ContainerUIManager.Instance.Close();
		}

        public static bool IsContainerOpen()
        {
            if (ContainerUIManager.Instance == null) return false;
            return ContainerUIManager.Instance.isOpen;
        }

        private static void RequireInstance(Container container)
		{
            if (DATABASE_INVENTORY == null) DATABASE_INVENTORY = DatabaseInventory.Load();

            if (ContainerUIManager.Instance != null)
            {
                Destroy(ContainerUIManager.Instance.gameObject);
            }
            
            EventSystemManager.Instance.Wakeup();
            if (DATABASE_INVENTORY.inventorySettings == null)
            {
                Debug.LogError("No inventory database found");
                return;
            }

            GameObject prefab = container.containerUI;
            if (prefab == null) prefab = DATABASE_INVENTORY.inventorySettings.containerUIPrefab;
            if (prefab == null) prefab = Resources.Load<GameObject>(DEFAULT_UI_PATH);

            Instantiate(prefab, Vector3.zero, Quaternion.identity);
		}

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void ChangeState(bool toOpen)
        {
            if (this.containerRoot == null)
            {
                Debug.LogError("Unable to find containerRoot");
                return;
            }

            this.isOpen = toOpen;

            if (this.containerAnimator == null)
            {
                this.containerRoot.SetActive(toOpen);
                return;
            }

            this.containerAnimator.SetBool("State", toOpen);
            InventoryManager.Instance.eventContainerUI.Invoke(
                toOpen,
                this.currentContainer.gameObject
            );
        }

        // PRIVATE CONTAINER METHODS: -------------------------------------------------------------

        private void UpdateContainerItems(int itemID = 0, int amount = 0)
        {
            Dictionary<int, ContainerUIItemBox> remainingItems = null;
            remainingItems = new Dictionary<int, ContainerUIItemBox>(this.containerItems);

            List<Container.ItemData> items = this.currentContainer.GetItems();
            for (int i = 0; i < items.Count; ++i)
            {
                Container.ItemData currentItem = items[i];
                if (currentItem.amount <= 0) continue;

                if (this.containerItems.ContainsKey(currentItem.uuid))
                {
                    this.containerItems[currentItem.uuid].UpdateUI();
                    remainingItems.Remove(currentItem.uuid);
                }
                else
                {
                    GameObject itemUIPrefab = this.itemUIPrefabContainer;
                    GameObject itemUIAsset = Instantiate(itemUIPrefab, this.scrollContainer.content);

                    ContainerUIItemBox itemUI = itemUIAsset.GetComponent<ContainerUIItemBox>();

                    itemUI.Setup(this, currentItem);
                    this.containerItems.Add(currentItem.uuid, itemUI);
                }
            }

            foreach (KeyValuePair<int, ContainerUIItemBox> entry in remainingItems)
            {
                this.containerItems.Remove(entry.Key);
                Destroy(entry.Value.gameObject);
            }
        }

        public void GetAllItemsFromContainer()
        {
            List<Container.ItemData> items = this.currentContainer.GetItems();
            for (int i = 0; i < items.Count; ++i)
            {
                Container.ItemData item = items[i];

                InventoryManager.Instance.AddItemToInventory(item.uuid, item.amount);
                this.currentContainer.RemoveItem(item.uuid, item.amount);
            }

            this.onTakeAll.Invoke();
        }

        // PRIVATE PLAYER METHODS: ----------------------------------------------------------------

        private void UpdatePlayerItems()
        {
            Dictionary<int, ContainerUIItemPlayer> remainingItems = null;
            remainingItems = new Dictionary<int, ContainerUIItemPlayer>(this.playerItems);

            foreach (KeyValuePair<int, int> entry in InventoryManager.Instance.playerInventory.items)
            {
                Item currentItem = InventoryManager.Instance.itemsCatalogue[entry.Key];
                int currentItemAmount = InventoryManager.Instance.playerInventory.items[currentItem.uuid];

                if (currentItemAmount <= 0) continue;
                if ((currentItem.itemTypes & this.playerTypes) == 0) continue;

                if (this.playerItems != null && this.playerItems.ContainsKey(currentItem.uuid))
                {
                    this.playerItems[currentItem.uuid].UpdateUI();
                    remainingItems.Remove(currentItem.uuid);
                }
                else
                {
                    GameObject itemUIPrefab = this.itemUIPrefabPlayer;
                    GameObject itemUIAsset = Instantiate(itemUIPrefab, this.scrollPlayer.content);

                    ContainerUIItemPlayer itemUI = itemUIAsset.GetComponent<ContainerUIItemPlayer>();

                    itemUI.Setup(this, currentItem);
                    this.playerItems.Add(currentItem.uuid, itemUI);
                }
            }

            foreach (KeyValuePair<int, ContainerUIItemPlayer> entry in remainingItems)
            {
                this.playerItems.Remove(entry.Key);
                Destroy(entry.Value.gameObject);
            }
        }

    }
}