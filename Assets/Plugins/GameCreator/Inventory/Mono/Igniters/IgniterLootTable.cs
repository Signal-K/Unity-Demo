namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using UnityEngine.EventSystems;
    using GameCreator.Core;
    using GameCreator.Variables;

    [AddComponentMenu("")]
	public class IgniterLootTable : Igniter 
	{
		#if UNITY_EDITOR
		public new static string NAME = "Inventory/On Loot Table";
        public new static string ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Igniters/";
        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Igniters/";
        #endif

        [Space] [VariableFilter(Variable.DataType.String)]
        public VariableProperty storeItemName = new VariableProperty();

        [Space] [VariableFilter(Variable.DataType.Number)]
        public VariableProperty storeItemAmount = new VariableProperty();

        private void Start()
        {
            LootTable.AddListener(this.OnUseLootTable);
        }

        private void OnDestroy()
        {
            if (this.isExitingApplication) return;
            LootTable.RemoveListener(this.OnUseLootTable);
        }

        private void OnUseLootTable(LootTable.LootResult result)
        {
            string itemName = result.item.itemName.GetText();
            float itemAmount = result.amount;

            this.storeItemName.Set(itemName, gameObject);
            this.storeItemAmount.Set(itemAmount, gameObject);

            this.ExecuteTrigger(gameObject);
        }
    }
}