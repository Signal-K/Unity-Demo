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

    [AddComponentMenu("Game Creator/UI/Item")]
	public class ItemUI : MonoBehaviour 
	{
		protected static DatabaseInventory DATABASE_INVENTORY;

		// PROPERTIES: ----------------------------------------------------------------------------

        [HideInInspector]
		public Item item { get; protected set; }

        protected Button button;

        public Image image;
        public Graphic color;
        public Text textName;
        public Text textDescription;

        [Space]
        public GameObject containerAmount;
		public Text textAmount;

        [Space]
        public GameObject equipped;

		[Space]
        public UnityEvent eventOnHoverEnter;
        public UnityEvent eventOnHoverExit;

		// CONSTRUCTOR & UPDATER: -----------------------------------------------------------------

		public virtual void Setup(Item item, int amount)
		{
			this.UpdateUI(item, amount);
			this.button = gameObject.GetComponentInChildren<Button>();

			if (DATABASE_INVENTORY == null) DATABASE_INVENTORY = DatabaseInventory.Load();
            if (DATABASE_INVENTORY.inventorySettings.onDragGrabItem)
			{
				this.SetupEvents(EventTriggerType.BeginDrag, this.OnDragBegin);
				this.SetupEvents(EventTriggerType.EndDrag, this.OnDragEnd);
				this.SetupEvents(EventTriggerType.Drag, this.OnDragMove);
			}

            this.SetupEvents(EventTriggerType.PointerEnter, this.OnPointerEnter);
            this.SetupEvents(EventTriggerType.PointerExit, this.OnPointerExit);
		}

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Select()
        {
	        SelectedItemUI.Select(this.item.uuid);
        }

        public void Deselect()
        {
	        SelectedItemUI.Deselect();
        }
        
        public virtual void UpdateUI(Item item, int amount)
		{
			this.item = item;

			if (this.image != null && item.sprite != null) this.image.sprite = item.sprite;
            if (this.color != null) this.color.color = item.itemColor;

            if (this.textName != null) this.textName.text = item.itemName.GetText();
            if (this.textDescription != null) this.textDescription.text = item.itemDescription.GetText();
            if (this.textAmount != null)
            {
                this.textAmount.text = amount.ToString();
                if (this.containerAmount != null)
                {
                    this.containerAmount.SetActive(amount != 1);
                }
            }

            if (this.equipped != null)
            {
                int equippedAmount = InventoryManager.Instance.HasEquiped(
                    HookPlayer.Instance == null ? null : HookPlayer.Instance.gameObject,
                    this.item.uuid
                );

                this.equipped.SetActive(equippedAmount > 0);
            }
		}

		public virtual void OnClick()
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); 
			InventoryManager.Instance.ConsumeItem(this.item.uuid);
			InventoryUIManager.OnDragItem(this.item.sprite, false);
		}

		public virtual void OnDragBegin(BaseEventData eventData)
		{
			if (DATABASE_INVENTORY != null && DATABASE_INVENTORY.inventorySettings.cursorDrag != null)
			{
				Cursor.SetCursor(
					DATABASE_INVENTORY.inventorySettings.cursorDrag, 
					DATABASE_INVENTORY.inventorySettings.cursorDragHotspot,
					CursorMode.Auto
				);
			}

			if (DATABASE_INVENTORY != null && 
				DATABASE_INVENTORY.inventorySettings.onDragGrabItem && this.item.sprite != null)
			{
				InventoryUIManager.OnDragItem(this.item.sprite, true);
			}

			eventData.Use();
		}

		public virtual void OnDragEnd(BaseEventData eventData)
		{
			if (DATABASE_INVENTORY != null && DATABASE_INVENTORY.inventorySettings.cursorDrag != null)
			{
				Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); 
			}

			InventoryUIManager.OnDragItem(null, false);
			PointerEventData pointerData = ((PointerEventData)eventData);
			if (pointerData == null) return;
            if (pointerData.pointerCurrentRaycast.gameObject == null) return;
            
			GameObject target = pointerData.pointerCurrentRaycast.gameObject;
			IgniterDropToConsume[] targetsDrop = target.GetComponents<IgniterDropToConsume>();
            EquipSlotUI[] targetsEquipSlotUI = target.GetComponents<EquipSlotUI>();
			HotbarUI[] targetsHotbarUI = target.GetComponents<HotbarUI>();
			ItemUI targetItemUI = target.GetComponent<ItemUI>();

            if (targetsDrop.Length > 0)
            {
                for (int i = 0; i < targetsDrop.Length; ++i)
                {
					targetsDrop[i].OnDrop(this.item);
					eventData.Use();
				}
            }
            else if (targetsEquipSlotUI.Length > 0)
            {
				for (int i = 0; i < targetsEquipSlotUI.Length; ++i)
				{
					targetsEquipSlotUI[i].OnDrop(this.item);
					eventData.Use();
				}
            }
            else if (targetsHotbarUI.Length > 0)
            {
				for (int i = 0; i < targetsHotbarUI.Length; ++i)
				{
					targetsHotbarUI[i].OnDrop(this.item);
					eventData.Use();
				}
			}
			else if (targetItemUI == null)
			{
                bool mouseOverUI = EventSystemManager.Instance.IsPointerOverUI();
                if (!mouseOverUI && DATABASE_INVENTORY.inventorySettings.canDropItems && this.item.prefab != null)
				{
					Vector3 position = pointerData.pointerCurrentRaycast.worldPosition + (Vector3.up * 0.1f);
                    Vector3 direction = position - HookPlayer.Instance.transform.position;

                    if (direction.magnitude > DATABASE_INVENTORY.inventorySettings.dropItemMaxDistance)
                    {
                        position = (
                            HookPlayer.Instance.transform.position +
                            direction.normalized * DATABASE_INVENTORY.inventorySettings.dropItemMaxDistance
                        );
                    }

					Instantiate(this.item.prefab, position, Quaternion.identity);
					InventoryManager.Instance.SubstractItemFromInventory(this.item.uuid, 1);
				}
				
				eventData.Use();
			}
			else if (this.item.uuid != targetItemUI.item.uuid)
			{
				Button otherButton = pointerData.pointerCurrentRaycast.gameObject.GetComponentInChildren<Button>();
				if (otherButton != null)
				{
					this.HighlightButton(this.button, false);
					this.HighlightButton(otherButton, false);
				}

				InventoryManager.Instance.UseRecipe(this.item.uuid, targetItemUI.item.uuid);
				eventData.Use();
			}
		}

		public virtual void OnDragMove(BaseEventData eventData)
		{
			PointerEventData pointerData = ((PointerEventData)eventData);
			if (pointerData == null) return;

			if (DATABASE_INVENTORY != null && 
				DATABASE_INVENTORY.inventorySettings.onDragGrabItem && this.item.sprite != null)
			{
				InventoryUIManager.OnDragItem(this.item.sprite, true);
			}

            if (pointerData.pointerCurrentRaycast.gameObject == null) return;
			GameObject target = pointerData.pointerCurrentRaycast.gameObject;
            ItemUI otherItemUI = target.GetComponent<ItemUI>();

            if (otherItemUI != null)
            {
                Button otherButton = pointerData.pointerCurrentRaycast.gameObject.GetComponentInChildren<Button>();
                if (otherButton != null)
                {
                    this.HighlightButton(this.button, true);
                    this.HighlightButton(otherButton, true);
                }

                eventData.Use();
            }
		}

        // PROTECTED METHODS: ---------------------------------------------------------------------

		protected void SetupEvents(EventTriggerType eventType, UnityAction<BaseEventData> callback)
		{
			EventTrigger.Entry eventTriggerEntry = new EventTrigger.Entry();
			eventTriggerEntry.eventID = eventType;
			eventTriggerEntry.callback.AddListener(callback);

			EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>();
			if (eventTrigger == null) eventTrigger = gameObject.AddComponent<EventTrigger>();

			eventTrigger.triggers.Add(eventTriggerEntry);
		}

        protected void HighlightButton(Button button, bool highlight)
		{
			switch (button.transition)
			{
			case Selectable.Transition.ColorTint:
				button.image.color = (highlight ? button.colors.pressedColor : Color.white);
				break;

			case Selectable.Transition.Animation:
				button.animator.SetTrigger((highlight 
					? button.animationTriggers.pressedTrigger 
					: button.animationTriggers.normalTrigger)
				);
				break;

			case Selectable.Transition.SpriteSwap: 
				button.image.overrideSprite = (highlight
					? button.spriteState.pressedSprite
					: button.image.sprite
				);
				break;
			}
		}

        protected void OnPointerEnter(BaseEventData eventData)
		{
			if (this.eventOnHoverEnter != null) this.eventOnHoverEnter.Invoke();
		}

        protected void OnPointerExit(BaseEventData eventData)
		{
			if (this.eventOnHoverExit != null) this.eventOnHoverExit.Invoke();
		}
	}
}