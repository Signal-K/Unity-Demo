namespace GameCreator.UIComponents
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Events;

	using GameCreator.Core;
	using GameCreator.Variables;
	using TMPro;

#if UNITY_EDITOR
	using UnityEditor;
	#endif

	public class DropDownSavesTMP : MonoBehaviour
	{
		public TMP_Dropdown savesDropdown;
		public TextMeshProUGUI dropdownLabel;
		public string targetVariable;
		
		List<string> slots = new List<string> ();


		void Start()
		{
    	
			int count = SaveLoadManager.Instance.GetSavesCount();
			
			Debug.Log("get saves count returns" + count);
			
	    savesDropdown.ClearOptions();

	    var dictionary = SaveLoadManager.Instance.savesData.profiles;
	    
	       foreach (KeyValuePair<int, SavesData.Profile> item in dictionary)
	    {
		       Debug.Log("Save profile ID: " + item.Key);
		       Debug.Log("Save profile date: " + item.Value.date);
		       
		       
		           
		       slots.Add (item.Value.date.ToString());
		     
	    }
	    
	    slots.Reverse();
	    savesDropdown.AddOptions(slots);

	 
    }
    
		void Update(){
		}


		public void dropdown_indexchanged()
		{
			float slot = (float)savesDropdown.value;
			VariablesManager.SetGlobal(targetVariable, slot);

		}
   
	}
}