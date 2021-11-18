namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using GameCreator.Core;

	public class InventoryUIManager : MonoBehaviour
	{
        private const int TIME_LAYER = 200;

		private static InventoryUIManager Instance;
		private static DatabaseInventory DATABASE_INVENTORY;

        private const string DEFAULT_UI_PATH = "GameCreator/Inventory/InventoryRPG";

		// PROPERTIES: ----------------------------------------------------------------------------

		[Space] public Image floatingItem;

        private CanvasScaler canvasScaler;
		private RectTransform floatingItemRT;
		private Animator inventoryAnimator;
		private GameObject inventoryRoot;
        private bool isOpen = false;

        // INITIALIZERS: --------------------------------------------------------------------------

        private void Awake()
		{
			InventoryUIManager.Instance = this;
			if (transform.childCount >= 1) 
			{
				this.inventoryRoot = transform.GetChild(0).gameObject;
				this.inventoryAnimator = this.inventoryRoot.GetComponent<Animator>();
			}

			if (this.floatingItem != null) this.floatingItemRT = this.floatingItem.GetComponent<RectTransform>();
			InventoryUIManager.OnDragItem(null, false);
		}

		// PUBLIC METHODS: ------------------------------------------------------------------------

		public void Open()
		{
			if (this.isOpen) return;

			this.ChangeState(true);
			if (DATABASE_INVENTORY.inventorySettings.pauseTimeOnUI)
			{
                TimeManager.Instance.SetTimeScale(0f, TIME_LAYER);
			}
		}

		public void Close()
		{
			if (!this.isOpen) return;

			if (DATABASE_INVENTORY.inventorySettings.pauseTimeOnUI)
			{
                TimeManager.Instance.SetTimeScale(1f, TIME_LAYER);
            }

			this.ChangeState(false);
		}

        public bool IsOpen()
        {
            return this.isOpen;
        }

		// STATIC METHODS: ------------------------------------------------------------------------

		public static void OpenInventory()
		{
			if (IsInventoryOpen()) return;

            InventoryUIManager.RequireInstance();
			InventoryUIManager.Instance.Open();
		}

		public static void CloseInventory()
		{
            if (!IsInventoryOpen()) return;

			InventoryUIManager.RequireInstance();
			InventoryUIManager.Instance.Close();
		}

        public static bool IsInventoryOpen()
        {
            if (InventoryUIManager.Instance == null) return false;
            return InventoryUIManager.Instance.IsOpen();
        }

		private static void RequireInstance()
		{
            if (DATABASE_INVENTORY == null) DATABASE_INVENTORY = DatabaseInventory.Load();
			if (InventoryUIManager.Instance == null)
			{
				EventSystemManager.Instance.Wakeup();
				if (DATABASE_INVENTORY.inventorySettings == null)
				{
                    Debug.LogError("No inventory database found");
					return;
                }

                GameObject prefab = DATABASE_INVENTORY.inventorySettings.inventoryUIPrefab;
                if (prefab == null) prefab = Resources.Load<GameObject>(DEFAULT_UI_PATH);

				Instantiate(prefab, Vector3.zero, Quaternion.identity);
			}
		}

		public static void OnDragItem(Sprite sprite, bool dragging)
		{
            if (!InventoryUIManager.Instance.floatingItem) return;

			InventoryUIManager.Instance.floatingItem.gameObject.SetActive(dragging);
			if (!dragging) return;

			InventoryUIManager.Instance.floatingItem.sprite = sprite;

            Vector2 position = InventoryUIManager.Instance.GetPointerPositionUnscaled(Input.mousePosition);
			InventoryUIManager.Instance.floatingItemRT.anchoredPosition = position;
		}

		// PRIVATE METHODS: -----------------------------------------------------------------------

		private void ChangeState(bool toOpen)
		{
			if (this.inventoryRoot == null) 
			{
				Debug.LogError("Unable to find inventoryRoot");
				return;
			}

			this.isOpen = toOpen;

			if (this.inventoryAnimator == null)
			{
				this.inventoryRoot.SetActive(toOpen);
				return;
			}

            this.inventoryAnimator.SetBool("State", toOpen);
			InventoryManager.Instance.eventInventoryUI.Invoke(toOpen);
		}

		private Vector2 GetPointerPositionUnscaled(Vector2 mousePosition)
		{
			if (this.canvasScaler == null) this.canvasScaler = transform.GetComponentInParent<CanvasScaler>();
			if (this.canvasScaler == null) return mousePosition;

			Vector2 referenceResolution = this.canvasScaler.referenceResolution;
			Vector2 currentResolution = new Vector2(Screen.width, Screen.height);

			float widthRatio = currentResolution.x / referenceResolution.x;
			float heightRatio = currentResolution.y / referenceResolution.y;
			float ratio = Mathf.Lerp(widthRatio, heightRatio, this.canvasScaler.matchWidthOrHeight);

			return mousePosition/ratio;
		}
	}
}