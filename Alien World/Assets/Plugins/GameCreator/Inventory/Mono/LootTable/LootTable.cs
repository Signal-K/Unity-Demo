namespace GameCreator.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    [CreateAssetMenu(fileName = "New Loot Table", menuName = "Game Creator/Inventory/Loot Table")]
    public class LootTable : ScriptableObject
    {
        public class EventLoot : UnityEvent<LootResult> { }

        [System.Serializable]
        public class Loot
        {
            public ItemHolder item = new ItemHolder();
            public int amount = 1;
            public int weight = 1;

            public Loot(ItemHolder item, int amount, int weight)
            {
                this.item = item ?? new ItemHolder();
                this.amount = amount;
                this.weight = weight;
            }

            public Loot()
            {
                this.item = new ItemHolder();
                this.amount = 1;
                this.weight = 1;
            }
        }

        [System.Serializable]
        public class LootResult
        {
            public Item item = null;
            public int amount = 0;

            public LootResult(ItemHolder itemHolder, int amount)
            {
                if (itemHolder != null) this.item = itemHolder.item;
                this.amount = amount;
            }
        }

        // PROPERTIES: ----------------------------------------------------------------------------

        private static readonly EventLoot EVENT_LOOT = new EventLoot();

        public int noDropWeight = 0;
        public Loot[] loot = new Loot[0];

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public LootResult Get()
        {
            List<Loot> chances = new List<Loot>();
            int totalWeight = 0;

            if (this.noDropWeight > 0)
            {
                totalWeight += this.noDropWeight;
                chances.Add(new Loot(null, 0, this.noDropWeight));
            }

            for (int i = 0; i < this.loot.Length; ++i)
            {
                chances.Add(this.loot[i]);
                totalWeight += this.loot[i].weight;
            }

            chances.Sort((Loot x, Loot y) => y.weight.CompareTo(x.weight));
            int random = Random.Range(0, totalWeight);

            for (int i = 0; i < chances.Count; ++i)
            {
                Loot item = chances[i];
                if (random < item.weight)
                {
                    LootResult result = new LootResult(item.item, item.amount);
                    if (result.item == null || result.amount < 1) return new LootResult(null, 0);

                    EVENT_LOOT.Invoke(result);
                    return result;
                }

                random -= item.weight;
            }

            return new LootResult(null, 0);
        }

        // STATIC METHODS: ------------------------------------------------------------------------

        public static void AddListener(UnityAction<LootResult> callback)
        {
            EVENT_LOOT.AddListener(callback);
        }

        public static void RemoveListener(UnityAction<LootResult> callback)
        {
            EVENT_LOOT.RemoveListener(callback);
        }
    }
}