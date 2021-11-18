namespace GameCreator.Inventory
{
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;

	public class DraggablePanel : MonoBehaviour
	{
		// PROPERTIES: ----------------------------------------------------------------------------

		public RectTransform targetPanel;
		public bool constraintWithinScreen = true;

		private CanvasScaler canvasScaler;

		// INITIALIZERS: --------------------------------------------------------------------------

		public void Awake()
		{
			this.SetupEvents(EventTriggerType.BeginDrag, this.OnDragBegin);
			this.SetupEvents(EventTriggerType.EndDrag, this.OnDragEnd);
			this.SetupEvents(EventTriggerType.Drag, this.OnDragMove);
		}

		// CALLBACK METHODS: ----------------------------------------------------------------------

		private void OnDragBegin(BaseEventData eventData)
		{
			this.MovePanel(eventData);
		}

		private void OnDragEnd(BaseEventData eventData)
		{
			this.MovePanel(eventData);
		}

		private void OnDragMove(BaseEventData eventData)
		{
			this.MovePanel(eventData);
		}

		private void MovePanel(BaseEventData eventData)
		{
			PointerEventData pointerData = (PointerEventData)eventData;
			if (pointerData == null) return;

			Vector2 currentPosition = this.targetPanel.anchoredPosition;
			Vector2 deltaUnscaled = this.GetPonterDataDeltaUnscaled(pointerData.delta);

			currentPosition.x += deltaUnscaled.x;
			currentPosition.y += deltaUnscaled.y;

			if (this.constraintWithinScreen)
			{
				Vector3 pos = this.targetPanel.localPosition;
				RectTransform parentPanel = (RectTransform)this.targetPanel.parent;

				Vector3 minPosition = parentPanel.rect.min - this.targetPanel.rect.min;
				Vector3 maxPosition = parentPanel.rect.max - this.targetPanel.rect.max;

				currentPosition = new Vector2(
					Mathf.Clamp(currentPosition.x, minPosition.x, maxPosition.x),
					Mathf.Clamp(currentPosition.y, minPosition.y, maxPosition.y)
				);
			}

			targetPanel.anchoredPosition = currentPosition;
		}

		private Vector2 GetPonterDataDeltaUnscaled(Vector2 delta)
		{
			if (this.canvasScaler == null) this.canvasScaler = transform.GetComponentInParent<CanvasScaler>();
			if (this.canvasScaler == null) return delta;

			Vector2 referenceResolution = this.canvasScaler.referenceResolution;
			Vector2 currentResolution = new Vector2(Screen.width, Screen.height);

			float widthRatio = currentResolution.x / referenceResolution.x;
			float heightRatio = currentResolution.y / referenceResolution.y;
			float ratio = Mathf.Lerp(widthRatio, heightRatio, this.canvasScaler.matchWidthOrHeight);

			return delta/ratio;
		}

		// PRIVATE METHODS: -----------------------------------------------------------------------

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