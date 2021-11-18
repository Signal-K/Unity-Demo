namespace GameCreator.Inventory
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GameCreator.Core;

	#if UNITY_EDITOR
	using UnityEditor;
	using System.IO;
	#endif

	[System.Serializable]
	public class Recipe : ScriptableObject 
	{
		[System.Serializable]
		public class Key : IEquatable<Recipe.Key>
		{
			public int item1;
			public int item2;

			public Key(int item1, int item2)
			{
				this.item1 = item1;
				this.item2 = item2;
			}

			public override int GetHashCode() 
			{
				return this.item1 ^ this.item2;
			}

			public override bool Equals(object other) 
			{
				return Equals(other as Recipe.Key);
			}

			public bool Equals(Recipe.Key other) 
			{
				if (this.item1 == other.item1 && this.item2 == other.item2) return true;
				if (this.item1 == other.item2 && this.item2 == other.item1) return true;
				return false;
			}
		}

		// PROPERTIES: -------------------------------------------------------------------------------------------------

		public ItemHolder itemToCombineA;
		public int amountA = 1;

		public ItemHolder itemToCombineB;
		public int amountB = 1;

		[Tooltip("If on, itemA and itemB will be removed from the Player's inventory.")]
		public bool removeItemsOnCraft = true;
		public IActionsList actionsList;

		// CONSTRUCTOR: ------------------------------------------------------------------------------------------------

		#if UNITY_EDITOR
		
        public static Recipe CreateRecipeInstance()
		{
			Recipe recipe = ScriptableObject.CreateInstance<Recipe>();
			Guid guid = Guid.NewGuid();

			recipe.itemToCombineA = new ItemHolder();
			recipe.itemToCombineB = new ItemHolder();
			recipe.removeItemsOnCraft = true;

			recipe.name = "recipe." + Mathf.Abs(guid.GetHashCode());
			recipe.hideFlags = HideFlags.HideInHierarchy;

			DatabaseInventory databaseInventory = DatabaseInventory.Load();
			AssetDatabase.AddObjectToAsset(recipe, databaseInventory);

			AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(recipe));
			return recipe;
		}

        private void OnDestroy()
        {
            if (this.actionsList == null) return;
            GameObject prefabAction = this.actionsList.gameObject;
            if (prefabAction != null) DestroyImmediate(prefabAction, true);
        }

		#endif
	}
}