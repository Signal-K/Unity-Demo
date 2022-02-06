namespace GameCreator.Inventory
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using GameCreator.Core;
    using GameCreator.Characters;
    using GameCreator.Core.Hooks;

    [DisallowMultipleComponent]
    [AddComponentMenu("Game Creator/Characters/Equipment")]
    public class CharacterEquipment : GlobalID, IGameSave
    {
        [Serializable]
        public class Item
        {
            public bool isEquipped;
            public int itemID;

            public Item()
            {
                this.isEquipped = false;
                this.itemID = 0;
            }

            public Item(int itemID)
            {
                this.isEquipped = true;
                this.itemID = itemID;
            }
        }

        [Serializable]
        public class Equipment
        {
            public Item[] items;

            public Equipment()
            {
                this.items = new Item[ItemType.MAX]
                {
                    new Item(), new Item(), new Item(), new Item(),
                    new Item(), new Item(), new Item(), new Item(),
                    new Item(), new Item(), new Item(), new Item(),
                    new Item(), new Item(), new Item(), new Item(),
                    new Item(), new Item(), new Item(), new Item(),
                    new Item(), new Item(), new Item(), new Item(),
                    new Item(), new Item(), new Item(), new Item(),
                    new Item(), new Item(), new Item(), new Item(),
                };
            }
        }

        // PROPERTIES: ----------------------------------------------------------------------------

        public Equipment equipment = new Equipment();
        public bool saveEquipment = true;

        // INITIALIZERS: --------------------------------------------------------------------------

        private void Start()
        {
            if (!Application.isPlaying) return;
            SaveLoadManager.Instance.Initialize(this);
        }

        private void OnDestroy()
        {
            base.OnDestroyGID();

            if (!Application.isPlaying) return;
            if (this.exitingApplication) return;
            SaveLoadManager.Instance.OnDestroyIGameSave(this);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public virtual bool CanEquip(int itemID)
        {
            return true;
        }

        public int HasEquip(int itemID)
        {
            int counter = 0;
            for (int i = 0; i < this.equipment.items.Length; ++i)
            {
                if (this.equipment.items[i].isEquipped && 
                    this.equipment.items[i].itemID == itemID)
                {
                    counter += 1;
                }
            }

            return counter;
        }

        public bool HasEquipTypes(int itemTypes)
        {
            for (int i = 0; i < ItemType.MAX; ++i)
            {
                if (((itemTypes >> i) & 1) > 0 && !this.equipment.items[i].isEquipped)
                {
                    return false;
                }
            }

            return true;
        }

        public int GetEquip(int itemType)
        {
            return (this.equipment.items[itemType].isEquipped 
                ? this.equipment.items[itemType].itemID
                : 0
            );
        }

        public bool EquipItem(int itemID, int itemType, Action onEquip = null)
        {
            Inventory.Item item = InventoryManager.Instance.itemsCatalogue[itemID];
            if (!item.conditionsEquip.Check(gameObject))
            {
                return false;
            }

            List<int> itemTypes = new List<int>();
            if (InventoryManager.Instance.itemsCatalogue[itemID].fillAllTypes)
            {
                for (int i = 0; i < ItemType.MAX; ++i)
                {
                    if (((item.itemTypes >> i) & 1) > 0)
                    {
                        itemTypes.Add(i);
                    }
                }
            }
            else
            {
                itemTypes.Add(itemType);
            }

            int numToUnequip = 0;
            int numUnequipped = 0;

            for (int i = 0; i < itemTypes.Count; ++i)
            {
                if (this.equipment.items[itemTypes[i]].isEquipped)
                {
                    numToUnequip += 1;
                    this.UnequipItem(this.equipment.items[itemTypes[i]].itemID, () =>
                    {
                        numUnequipped += 1;
                        if (numUnequipped >= numToUnequip)
                        {
                            this.ExecuteActions(item.actionsOnEquip, onEquip);
                        }
                    });
                }

                this.equipment.items[itemTypes[i]].isEquipped = true;
                this.equipment.items[itemTypes[i]].itemID = itemID;
            }

            if (numToUnequip == 0) this.ExecuteActions(item.actionsOnEquip, onEquip);

            return true;
        }

        public bool UnequipItem(int itemID, Action onUnequip = null)
        {
            bool unequipped = false;

            // int itemsToUnequip = 0;
            // int itemsUnequipped = 0;

            bool calledUnequipActions = false;

            for (int i = 0; i < this.equipment.items.Length; ++i)
            {
                if (this.equipment.items[i].isEquipped && this.equipment.items[i].itemID == itemID)
                {
                    Inventory.Item item = InventoryManager.Instance.itemsCatalogue[itemID];

                    this.equipment.items[i].isEquipped = false;

                    // itemsToUnequip += 1;

                    if (!calledUnequipActions)
                    {
                        GameObject instance = Instantiate<GameObject>(
                            item.actionsOnUnequip.gameObject,
                            transform.position,
                            transform.rotation
                        );
                        
                        Actions actions = instance.GetComponent<Actions>();
                        actions.destroyAfterFinishing = true;
                        actions.onFinish.AddListener(() =>
                        {
                            // itemsUnequipped += 1;
                            // if (itemsUnequipped >= itemsToUnequip && onUnequip != null)
                            // {
                            //     onUnequip.Invoke();
                            // }
                            if (onUnequip != null) onUnequip.Invoke();
                        });

                        actions.Execute(gameObject);
                        calledUnequipActions = true;
                    }
                    
                    unequipped = true;
                }
            }

            return unequipped;
        }

        public int[] UnequipTypes(int itemTypes)
        {
            List<int> unequipped = new List<int>();
            for (int i = 0; i < ItemType.MAX; ++i)
            {
                if (((itemTypes >> i) & 1) > 0)
                {
                    if (this.equipment.items[i].isEquipped)
                    {
                        unequipped.Add(this.equipment.items[i].itemID);
                        this.equipment.items[i].isEquipped = false;

                        GameObject instance = Instantiate(
                            InventoryManager
                                .Instance
                                .itemsCatalogue[this.equipment.items[i].itemID]
                                .actionsOnUnequip.gameObject,
                            transform.position,
                            transform.rotation
                        );

                        Actions actions = instance.GetComponent<Actions>();
                        actions.destroyAfterFinishing = true;
                        actions.Execute(gameObject);
                    }
                }
            }

            return unequipped.ToArray();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void ExecuteActions(IActionsList list, Action callback = null)
        {
            GameObject instance = Instantiate<GameObject>(
                list.gameObject,
                transform.position,
                transform.rotation
            );

            Actions actions = instance.GetComponent<Actions>();
            actions.destroyAfterFinishing = true;
            actions.onFinish.AddListener(() =>
            {
                if (callback != null) callback.Invoke();
            });

            actions.Execute(gameObject);
        }

        // IGAMESAVE: -----------------------------------------------------------------------------

        public string GetUniqueName()
        {
            return string.Format("equip:{0}", this.GetUniqueID());
        }

        protected virtual string GetUniqueID()
        {
            return this.GetID();
        }

        public Type GetSaveDataType()
        {
            return typeof(Equipment);
        }

        public object GetSaveData()
        {
            if (!this.saveEquipment) return null;
            return this.equipment;
        }

        public void ResetData()
        {
            for (int i = 0; i < this.equipment.items.Length; ++i)
            {
                if (this.equipment.items[i].isEquipped)
                {
                    this.UnequipItem(this.equipment.items[i].itemID);
                }
            }

            this.equipment = new Equipment();
        }

        public void OnLoad(object generic)
        {
            Equipment savedEquipment = generic as Equipment;
            if (!this.saveEquipment || savedEquipment == null) return;

            StartCoroutine(this.OnLoadNextFrame(savedEquipment));
        }

        private IEnumerator OnLoadNextFrame(Equipment savedEquipment)
        {
            Debug.Log("On Load");

            yield return null;
            for (int i = 0; i < savedEquipment.items.Length; ++i)
            {
                if (savedEquipment.items[i].isEquipped)
                {
                    this.EquipItem(savedEquipment.items[i].itemID, i);
                }
            }
        }
    }
}