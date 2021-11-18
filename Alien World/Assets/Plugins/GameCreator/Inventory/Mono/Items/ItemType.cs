namespace GameCreator.Inventory
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using GameCreator.Core;
    using GameCreator.Localization;

    [Serializable]
    public class ItemType
    {
        public const int MAX = 32;

        // PROPERTIES: ----------------------------------------------------------------------------

        public string id;

        [LocStringNoPostProcess]
        public LocString name;

        // INITIALIZERS: --------------------------------------------------------------------------

        public ItemType()
        {
            this.id = "";
            this.name = new LocString();
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////
    // ITEM ATTRIBUTE: ----------------------------------------------------------------------------

    public class InventoryMultiItemTypeAttribute : PropertyAttribute
    {
        public InventoryMultiItemTypeAttribute() { }
    }

    public class InventorySingleItemTypeAttribute : PropertyAttribute
    {
        public InventorySingleItemTypeAttribute() { }
    }
}