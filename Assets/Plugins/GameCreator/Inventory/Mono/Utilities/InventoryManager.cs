namespace GameCreator.Inventory
{
    using System;
    using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
	using GameCreator.Core.Hooks;

	[AddComponentMenu("Game Creator/Managers/Inventory Manager", 100)]
	public class InventoryManager : Singleton<InventoryManager>, IGameSave
	{
		[Serializable]
		public class PlayerInventory
		{
			public Dictionary<int, int> items;
			public int currencyAmount;

            public bool weightDirty;
            public float weightCached;

			public PlayerInventory()
			{
				this.items = new Dictionary<int, int>();
				this.currencyAmount = 0;

                this.weightDirty = true;
                this.weightCached = 0f;
			}
		}

		[Serializable]
		protected class InventorySaveData
		{
			public int[] playerItemsUUIDS;
			public int[] playerItemsStack;
			public int playerCurrencyAmount;

			public InventorySaveData()
			{
				this.playerItemsUUIDS = new int[0];
				this.playerItemsStack = new int[0];
				this.playerCurrencyAmount = 0;
			}
		}

        public class InventoryEvent : UnityEvent { }
        public class EquipmentEvent : UnityEvent<GameObject, int> { }

        public class ContainerUIEvent : UnityEvent<bool, GameObject> { }
        public class InventoryUIEvent : UnityEvent<bool> { }
        public class MerchantUIEvent : UnityEvent<bool> { }

        private const string WARN_PLYR = "Adding <b>PlayerEquipment</b> component because none was found.";
        private const string WARN_CHAR = "Adding <b>CharacterEquipment</b> component because none was found.";
        private const string WARN_SOLT = "Consider adding one in the Editor";

        // PROPERTIES: ----------------------------------------------------------------------------

        protected static DatabaseInventory INVENTORY;

		public PlayerInventory playerInventory {private set; get;}
		public Dictionary<int, Item> itemsCatalogue {private set; get;}
		public Dictionary<Recipe.Key, Recipe> recipes {private set; get;}

        [HideInInspector] public InventoryEvent eventChangePlayerInventory = new InventoryEvent();
        [HideInInspector] public InventoryEvent eventChangePlayerCurrency = new InventoryEvent();

        [HideInInspector] public EquipmentEvent eventOnEquip = new EquipmentEvent();
        [HideInInspector] public EquipmentEvent eventOnUnequip = new EquipmentEvent();

        [HideInInspector] public InventoryUIEvent eventInventoryUI = new InventoryUIEvent();
        [HideInInspector] public ContainerUIEvent eventContainerUI = new ContainerUIEvent();
        [HideInInspector] public MerchantUIEvent eventMerchantUI = new MerchantUIEvent();

        // INITIALIZE: ----------------------------------------------------------------------------

        protected override void OnCreate ()
		{
            DatabaseInventory dbInventory = DatabaseInventory.Load();
            if (INVENTORY == null) INVENTORY = dbInventory;

            this.eventChangePlayerInventory = new InventoryEvent();
            this.eventChangePlayerCurrency = new InventoryEvent();

			this.itemsCatalogue = new Dictionary<int, Item>();
			for (int i = 0; i < dbInventory.inventoryCatalogue.items.Length; ++i)
			{
				this.itemsCatalogue.Add(
					dbInventory.inventoryCatalogue.items[i].uuid,
					dbInventory.inventoryCatalogue.items[i]
				);
			}

			this.recipes = new Dictionary<Recipe.Key, Recipe>();
			for (int i = 0; i < dbInventory.inventoryCatalogue.recipes.Length; ++i)
			{
				this.recipes.Add(
					new Recipe.Key(
						dbInventory.inventoryCatalogue.recipes[i].itemToCombineA.item.uuid,
						dbInventory.inventoryCatalogue.recipes[i].itemToCombineB.item.uuid
					),
					dbInventory.inventoryCatalogue.recipes[i]
				);
			}

			this.playerInventory = new PlayerInventory();
			SaveLoadManager.Instance.Initialize(this, 25);

            SaveLoadManager.Instance.eventOnChangeProfile.AddListener(this.OnChangeProfile);
		}

        protected virtual void OnChangeProfile(int prevProfile, int nextProfile)
        {
            SaveLoadManager.Instance.eventOnChangeProfile.RemoveListener(this.OnChangeProfile);
            Destroy(gameObject);
        }

		// PUBLIC METHODS: ------------------------------------------------------------------------

		public virtual int AddItemToInventory(int uuid, int amount = 1)
		{
			if (!this.itemsCatalogue.ContainsKey(uuid))
			{
				Debug.LogError("Could not find item UUID in item catalogue");
				return 0;
			}

            Item item = this.itemsCatalogue[uuid];

            GameObject player = (HookPlayer.Instance == null ? null : HookPlayer.Instance.gameObject);
            float maxWeight = (INVENTORY.inventorySettings.maxInventoryWeight == null
                ? 100f
                : INVENTORY.inventorySettings.maxInventoryWeight.GetValue(player)
            );

            int amountAdded = 0;
            bool limitWeight = INVENTORY.inventorySettings.limitInventoryWeight;

            for (int i = 0; i < amount; ++i)
            {
                float candidateWeight = this.GetCurrentWeight() + item.weight;
                if (limitWeight && candidateWeight > maxWeight) continue;

                if (this.playerInventory.items.ContainsKey(uuid))
                {
                    int candidateAmount = this.playerInventory.items[uuid] + 1;
                    if (candidateAmount <= this.itemsCatalogue[uuid].maxStack)
                    {
                        this.playerInventory.items[uuid] += 1;
                        amountAdded += 1;
                    }
                }
                else
                {
                    this.playerInventory.items.Add(uuid, 1);
                    amountAdded += 1;
                }
            }

            this.playerInventory.weightDirty = true;
            if (this.eventChangePlayerInventory != null)
            {
                this.eventChangePlayerInventory.Invoke();
            }

            return amountAdded;
        }

        public virtual int SubstractItemFromInventory(int uuid, int amount = 1)
		{
			if (!this.itemsCatalogue.ContainsKey(uuid))
			{
				Debug.LogError("Could not find item UUID in item catalogue");
				return 0;
			}

			if (this.playerInventory.items.ContainsKey(uuid))
			{
                int amountRemoved = 0;
                for (int i = 0; i < amount; ++i)
                {
                    if (this.playerInventory.items[uuid] > 0)
                    {
                        this.playerInventory.items[uuid] -= 1;
                        amountRemoved += 1;
                    }
                }

                GameObject player = HookPlayer.Instance != null ? HookPlayer.Instance.gameObject : null;
                int numEquipped = this.HasEquiped(player, uuid);
                while (numEquipped > this.playerInventory.items[uuid])
                {
                    this.Unequip(player, uuid);
                    --numEquipped;
                }

                if (this.playerInventory.items[uuid] <= 0)
                {
                    this.playerInventory.items.Remove(uuid);
                }

                this.playerInventory.weightDirty = true;
                if (this.eventChangePlayerInventory != null) this.eventChangePlayerInventory.Invoke();
                return amountRemoved;
            }

			return 0;
		}

		public virtual bool ConsumeItem(int uuid, GameObject target = null)
		{
			Item item = this.itemsCatalogue[uuid];
            target = (target == null ? HookPlayer.Instance.gameObject : target);

			if (item == null) return false;
			if (!item.onClick) return false;
			if (item.actionsOnClick.isExecuting) return false;
			if (this.GetInventoryAmountOfItem(uuid) <= 0) return false;

            if (item.consumeItem)
            {
                int amount = this.SubstractItemFromInventory(item.uuid, 1);
                if (amount == 0) return false;
            }

			Vector3 position = Vector3.zero;
			Quaternion rotation = Quaternion.identity;
			if (target != null)
			{
				position = target.transform.position;
				rotation = target.transform.rotation;
			}

			GameObject instance = Instantiate<GameObject>(item.actionsOnClick.gameObject, position, rotation);
			Actions actions = instance.GetComponent<Actions>();
            actions.destroyAfterFinishing = true;
            actions.Execute(target ?? gameObject);
			return true;
		}

        public virtual int GetInventoryAmountOfItem(int uuid)
		{
			if (!this.itemsCatalogue.ContainsKey(uuid))
			{
				Debug.LogError("Could not find item UUID in item catalogue");
				return 0;
			}

			if (this.playerInventory.items.ContainsKey(uuid))
			{
				return this.playerInventory.items[uuid];
			}

			return 0;
		}

        public virtual bool BuyItem(int uuid, int amount, Merchant merchant = null)
		{
			if (!this.itemsCatalogue.ContainsKey(uuid))
			{
				Debug.LogError("Could not find item UUID in item catalogue");
				return false;
			}

            GameObject player = HookPlayer.Instance != null ? HookPlayer.Instance.gameObject : null;
            float percent = (merchant != null
                ? merchant.purchasePercent.GetValue(player)
                : 1.0f
            );

            int totalPrice = Mathf.FloorToInt(this.itemsCatalogue[uuid].price * amount * percent);
			if (totalPrice > this.playerInventory.currencyAmount) return false;

            int amountAdded = this.AddItemToInventory(uuid, amount);
            if (amountAdded > 0)
            {
                int finalPrice = Mathf.FloorToInt(
                    this.itemsCatalogue[uuid].price * amountAdded * percent
                );

                this.SubstractCurrency(finalPrice);
                return true;
            }

            return false;
		}

        public virtual bool SellItem(int uuid, int amount, Merchant merchant = null)
		{
			if (!this.itemsCatalogue.ContainsKey(uuid))
			{
				Debug.LogError("Could not find item UUID in item catalogue");
				return false;
			}

			if (this.GetInventoryAmountOfItem(uuid) < amount) return false;
			this.SubstractItemFromInventory(uuid, amount);

			int price = this.itemsCatalogue[uuid].price * amount;
            if (merchant != null)
            {
                GameObject player = HookPlayer.Instance != null ? HookPlayer.Instance.gameObject : null;
                float percent = merchant.sellPercent.GetValue(player);
                price = Mathf.FloorToInt(price * percent);
            }

            this.AddCurrency(price);
			return true;
		}

        public virtual void AddCurrency(int amount)
		{
			this.playerInventory.currencyAmount += amount;
			if (this.eventChangePlayerCurrency != null) this.eventChangePlayerCurrency.Invoke();
		}

        public virtual void SubstractCurrency(int amount)
		{
			this.playerInventory.currencyAmount -= amount;
			this.playerInventory.currencyAmount = Mathf.Max(0, this.playerInventory.currencyAmount);
			if (this.eventChangePlayerCurrency != null) this.eventChangePlayerCurrency.Invoke();
		}

        public virtual int GetCurrency()
		{
			return this.playerInventory.currencyAmount;
		}

        public virtual void SetCurrency(int value)
        {
            this.playerInventory.currencyAmount = Mathf.Max(value, 0);
            if (this.eventChangePlayerCurrency != null) this.eventChangePlayerCurrency.Invoke();
        }

        public virtual float GetCurrentWeight()
        {
            if (this.playerInventory.weightDirty)
            {
                float weight = 0f;
                foreach (KeyValuePair<int, int> item in this.playerInventory.items)
                {
                    int uuid = item.Key;
                    int amount = item.Value;
                    weight += this.itemsCatalogue[uuid].weight * amount;
                }

                this.playerInventory.weightCached = weight;
                this.playerInventory.weightDirty = false;
            }

            return this.playerInventory.weightCached;
        }

        // RECIPES: -------------------------------------------------------------------------------

        public virtual bool ExistsRecipe(int uuid1, int uuid2)
		{
			bool order1 = this.recipes.ContainsKey(new Recipe.Key(uuid1, uuid2));
			bool order2 = this.recipes.ContainsKey(new Recipe.Key(uuid2, uuid1));
			return (order1 || order2);
		}

		public bool UseRecipe(int uuid1, int uuid2)
		{
			if (!this.ExistsRecipe(uuid1, uuid2)) return false;
			Recipe recipe = this.recipes[new Recipe.Key(uuid1, uuid2)];
			if (recipe == null) recipe = this.recipes[new Recipe.Key(uuid2, uuid1)];
			if (recipe == null) return false;
			if (recipe.actionsList.isExecuting) return false;

			if (recipe.itemToCombineA.item.uuid == uuid2 && recipe.itemToCombineB.item.uuid == uuid1)
			{
				int auxiliar = uuid1;
				uuid1 = uuid2;
				uuid2 = auxiliar;
			}

			if (this.GetInventoryAmountOfItem(uuid1) < recipe.amountA ||
				this.GetInventoryAmountOfItem(uuid2) < recipe.amountB)
			{
				return false;
			}

			if (recipe.removeItemsOnCraft)
			{
				this.SubstractItemFromInventory(recipe.itemToCombineA.item.uuid, recipe.amountA);
				this.SubstractItemFromInventory(recipe.itemToCombineB.item.uuid, recipe.amountB);
			}

			Vector3 position = Vector3.zero;
			Quaternion rotation = Quaternion.identity;
			if (HookPlayer.Instance != null)
			{
				position = HookPlayer.Instance.transform.position;
				rotation = HookPlayer.Instance.transform.rotation;
			}

			GameObject instance = Instantiate<GameObject>(recipe.actionsList.gameObject, position, rotation);
			Actions actions = instance.GetComponent<Actions>();
            actions.destroyAfterFinishing = true;
            actions.Execute();
			return true;
		}

        // EQUIPMENT: -----------------------------------------------------------------------------

        public virtual bool CanEquipItem(GameObject target, int uuid, int itemType)
        {
            CharacterEquipment equipment = this.RequireEquipment(target);
            return (
                equipment.CanEquip(uuid) &&
                this.itemsCatalogue[uuid].equipable &&
                (this.itemsCatalogue[uuid].itemTypes & (1 << itemType)) > 0 &&
                this.itemsCatalogue[uuid].conditionsEquip.Check(target)
            );
        }

        public virtual int HasEquiped(GameObject target, int uuid)
        {
            CharacterEquipment equipment = this.RequireEquipment(target);
            return (equipment != null ? equipment.HasEquip(uuid) : 0);
        }

        public virtual bool HasEquipedTypes(GameObject target, int itemTypes)
        {
            CharacterEquipment equipment = this.RequireEquipment(target);
            return equipment != null && equipment.HasEquipTypes(itemTypes);
        }

        public virtual Item GetEquip(GameObject target, int itemType)
        {
            CharacterEquipment equipment = this.RequireEquipment(target);
            int uuid = equipment.GetEquip(itemType);

            if (uuid != 0) return this.itemsCatalogue[uuid];
            return null;
        }

        public virtual bool Equip(GameObject target, int uuid, int itemType)
        {
            if (this.CanEquipItem(target, uuid, itemType))
            {
                CharacterEquipment equipment = this.RequireEquipment(target);
                if (equipment.CanEquip(uuid) && equipment.EquipItem(uuid, itemType))
                {
                    if (this.eventOnEquip != null) this.eventOnEquip.Invoke(target, uuid);
                    if (this.eventChangePlayerInventory != null) this.eventChangePlayerInventory.Invoke();
                    return true;
                }

                return false;
            }

            return false;
        }

        public virtual bool Unequip(GameObject target, int uuid)
        {
            CharacterEquipment equipment = this.RequireEquipment(target);
            if (equipment.UnequipItem(uuid))
            {
                if (this.eventOnUnequip != null) this.eventOnUnequip.Invoke(target, uuid);
                if (this.eventChangePlayerInventory != null) this.eventChangePlayerInventory.Invoke();
                return true;
            }

            return false;
        }

        public virtual bool UnequipTypes(GameObject target, int itemTypes)
        {
            CharacterEquipment equipment = this.RequireEquipment(target);
            int[] unequipped = equipment.UnequipTypes(itemTypes);

            for (int i = 0; i < unequipped.Length; ++i)
            {
                if (this.eventOnUnequip != null) this.eventOnUnequip.Invoke(target, unequipped[i]);
                if (this.eventChangePlayerInventory != null) this.eventChangePlayerInventory.Invoke();
            }

            return (unequipped.Length > 0);
        }

        protected virtual CharacterEquipment RequireEquipment(GameObject target)
        {
            if (target == null) return null;
            CharacterEquipment equipment = target.GetComponent<CharacterEquipment>();
            if (equipment == null)
            {
                if (HookPlayer.Instance != null && target == HookPlayer.Instance.gameObject)
                {
                    equipment = target.AddComponent<PlayerEquipment>();
                    Debug.LogWarning(WARN_PLYR + " " + WARN_SOLT, target);
                }
                else
                {
                    equipment = target.AddComponent<CharacterEquipment>();
                    Debug.LogWarning(WARN_CHAR + " " + WARN_SOLT, target);
                }
            }

            return equipment;
        }

        // INTERFACE ISAVELOAD: -------------------------------------------------------------------

        public virtual string GetUniqueName()
		{
			return "inventory";
		}

        public virtual Type GetSaveDataType()
		{
			return typeof(InventorySaveData);
		}

        public virtual object GetSaveData()
		{
			InventorySaveData inventorySaveData = new InventorySaveData();
			if (DatabaseInventory.Load().inventorySettings.saveInventory &&
                this.playerInventory != null)
			{
				if (this.playerInventory.items != null && this.playerInventory.items.Count > 0)
				{
					int playerInventoryItemsCount = this.playerInventory.items.Count;
					inventorySaveData.playerItemsUUIDS = new int[playerInventoryItemsCount];
					inventorySaveData.playerItemsStack = new int[playerInventoryItemsCount];

					int itemIndex = 0;
					foreach(KeyValuePair<int, int> entry in this.playerInventory.items)
					{
						inventorySaveData.playerItemsUUIDS[itemIndex] = entry.Key;
						inventorySaveData.playerItemsStack[itemIndex] = entry.Value;
						++itemIndex;
					}
				}

				inventorySaveData.playerCurrencyAmount = this.playerInventory.currencyAmount;
			}

			return inventorySaveData;
		}

        public virtual void ResetData()
		{
			this.playerInventory = new PlayerInventory();
		}

        public virtual void OnLoad(object generic)
		{
            InventorySaveData inventorySaveData = generic as InventorySaveData;

			this.playerInventory = new PlayerInventory();
            if (!DatabaseInventory.Load().inventorySettings.saveInventory ||
                inventorySaveData == null)
            {
                return;
            }

            this.SetCurrency(inventorySaveData.playerCurrencyAmount);
            int playerInventoryItemsCount = inventorySaveData.playerItemsUUIDS.Length;

			for (int i = 0; i < playerInventoryItemsCount; ++i)
			{
				this.playerInventory.items.Add(
					inventorySaveData.playerItemsUUIDS[i],
					inventorySaveData.playerItemsStack[i]
				);
			}
		}
	}
}
