namespace GameCreator.UIComponents
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UI;
	using UnityEngine.Video;
	using GameCreator.Core;
	using GameCreator.Variables;
	using GameCreator.Core.Hooks;
	using GameCreator.Characters;
	using System.Linq;

public class MapManager : MonoBehaviour
	{
		public GameObject minimapPanel;

		public bool miniMapshowing;
		public bool fullMapshowing;
		public bool miniMapscrollWheel;
		public float miniMapscrollWheelSpeed = 5.0f;
		public bool miniMapmouseDrag;
		public int miniMapDragSpeed = 1;
		public int miniMapDragButton = 0;

		private float mmwidth;
		private float mmoffsetx;
		private float mmoffsety;
		private RectTransform m_RectTransform;
		private RectTransform parentCanvas;
		
		[HideInInspector]
		public enum MINIMAPPOSITION
		{
			TopRight,
			TopLeft,
			BottomLeft,
			BottomRight
            
		}

		[HideInInspector]
		public MINIMAPPOSITION minimapPosition = MINIMAPPOSITION.TopRight;

		public GameObject go;
		private List<GameObject> goList = new List<GameObject>();
		private bool updatingList;

		private Vector2 lastScreenSize;
		void Start()
		{
			lastScreenSize = new Vector2(Screen.width,Screen.height);
			parentCanvas = minimapPanel.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

			m_RectTransform = minimapPanel.GetComponent<RectTransform>();
			m_RectTransform.localScale += new Vector3(0, 0, 0);
			mmwidth = m_RectTransform.rect.width;
			mmoffsetx = 10;
			mmoffsety = 10;
			switchCams();
		}
		
		public void switchCams()
		{
			updatingList = true;
			go = GameObject.Find ("MiniMapCamera");
			goList = new List<GameObject>();

			foreach(GameObject gameObj in Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "MapMarkerLabel"))
			{
			
				goList.Add (gameObj);
			
			}
			updatingList = false;
	
		}
		
		
		void FixedUpdate()
		{
       
			if ((fullMapshowing == true) || (miniMapshowing == true) && (updatingList == false)) 
			{
				
				if (goList != null)
				{
					go = GameObject.Find ("MiniMapCamera");

					foreach (var label in goList)
					{
						label.GetComponent<Transform>().eulerAngles = new Vector3(label.transform.eulerAngles.x,go.transform.eulerAngles.y,label.transform.eulerAngles.z);
					}
	    	
				}
				
			}	    
			
			if ((fullMapshowing == true) && (miniMapscrollWheel == true))
				{
				go = GameObject.Find ("MiniMapCamera");

				go.GetComponent<Camera>().orthographicSize += Input.GetAxis("Mouse ScrollWheel") * miniMapscrollWheelSpeed;
				}
				
			if ((fullMapshowing == true) && (miniMapmouseDrag == true))
			{
		
			 if (Input.GetMouseButton(miniMapDragButton))
			 {
				 go = GameObject.Find ("MiniMapCamera");

				 go.GetComponent<Camera>().transform.position -= new Vector3(Input.GetAxis("Mouse X") * miniMapDragSpeed, 0, Input.GetAxis("Mouse Y") * miniMapDragSpeed);
			 }
			}
			
			
			Vector2 screenSize = new Vector2(Screen.width, Screen.height); 
 
			if (this.lastScreenSize != screenSize)
			{
				lastScreenSize = new Vector2(Screen.width,Screen.height);
    
				switch (minimapPosition)
				{
				case MINIMAPPOSITION.BottomLeft:
					m_RectTransform.anchoredPosition = new Vector3(mmoffsetx, mmoffsety);
					break;
				case MINIMAPPOSITION.TopLeft:
					m_RectTransform.anchoredPosition = new Vector3(mmoffsetx, parentCanvas.rect.height - (mmwidth + mmoffsety));
					break;
				case MINIMAPPOSITION.TopRight:
					m_RectTransform.anchoredPosition = new Vector3(parentCanvas.rect.width - (mmwidth + mmoffsetx), parentCanvas.rect.height - (mmwidth + mmoffsety));
					break;
				case MINIMAPPOSITION.BottomRight:
					m_RectTransform.anchoredPosition = new Vector3(parentCanvas.rect.width - (mmwidth + mmoffsetx), mmoffsety);
					break;

				}
			
			}
			
			
			
			}
		
	
		}
		
}	