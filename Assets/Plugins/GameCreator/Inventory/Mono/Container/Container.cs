using System.Runtime.CompilerServices;
namespace GameCreator.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using GameCreator.Core;
    using System;
    using UnityEngine.Events;

    [ExecuteAlways]
    [AddComponentMenu("Game Creator/Inventory/Container")]
    public class Container : GlobalID, IGameSave
    {
        [Serializable]
        public class Data
        {
            public ItemsContainer items = new ItemsContainer();
        }

        [Serializable]
        public class ItemsContainer : SerializableDictionaryBase<int, ItemData> 
        { }

        [Serializable]
        public class ItemData
        {
            public int uuid = 0;
            public int amount = 0;

            public ItemData(int uuid, int amount)
            {
                this.uuid = uuid;
                this.amount = amount;
            }
        }

        [Serializable]
        public class InitData
        {
            public ItemHolder item = new ItemHolder();
            public int amount = 0;
        }

        public class EventAdd : UnityEvent<int, int> {}
        public class EventRmv : UnityEvent<int, int> { }

        // PROPERTIES: ----------------------------------------------------------------------------

        public Data data { private set; get; }

        public List<InitData> initItems = new List<InitData>();

        public bool saveContainer = true;
        public GameObject containerUI;

        private readonly EventAdd eventAdd = new EventAdd();
        private readonly EventRmv eventRmv = new EventRmv();

        // INITIALIZERS: --------------------------------------------------------------------------

        private void Start()
        {
            if (!Application.isPlaying) return;

            this.Initialize();
            SaveLoadManager.Instance.Initialize(this);
        }

        private void Initialize()
        {
            this.data = new Data();
            for (int i = 0; i < this.initItems.Count; ++i)
            {
                Item item = this.initItems[i].item.item;
                if (item == null || item.uuid == 0) continue;

                int uuid = item.uuid;
                int amount = this.initItems[i].amount;
                if (amount < 1) continue;

                if (this.data.items.ContainsKey(uuid))
                {
                    this.data.items[uuid].amount += amount;
                }
                else
                {
                    this.data.items.Add(uuid, new ItemData(uuid, amount));
                }
            }
        }

        protected virtual void OnDestroy()
        {
            this.OnDestroyGID();

            if (!Application.isPlaying) return;
            if (this.exitingApplication) return;

            SaveLoadManager.Instance.OnDestroyIGameSave(this);
        }

        // GETTER METHODS: ------------------------------------------------------------------------

        public List<ItemData> GetItems()
        {
            List<ItemData> items = new List<ItemData>();
            foreach (KeyValuePair<int, ItemData> element in this.data.items)
            {
                items.Add(element.Value);
            }

            return items;
        }

        public int GetAmount(int uuid)
        {
            if (!this.data.items.ContainsKey(uuid)) return 0;
            return this.data.items[uuid].amount;
        }

        // SETTER METHODS: ------------------------------------------------------------------------

        public void AddItem(int uuid, int amount = 1)
        {
            if (amount < 1) return;

            if (this.data.items.ContainsKey(uuid))
            {
                this.data.items[uuid].amount += amount;
                this.eventAdd.Invoke(uuid, amount);
            }
            else
            {
                this.data.items.Add(uuid, new ItemData(uuid, amount));
                this.eventAdd.Invoke(uuid, amount);
            }
        }

        public void RemoveItem(int uuid, int amount = 1)
        {
            if (amount < 1) return;

            if (this.data.items.ContainsKey(uuid))
            {
                int remaining = this.data.items[uuid].amount - amount;
                if (remaining < 0) amount = this.data.items[uuid].amount;

                this.data.items[uuid].amount -= amount;
                if (this.data.items[uuid].amount < 1)
                {
                    this.data.items.Remove(uuid);
                }

                this.eventRmv.Invoke(uuid, amount);
            }
        }

        // EVENT METHODS: -------------------------------------------------------------------------

        public void AddOnAddListener(UnityAction<int, int> callback)
        {
            this.eventAdd.AddListener(callback);
        }

        public void RemoveOnAddListener(UnityAction<int, int> callback)
        {
            this.eventAdd.RemoveListener(callback);
        }

        public void AddOnRemoveListener(UnityAction<int, int> callback)
        {
            this.eventRmv.AddListener(callback);
        }

        public void RemoveOnRemoveListener(UnityAction<int, int> callback)
        {
            this.eventRmv.RemoveListener(callback);
        }

        // IGAMESAVE: -----------------------------------------------------------------------------

        public object GetSaveData()
        {
            if (!this.saveContainer) return null;
            return this.data;
        }

        public Type GetSaveDataType()
        {
            return typeof(Container.Data);
        }

        public string GetUniqueName()
        {
            string uniqueName = string.Format(
                "container:{0}",
                this.GetID()
            );

            return uniqueName;
        }

        public void OnLoad(object generic)
        {
            Container.Data newData = generic as Container.Data;
            this.data = new Data();

            if (newData == null || !this.saveContainer) return;

            foreach (KeyValuePair<int, ItemData> item in newData.items)
            {
                int uuid = item.Value.uuid;
                int amount = item.Value.amount;

                if (this.data.items.ContainsKey(uuid))
                {
                    this.data.items[uuid].amount = amount;
                }
                else
                {
                    this.data.items.Add(uuid, new ItemData(uuid, amount));
                }
            }
        }

        public void ResetData()
        {
            this.Initialize();
        }
    }
}