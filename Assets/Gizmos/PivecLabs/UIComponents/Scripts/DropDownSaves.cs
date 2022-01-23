namespace GameCreator.UIComponents
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Events;

	using GameCreator.Core;
	using GameCreator.Variables;

#if UNITY_EDITOR
	using UnityEditor;
	#endif

	public class DropDownSaves : MonoBehaviour
	{
		public Dropdown savesDropdown;
		public Text dropdownLabel;
		public string loadSlotVariable;
		public string saveSlotVariable;
		private int count;
		
		List<string> slots = new List<string> ();


		void Start()
		{
    	

			
			count = SaveLoadManager.Instance.GetSavesCount();
			VariablesManager.SetGlobal(saveSlotVariable, count);

			
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

			VariablesManager.SetGlobal(loadSlotVariable, count);

    }
    
		void Update(){
		}


		public void dropdown_indexchanged()
		{
			float slot = (float)savesDropdown.value;
			float num =  count -(slot);
			VariablesManager.SetGlobal(loadSlotVariable, num);

		}
   
	}
}