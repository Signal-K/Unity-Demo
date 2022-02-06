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

#if UNITY_EDITOR
    using UnityEditor;
#endif

    [AddComponentMenu("")]
	public class ActionToggleSysInfo : IAction
    {
	    public GameObject infoPanel;
	    public SysInfo infoSwitch;
	    public GameObject fpsPanel;
	    public GameObject hwPanel;

	

	    
	    public enum INFOTYPE
	    {
		    FPSAndMEM,
		    HWConfig
            
	    }
	    
	    
	    public INFOTYPE infoType = INFOTYPE.FPSAndMEM;


	
     

       

      

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	        fpsPanel = infoPanel.transform.GetChild (0).gameObject;
	        hwPanel = infoPanel.transform.GetChild (1).gameObject;
	        infoSwitch = infoPanel.GetComponentInChildren<SysInfo>();
	 


	    			 switch (infoType)
	    			  {
	    						case INFOTYPE.FPSAndMEM:
		    						infoSwitch.fpsinfo = true;
		    						hwPanel.SetActive(false);
		    						fpsPanel.SetActive(true);
		    					 break;
		    			    
	    					     case INFOTYPE.HWConfig:
		    					     infoSwitch.fpsinfo = false;
		    					     fpsPanel.SetActive(false);
		    					     hwPanel.SetActive(true);
		    				     break;
	

	        }
            return true;

        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {



            return base.Execute(target, actions, index);

        }

        

       

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	    public static new string NAME = "UI/SysInfo/Toggle SysInfo";
	    private const string NODE_TITLE = "Toggle SysInfo";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

	    private SerializedProperty spinfopanel;
	    private SerializedProperty spinfotype;
        

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
            {
                return string.Format(
                     NODE_TITLE
                   //  (targetObject == null ? "none" : targetObject.name)
                 );
            }

        protected override void OnEnableEditorChild()
            {
	            this.spinfopanel = this.serializedObject.FindProperty("infoPanel");
	            this.spinfotype = this.serializedObject.FindProperty("infoType");
             
        }


        protected override void OnDisableEditorChild()
            {
            this.spinfopanel = null;
	        this.spinfotype = null;

        }

        public override void OnInspectorGUI()
            {
                this.serializedObject.Update();
	            EditorGUILayout.PropertyField(this.spinfopanel, new GUIContent("Panel - SysInfo"));
            EditorGUILayout.Space();
	            EditorGUILayout.PropertyField(this.spinfotype, new GUIContent("SysInfo Type"));
         
            this.serializedObject.ApplyModifiedProperties();
            }

#endif

        }
    }
