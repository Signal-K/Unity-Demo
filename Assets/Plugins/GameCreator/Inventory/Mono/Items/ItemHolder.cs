namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GameCreator.Core;

	[System.Serializable]
	public class ItemHolder
	{
		public Item item;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override string ToString()
        {
            if (this.item == null) return "(none)";
            return this.item.itemName.content;
        }
    }
}