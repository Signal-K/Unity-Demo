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
	public class ActionDisplayCompass : IAction
    {
	    public GameObject Compasscanvas;

	    
        private PlayerCharacter player;
     
	    private RawImage CompassImage;
	    private Transform playerTransform;
	    private Text CompassDirectionText;

	    public bool setCompassActive;
	    private bool CompassActive = false;

	    
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

	        
	        if (Compasscanvas != null)
	        {
	        	if (setCompassActive == true)
	        	{
		        	Compasscanvas.SetActive(true);

		        	player = HookPlayer.Instance.Get<PlayerCharacter>();
            
		        	playerTransform = player.transform;
	        
		        	CompassImage = Compasscanvas.GetComponentInChildren<RawImage>();
		        	CompassDirectionText = Compasscanvas.GetComponentInChildren<Text>();
	        
		        	CompassActive = true;
	      	
	        	}
	        	else 
	        	{
		        	CompassActive = false;
		        	Compasscanvas.SetActive(false);

	        		
	        	}
	        	
	        }
	      
		      
	        
            return true;

        }

	    void Update () {
	    	
	    	if (CompassActive)
	    	{

		    	Vector3 forward = playerTransform.transform.forward;
		    	forward.y = 0;
	    		CompassImage.uvRect = new Rect(playerTransform.localEulerAngles.y / 360, 0, 1, 1);
		    	float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
		    	headingAngle = 5 * (Mathf.RoundToInt(headingAngle / 5.0f));
		    	int displayangle;
		    	displayangle = Mathf.RoundToInt(headingAngle);

		    	switch (displayangle)
		    	{
		    	case 0:			
			    	CompassDirectionText.text = "N";
			    	break;
		    	case 360:
			    	CompassDirectionText.text = "N";
			    	break;
		    	case 45:
			    	CompassDirectionText.text = "NE";
			    	break;
		    	case 90:
			    	CompassDirectionText.text = "E";
			    	break;
		    	case 130:
			    	CompassDirectionText.text = "SE";
			    	break;
		    	case 180:
			    	CompassDirectionText.text = "S";
			    	break;
		    	case 225:
			    	CompassDirectionText.text = "SW";
			    	break;
		    	case 270:
			    	CompassDirectionText.text = "W";
			    	break;
		    	default:
			    	CompassDirectionText.text = headingAngle.ToString ();
			    	break;
		    	}
	    		
	    	}
		   	
     }
	  
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	    public static new string NAME = "UI/MiniMap/Display Compass";
	    private const string NODE_TITLE = "Display Compass";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

	    private SerializedProperty spcompasscanvas;
	    private SerializedProperty spcompassactive;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
            {
                return string.Format(
                     NODE_TITLE
                 );
            }

        protected override void OnEnableEditorChild()
            {
	            this.spcompasscanvas = this.serializedObject.FindProperty("Compasscanvas");
	            this.spcompassactive = this.serializedObject.FindProperty("setCompassActive");

        }


        protected override void OnDisableEditorChild()
            {
 	            this.spcompasscanvas = null;
	            this.spcompassactive = null;


        }

        public override void OnInspectorGUI()
            {
                this.serializedObject.Update();
	            EditorGUILayout.PropertyField(this.spcompasscanvas, new GUIContent("Compass Canvas"));
	            EditorGUILayout.PropertyField(this.spcompassactive, new GUIContent("Set Compass Active"));


            this.serializedObject.ApplyModifiedProperties();
            }
	    
	   

	 
#endif

        }
    }
