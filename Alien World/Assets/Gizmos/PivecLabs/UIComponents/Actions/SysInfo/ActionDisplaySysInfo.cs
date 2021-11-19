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
	public class ActionDisplaySysInfo : IAction
    {
	    public GameObject infoPanel;
	    public SysInfo infoSwitch;
	    public GameObject fpsPanel;
	    public GameObject hwPanel;

	    public enum INFOPOSITION
        {
            TopRight,
            TopLeft,
            BottomLeft,
            BottomRight
            
        }


	    public INFOPOSITION infoPosition = INFOPOSITION.TopLeft;
	    
	    public enum INFOTYPE
	    {
		    FPSAndMEM,
		    HWConfig
            
	    }
	    
	    
	    public INFOTYPE infoType = INFOTYPE.FPSAndMEM;


	    private float swidth;
	    private float sheight;
	    private float soffsetx;
	    private float soffsety;
	    private RectTransform s_RectTransform;

     

       

      

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	        fpsPanel = infoPanel.transform.GetChild (0).gameObject;
	        hwPanel = infoPanel.transform.GetChild (1).gameObject;
	        infoSwitch = infoPanel.GetComponentInChildren<SysInfo>();
	        s_RectTransform = infoPanel.GetComponent<RectTransform>();
	        s_RectTransform.localScale += new Vector3(0, 0, 0);
	        swidth = s_RectTransform.rect.width;
	        sheight = s_RectTransform.rect.height;
	        soffsetx = s_RectTransform.position.x;
	        soffsety = s_RectTransform.position.y;

           
	        switch (infoPosition)
                        {
                            case INFOPOSITION.BottomLeft:
	                            s_RectTransform.anchoredPosition = new Vector3(soffsetx, soffsety);
                                break;
                            case INFOPOSITION.TopLeft:
	                            s_RectTransform.anchoredPosition = new Vector3(soffsetx, Screen.height - (sheight + soffsety));
                                break;
                            case INFOPOSITION.TopRight:
	                            s_RectTransform.anchoredPosition = new Vector3(Screen.width - (swidth + soffsetx), Screen.height - (sheight + soffsety));
                                break;
                            case INFOPOSITION.BottomRight:
	                            s_RectTransform.anchoredPosition = new Vector3(Screen.width - (swidth + soffsetx), soffsety);
                                break;

                        }


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

	    public static new string NAME = "UI/SysInfo/Display SysInfo";
	    private const string NODE_TITLE = "Display SysInfo";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

	    private SerializedProperty spinfoposition;
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
	            this.spinfoposition = this.serializedObject.FindProperty("infoPosition");
	            this.spinfopanel = this.serializedObject.FindProperty("infoPanel");
	            this.spinfotype = this.serializedObject.FindProperty("infoType");
             
        }


        protected override void OnDisableEditorChild()
            {
            this.spinfoposition = null;
            this.spinfopanel = null;
	        this.spinfotype = null;

        }

        public override void OnInspectorGUI()
            {
                this.serializedObject.Update();
	            EditorGUILayout.PropertyField(this.spinfopanel, new GUIContent("Panel - SysInfo"));
            EditorGUILayout.Space();
	            EditorGUILayout.PropertyField(this.spinfoposition, new GUIContent("SysInfo Position"));
            EditorGUILayout.Space();
	            EditorGUILayout.PropertyField(this.spinfotype, new GUIContent("SysInfo Type"));
         
            this.serializedObject.ApplyModifiedProperties();
            }

#endif

        }
    }
