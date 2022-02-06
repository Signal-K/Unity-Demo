namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
	using GameCreator.Core;
    using GameCreator.Core.Hooks;

    [AddComponentMenu("Game Creator/UI/Equip Slot")]
    public class EquipSlotUI : MonoBehaviour 
	{
		private static DatabaseInventory DATABASE_INVENTORY;

        // PROPERTIES: ----------------------------------------------------------------------------

        [InventorySingleItemType]
        public int itemType = 1;

        public Image equipmentImage;
        public Text equipmentText;

        public Image frameImage;
        public Sprite onUnequipped;
        public Sprite onEquipped;
        public Sprite onHighlight;

        private bool isExittingApplication = false;

		// INITIALIZERS: --------------------------------------------------------------------------

        private void Start()
		{
			if (DATABASE_INVENTORY == null) DATABASE_INVENTORY = DatabaseInventory.Load();
            this.SetupEvents(EventTriggerType.PointerClick, this.OnClick);
		}

        private void OnEnable()
        {
            InventoryManager.Instance.eventOnEquip.AddListener(this.OnEquip);
            InventoryManager.Instance.eventOnUnequip.AddListener(this.OnEquip);
            this.UpdateUI();
        }

        private void OnDisable()
        {
            if (this.isExittingApplication) return;
            InventoryManager.Instance.eventOnEquip.RemoveListener(this.OnEquip);
            InventoryManager.Instance.eventOnUnequip.RemoveListener(this.OnEquip);
        }

        private void OnApplicationQuit()
        {
            this.isExittingApplication = true;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnEquip(GameObject target, int itemID)
        {
            this.UpdateUI();
        }

        private void UpdateUI()
		{
            Item item = null;
            if (HookPlayer.Instance != null)
            {
                item = InventoryManager.Instance.GetEquip(
                    HookPlayer.Instance.gameObject,
                    this.itemType
                );
            }

            if (item != null)
            {
                if (this.frameImage != null) this.frameImage.sprite = this.onEquipped;
                if (this.equipmentImage != null) this.equipmentImage.overrideSprite = item.sprite;
                if (this.equipmentText != null) this.equipmentText.text = item.itemName.GetText();
            }
            else
            {
                if (this.frameImage != null) this.frameImage.sprite = this.onUnequipped;
                if (this.equipmentImage != null) this.equipmentImage.overrideSprite = null;
                if (this.equipmentText != null) this.equipmentText.text = string.Empty;
            }
		}

        public void OnClick(BaseEventData eventData)
        {
            Item item = InventoryManager.Instance.GetEquip(
                HookPlayer.Instance.gameObject, 
                this.itemType
            );

            if (item != null)
            {
                InventoryManager.Instance.Unequip(
                    HookPlayer.Instance.gameObject,
                    item.uuid
                );
            }
        }

        public void OnDrop(Item item)
        {
            if (item == null) return;
            InventoryManager.Instance.Equip(
                HookPlayer.Instance.gameObject,
                item.uuid,
                this.itemType
            );
        }

        // OTHER METHODS: -------------------------------------------------------------------------

		private void SetupEvents(EventTriggerType eventType, UnityAction<BaseEventData> callback)
		{
			EventTrigger.Entry eventTriggerEntry = new EventTrigger.Entry();
			eventTriggerEntry.eventID = eventType;
			eventTriggerEntry.callback.AddListener(callback);

			EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>();
			if (eventTrigger == null) eventTrigger = gameObject.AddComponent<EventTrigger>();

			eventTrigger.triggers.Add(eventTriggerEntry);
		}
	}
}