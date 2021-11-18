namespace GameCreator.Inventory
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using UnityEngine.Serialization;
	using GameCreator.Core;
	using GameCreator.Localization;

	#if UNITY_EDITOR
	using UnityEditor;
	using System.IO;
	#endif

	[Serializable]
	public class Item : ScriptableObject
	{
		// PROPERTIES: -------------------------------------------------------------------------------------------------

		public int uuid = -1;
        [LocStringNoPostProcess] public LocString itemName = new LocString();
        [LocStringNoPostProcess] public LocString itemDescription = new LocString();
        public Color itemColor = Color.grey;

		public Sprite sprite;
		public GameObject prefab;

		public bool canBeSold = true;
		public int price = 0;
		public int maxStack = 99;
        public float weight = 0f;

        [InventoryMultiItemType]
        public int itemTypes = 0;

        [FormerlySerializedAs("consumable")]
		public bool onClick = true;
        public bool consumeItem = true;

        [FormerlySerializedAs("actionsList")]
		public IActionsList actionsOnClick;

        public bool equipable = false;
        public bool fillAllTypes = false;
        public IConditionsList conditionsEquip;
        public IActionsList actionsOnEquip;
        public IActionsList actionsOnUnequip;

		// CONSTRUCTOR: ------------------------------------------------------------------------------------------------

		#if UNITY_EDITOR
		
        public static Item CreateItemInstance()
		{
			Item item = ScriptableObject.CreateInstance<Item>();
			Guid guid = Guid.NewGuid();

			item.name = "item." + Mathf.Abs(guid.GetHashCode());
			item.uuid = Mathf.Abs(guid.GetHashCode());

			item.itemName = new LocString();
			item.itemDescription = new LocString();
			item.price = 1;
			item.maxStack = 99;
			item.hideFlags = HideFlags.HideInHierarchy;

			DatabaseInventory databaseInventory = DatabaseInventory.Load();
			AssetDatabase.AddObjectToAsset(item, databaseInventory);
			AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(item));
			return item;
		}

		private void OnDestroy()
		{
            this.DestroyAsset(this.actionsOnClick);
            this.DestroyAsset(this.conditionsEquip);
            this.DestroyAsset(this.actionsOnEquip);
            this.DestroyAsset(this.actionsOnUnequip);
		}

        private void DestroyAsset(MonoBehaviour reference)
        {
            if (reference == null) return;
            if (reference.gameObject == null) return;
            DestroyImmediate(reference.gameObject, true);
        }

        #endif
	}
}