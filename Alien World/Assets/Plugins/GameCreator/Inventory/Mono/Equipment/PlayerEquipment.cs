namespace GameCreator.Inventory
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using GameCreator.Core;
    using GameCreator.Characters;

    [DisallowMultipleComponent]
    [AddComponentMenu("Game Creator/Characters/Player Equipment")]
    public class PlayerEquipment : CharacterEquipment
    {
        // OVERRIDE METHODS: ----------------------------------------------------------------------

        public override bool CanEquip(int itemID)
        {
            int amountEquipped = this.HasEquip(itemID);
            int amountInventory = InventoryManager.Instance.GetInventoryAmountOfItem(itemID);

            return (amountEquipped < amountInventory);
        }

        protected override string GetUniqueID()
        {
            return "player";
        }
    }
}