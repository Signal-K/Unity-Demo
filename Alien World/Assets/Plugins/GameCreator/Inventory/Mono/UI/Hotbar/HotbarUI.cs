namespace GameCreator.Inventory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.Events;
    using UnityEngine.UI;
    using GameCreator.Core;
    using GameCreator.Core.Hooks;

    [AddComponentMenu("Game Creator/UI/Hotbar Item")]
    public class HotbarUI : MonoBehaviour
    {
        [Space]
        public KeyCode keyCode = KeyCode.Space;

        [InventoryMultiItemType]
        public int acceptItemTypes = -1;

        [Space]
        public Image itemImage;
        public Text itemText;
        public Text itemDescription;
        public Text itemCount;

        [Space]
        public UnityEvent eventOnHoverEnter;
        public UnityEvent eventOnHoverExit;

        private Item item = null;

        // INITIALIZERS: --------------------------------------------------------------------------

        private void Awake()
        {
            this.SetupEvents(EventTriggerType.PointerEnter, this.OnPointerEnter);
            this.SetupEvents(EventTriggerType.PointerExit, this.OnPointerExit);

            this.UpdateUI();
        }

        protected void SetupEvents(EventTriggerType eventType, UnityAction<BaseEventData> callback)
        {
            EventTrigger.Entry eventTriggerEntry = new EventTrigger.Entry();
            eventTriggerEntry.eventID = eventType;
            eventTriggerEntry.callback.AddListener(callback);

            EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>();
            if (eventTrigger == null) eventTrigger = gameObject.AddComponent<EventTrigger>();

            eventTrigger.triggers.Add(eventTriggerEntry);
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        private void Update()
        {
            if (this.item == null) return;
            if (!Input.GetKeyDown(this.keyCode)) return;

            InventoryManager.Instance.ConsumeItem(this.item.uuid, HookPlayer.Instance.gameObject);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool BindItem(Item item)
        {
            if (item == null) return false;
            if ((item.itemTypes & this.acceptItemTypes) == 0) return false;
            if (InventoryManager.Instance.GetInventoryAmountOfItem(item.uuid) == 0) return false;

            this.item = item;
            this.UpdateUI();

            InventoryManager.Instance.eventChangePlayerInventory.AddListener(this.UpdateUI);
            return true;
        }

        public void UnbindItem()
        {
            this.item = null;

            this.OnPointerExit(null);
            this.UpdateUI();

            InventoryManager.Instance.eventChangePlayerInventory.RemoveListener(this.UpdateUI);
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void UpdateUI()
        {
            if (this.item != null &&
                InventoryManager.Instance.GetInventoryAmountOfItem(this.item.uuid) == 0)
            {
                this.UnbindItem();
                return;
            }

            if (this.itemImage != null) this.itemImage.overrideSprite = this.item != null
                ? this.item.sprite
                : null;

            if (this.itemText != null) this.itemText.text = this.item != null
                ? this.item.itemName.GetText()
                : string.Empty;

            if (this.itemDescription != null) this.itemDescription.text = this.item != null
                ? this.item.itemDescription.GetText()
                : string.Empty;

            if (this.itemCount != null) this.itemCount.text = this.item != null
                ? InventoryManager.Instance.GetInventoryAmountOfItem(this.item.uuid).ToString()
                : 0.ToString();
        }

        public void OnDrop(Item item)
        {
            this.BindItem(item);
        }

        protected void OnPointerEnter(BaseEventData eventData)
        {
            if (this.item == null) return;
            if (this.eventOnHoverEnter != null) this.eventOnHoverEnter.Invoke();
        }

        protected void OnPointerExit(BaseEventData eventData)
        {
            if (this.eventOnHoverExit != null) this.eventOnHoverExit.Invoke();
        }
    }
}