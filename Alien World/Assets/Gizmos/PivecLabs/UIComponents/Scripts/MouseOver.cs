namespace GameCreator.UIComponents
{
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;


	public class MouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		
		public bool overObject = false;
		public bool outlineObject = false;
		
	public void OnPointerEnter(PointerEventData eventData)
		{
			overObject = true;
			if (outlineObject == true)
			{
				GetComponent<Outline>().enabled = true;

			}
		}
	public void OnPointerExit(PointerEventData eventData)
		{
			overObject = false;
			GetComponent<Outline>().enabled = false;
		
		}
	}
}