namespace GameCreator.UIComponents
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UI;
	using GameCreator.Core;
	using GameCreator.Variables;
  

#if UNITY_EDITOR
	using UnityEditor;
#endif

	[AddComponentMenu("Game Creator/UI/UI Element Manager", 100)]

	public class UIElementManager : MonoBehaviour
	{
			
		public enum Keys1
		{
			Tab,
			[InspectorName("Tab + Left Shift")]
			LeftShift,
			[InspectorName("Tab + Right Shift")]
			RightShift,
			[InspectorName("Tab + Left Ctrl")]
			LeftCtrl,
			[InspectorName("Tab + Right Ctrl")]
			RightCtrl

		}
		[SerializeField]
		public Keys1 keys1 = Keys1.Tab;
		
		public enum Keys2
		{
			Tab,
			[InspectorName("Tab + Left Shift")]
			LeftShift,
			[InspectorName("Tab + Right Shift")]
			RightShift,
			[InspectorName("Tab + Left Ctrl")]
			LeftCtrl,
			[InspectorName("Tab + Right Ctrl")]
			RightCtrl
		}
		[SerializeField]
		public Keys2 keys2 = Keys2.LeftShift;


		public enum Keys3
		{
			Tab,
			[InspectorName("Tab + Left Shift")]
			LeftShift,
			[InspectorName("Tab + Right Shift")]
			RightShift,
			[InspectorName("Tab + Left Ctrl")]
			LeftCtrl,
			[InspectorName("Tab + Right Ctrl")]
			RightCtrl
		}
		[SerializeField]
		public Keys3 keys3 = Keys3.RightShift;


		public enum Keys4
		{
			Tab,
			[InspectorName("Tab + Left Shift")]
			LeftShift,
			[InspectorName("Tab + Right Shift")]
			RightShift,
			[InspectorName("Tab + Left Ctrl")]
			LeftCtrl,
			[InspectorName("Tab + Right Ctrl")]
			RightCtrl
		
		}
		[SerializeField]
		public Keys4 keys4 = Keys4.LeftCtrl;
		
		[SerializeField]
		public Canvas uicanvas;
		
		private RawImage[] uirawimages;
		private ButtonActions[] uibuttons;
		private InputFieldVariable[] uiinputfield;
		private SliderVariable[] uisliderfield;
		private int n1;
		private int n2;
		private int n3;
		private int n4;
		private string keyscombo1;
		private string keyscombo2;
		private string keyscombo3;
		private string keyscombo4;
		private string keyscombo5;
		private float canvasVisible;
		
    	void Start()
		{
    		
			
	    	this.uibuttons = this.uicanvas.GetComponentsInChildren<ButtonActions>();
	    	n1=-1;
	    	this.uirawimages = this.uicanvas.GetComponentsInChildren<RawImage>();
	    	n2=-1;
	    	this.uiinputfield = this.uicanvas.GetComponentsInChildren<InputFieldVariable>();
	    	n3=-1;
	    	this.uisliderfield = this.uicanvas.GetComponentsInChildren<SliderVariable>();
	    	n4=-1;
	    	
	    	switch (this.keys1)
	    	{
	    	case Keys1.Tab:
		    	keyscombo1 = "Buttons";
		    	break;
	    	case Keys1.LeftShift:
		    	keyscombo2 = "Buttons";
		    	break;
	    	case Keys1.RightShift:
		    	keyscombo3 = "Buttons";
		    	break;
	    	case Keys1.LeftCtrl:
		    	keyscombo4 = "Buttons";
		    	break;
	    	case Keys1.RightCtrl:
		    	keyscombo5 = "Buttons";
		    	break;
	    	
	    	}
	    	switch (this.keys2)
	    	{
	    	case Keys2.Tab:
		    	keyscombo1 = "InputField";
		    	break;
	    	case Keys2.LeftShift:
		    	keyscombo2 = "InputField";
		    	break;
	    	case Keys2.RightShift:
		    	keyscombo3 = "InputField";
		    	break;
	    	case Keys2.LeftCtrl:
		    	keyscombo4 = "InputField";
		    	break;
	    	case Keys2.RightCtrl:
		    	keyscombo5 = "InputField";
		    	break;
	    	
	    	}
	    	switch (this.keys3)
	    	{
	    	case Keys3.Tab:
		    	keyscombo1 = "Slider";
		    	break;
	    	case Keys3.LeftShift:
		    	keyscombo2 = "Slider";
		    	break;
	    	case Keys3.RightShift:
		    	keyscombo3 = "Slider";
		    	break;
	    	case Keys3.LeftCtrl:
		    	keyscombo4 = "Slider";
		    	break;
	    	case Keys3.RightCtrl:
		    	keyscombo5 = "Slider";
		    	break;
	    	
	    	}
	    	switch (this.keys4)
	    	{
	    	case Keys4.Tab:
		    	keyscombo1 = "RawImage";
		    	break;
	    	case Keys4.LeftShift:
		    	keyscombo2 = "RawImage";
		    	break;
	    	case Keys4.RightShift:
		    	keyscombo3 = "RawImage";
		    	break;
	    	case Keys4.LeftCtrl:
		    	keyscombo4 = "RawImage";
		    	break;
	    	case Keys4.RightCtrl:
		    	keyscombo5 = "RawImage";
		    	break;
	    	
	    	}

    	}

    	void Update()
		{

			if (this.uicanvas.GetComponent<CanvasGroup>().interactable == true)
			{
				if (!(Input.GetKey(KeyCode.LeftShift)) && !(Input.GetKey(KeyCode.RightShift)) && !(Input.GetKey(KeyCode.LeftControl)) 
					&& !(Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Tab))
			
				{
					switch (this.keyscombo1)
					{
					case "Buttons":
						this.buttoncode();
						break;
					case "InputField":
						inputcode();
						break;
					case "Slider":
						slidercode();
						break;
					case "RawImage":
						rawimagecode();
						break;
	    	
					}
				
				}
			
				if ((Input.GetKey(KeyCode.LeftShift)) && !(Input.GetKey(KeyCode.RightShift)) && !(Input.GetKey(KeyCode.LeftControl) 
					&& !(Input.GetKey(KeyCode.RightControl))) && Input.GetKeyDown(KeyCode.Tab))
				{
					switch (this.keyscombo2)
					{
					case "Buttons":
						buttoncode();
						break;
					case "InputField":
						inputcode();
						break;
					case "Slider":
						slidercode();
						break;
					case "RawImage":
						rawimagecode();
						break;
	    	
					}
				
				}
			
				if (!(Input.GetKey(KeyCode.LeftShift)) && (Input.GetKey(KeyCode.RightShift)) && !(Input.GetKey(KeyCode.LeftControl) 
					&& !(Input.GetKey(KeyCode.RightControl))) && Input.GetKeyDown(KeyCode.Tab))
			
				{
					switch (this.keyscombo3)
					{
					case "Buttons":
						buttoncode();
						break;
					case "InputField":
						inputcode();
						break;
					case "Slider":
						slidercode();
						break;
					case "RawImage":
						rawimagecode();
						break;
	    	
					}
				
				}
			
				if (!(Input.GetKey(KeyCode.LeftShift)) && !(Input.GetKey(KeyCode.RightShift)) && (Input.GetKey(KeyCode.LeftControl) 
					&& !(Input.GetKey(KeyCode.RightControl))) && Input.GetKeyDown(KeyCode.Tab))
			
				{
					switch (this.keyscombo4)
					{
					case "Buttons":
						buttoncode();
						break;
					case "InputField":
						inputcode();
						break;
					case "Slider":
						slidercode();
						break;
					case "RawImage":
						rawimagecode();
						break;
	    	
					}
				
				}
			
				if (!(Input.GetKey(KeyCode.LeftShift)) && !(Input.GetKey(KeyCode.RightShift)) && !(Input.GetKey(KeyCode.LeftControl) 
					&& (Input.GetKey(KeyCode.RightControl))) && Input.GetKeyDown(KeyCode.Tab))
			
				{
					switch (this.keyscombo5)
					{
					case "Buttons":
						buttoncode();
						break;
					case "InputField":
						inputcode();
						break;
					case "Slider":
						slidercode();
						break;
					case "RawImage":
						rawimagecode();
						break;
	    	
					}
				
				}
			
			
				if (Input.GetKeyDown(KeyCode.Return)) {
				
					this.uibuttons[n2].onClick.Invoke(gameObject);
				
				}
				
			}
		
		}
		
		private void buttoncode()
		{
			if (this.uibuttons.Length > 0) 
			{
				n2++;
				if (n2 == this.uibuttons.Length) 
				{
					n2=0;
				}
				this.uibuttons[n2].Select();
				
			}
		
			
		}
		private void inputcode()
		{
			
			if (this.uiinputfield.Length > 0) 
			{
				n3++;
				if (n3 == this.uiinputfield.Length) 
				{
					n3=0;
				}
				this.uiinputfield[n3].Select();
				
			}
		
			
		}
		private void slidercode()
		{
			if (this.uisliderfield.Length > 0) 
			{
				n4++;
				if (n4 == this.uisliderfield.Length) 
				{
					n4=0;
				}
				this.uisliderfield[n4].Select();
				
			}
		
			
		}
		private void rawimagecode()
		{
			if (this.uirawimages.Length > 0) 
			{
				if (n1 > -1)
					this.uirawimages[n1].GetComponent<Outline>().enabled = false;

				n1++;
				if (n1 == this.uirawimages.Length) 
				{
					n1=0;
				}
				this.uirawimages[n1].GetComponent<Outline>().enabled = true;
				
			}
			
			
		}
	}

}