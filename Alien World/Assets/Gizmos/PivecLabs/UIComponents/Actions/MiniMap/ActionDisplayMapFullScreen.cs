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
    public class ActionDisplayMapFullScreen : IAction
    {
	    public GameObject mapManager;
	    public GameObject mapPanel;
        public GameObject rawImage;
	    public GameObject mapcanvas;
	    [Range(1,1500)]
	    public float cameraSize = 20f;
	    [Range(1,100)]
	    public float cameraDistance = 30f;
	    [Range(1,60)]
	    public float planedistance = 10f;
 
        private PlayerCharacter player;
     
   
        private float mmwidth;
        private float mmoffsetx;
        private float mmoffsety;
        private RectTransform m_RectTransform;
	    private Image mask;
     

        RenderTexture renderTexture;
        RectTransform rt;
        RawImage img;
        private Camera targetCamera;
    
	    public LayerMask cullingMask = ~0;
	    public bool occlusionCulling = true;


	    public MapManager fullscreen;
	    
	    public bool mapMarkers;
	    public bool showMarkers;
	    [Range(0,5)]
	    public float markerSize = 0.2f;
 

	    public bool overlay;
	    public bool lockMap;
	    public bool centerMap;
	    public GameObject mapCenter;

	    public bool overlayMap;
	    public bool zoomMap;
	    public bool dragMap;
	    [Range(0,2)]
	    public int dragbutton = 0;
	    [Range(1,10)]
	    public int dragspeed = 1;

	    [Range(1f, 20f)]
	    public float zoomSensitivity = 5.0f;


        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

	        GameObject go = GameObject.Find ("MiniMapCamera");
	      
	        if (go){
	        	
		        img = null;
		        targetCamera = null;
		        renderTexture = null;
		        Destroy (go.gameObject);

	        }
	        
	        player = HookPlayer.Instance.Get<PlayerCharacter>();

	        fullscreen = mapManager.GetComponent<MapManager>();
	        fullscreen.miniMapshowing = false;
	        fullscreen.fullMapshowing = true;
	        fullscreen.miniMapscrollWheel = zoomMap;
	        fullscreen.miniMapscrollWheelSpeed = zoomSensitivity;
	        fullscreen.miniMapmouseDrag = dragMap;
	        fullscreen.miniMapDragButton = dragbutton;
	        fullscreen.miniMapDragSpeed = dragspeed;
	        fullscreen.switchCams();

	        
	        if (renderTexture != null)
	        {
		        renderTexture.Release();
		        
	       
                rt = (RectTransform)rawImage.transform;
	            renderTexture = new RenderTexture((int)Screen.width, (int)Screen.height, 32);
	            renderTexture.Create();
	        }
           

            if (img == null)
            {

                img = rawImage.gameObject.GetComponent<RawImage>();
                img.texture = renderTexture;

            }
            
	        m_RectTransform = mapPanel.GetComponent<RectTransform>();
	        m_RectTransform.localScale += new Vector3(0, 0, 0);
	        mmwidth = m_RectTransform.rect.width;
	       
	        if (mapPanel != null)
		        mapPanel.SetActive(false);
	        
	      
	        
         if (targetCamera == null)
         {
            
	         GameObject cameraMinimap = new GameObject();
	         if (lockMap == false)
	         {
	         	cameraMinimap.transform.parent = player.transform;
	         }
	         targetCamera = cameraMinimap.AddComponent<Camera>();
	         targetCamera.enabled = true;
	         targetCamera.allowHDR = false;
	         targetCamera.cullingMask = this.cullingMask;
	         targetCamera.useOcclusionCulling = occlusionCulling;
	         targetCamera.allowHDR = false;
	         targetCamera.allowMSAA = false;
	         targetCamera.allowDynamicResolution = false;
	         targetCamera.targetTexture = renderTexture;
	         targetCamera.orthographic = true;
	         targetCamera.orthographicSize = cameraSize;
	         targetCamera.name = "MiniMapCamera";
	         targetCamera.transform.LookAt(player.transform);
	         targetCamera.transform.localRotation = Quaternion.Euler(90.0f,0.0f,0.0f);
	         targetCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + cameraDistance, player.transform.position.z);
	         if (centerMap == true)
	         {
	         	if (mapCenter != null)
		         targetCamera.transform.position = new Vector3(mapCenter.transform.position.x, mapCenter.transform.position.y + cameraDistance, mapCenter.transform.position.z);
	         }


         }
	        if (mapMarkers == true)
	        {
		        foreach(GameObject gameObj in Resources.FindObjectsOfTypeAll<GameObject>())
		        {
			        if(gameObj.name == "MapMarkerImage")
			        {
				        gameObj.transform.localScale = new Vector3(markerSize, markerSize, markerSize);
				        gameObj.SetActive(showMarkers);
	
			        }
		        }
	        	
	        }
         
	       
	       
         
         
	        if ((overlay == true) && (mapcanvas != null))
	        {
	        	
	      
	    	  if(overlayMap == true)
	    	 { 
	        	mapcanvas.SetActive(true);
	        	Canvas canvas = mapcanvas.gameObject.GetComponent<Canvas>();
		        canvas.renderMode = RenderMode.ScreenSpaceCamera;
		    	  canvas.worldCamera = targetCamera;
		    	  canvas.planeDistance = planedistance;
	    	  }
	    	  else 
	    	  {
		        mapcanvas.SetActive(false);

	    	  }

	        }
            return true;

        }

 
        

       

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	    public static new string NAME = "UI/MiniMap/Display Map Full Screen";
	    private const string NODE_TITLE = "Display Map Full Screen";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

	    private SerializedProperty spmapmanager;
        private SerializedProperty spmappanel;
        private SerializedProperty sprawimage;
	    private SerializedProperty spmapcanvas;
	    private SerializedProperty spcamerasize;
	    private SerializedProperty spcameradistance;
	    private SerializedProperty splockmap;
	    private SerializedProperty spcentermap;
	    private SerializedProperty spoverlay;
	    private SerializedProperty spoverlaymap;
	    private SerializedProperty spzoommap;
	    private SerializedProperty spzoommapsensitivity;
	    private SerializedProperty spdragmap;
	    private SerializedProperty spdragbutton;
	    private SerializedProperty spdragspeed;
	    private SerializedProperty spplanedistance;
	    private SerializedProperty spmapmarkers;
	    private SerializedProperty spshowmarkers;
	    private SerializedProperty spmarkersize;
	    private SerializedProperty spmapcenter;
	    private SerializedProperty spcullingMask;
	    private SerializedProperty spocclusionCulling;


        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
            {
                return string.Format(
                     NODE_TITLE
                 );
            }

        protected override void OnEnableEditorChild()
            {
	            this.spmapmanager = this.serializedObject.FindProperty("mapManager");
	            this.spmappanel = this.serializedObject.FindProperty("mapPanel");
            	this.sprawimage = this.serializedObject.FindProperty("rawImage");
            	this.spmapcanvas = this.serializedObject.FindProperty("mapcanvas");
	            this.spcamerasize = this.serializedObject.FindProperty("cameraSize");
	            this.spcameradistance = this.serializedObject.FindProperty("cameraDistance");
	            this.splockmap = this.serializedObject.FindProperty("lockMap");
	            this.spcentermap = this.serializedObject.FindProperty("centerMap");
	            this.spoverlay = this.serializedObject.FindProperty("overlay");
	            this.spoverlaymap = this.serializedObject.FindProperty("overlayMap");
	            this.spzoommap = this.serializedObject.FindProperty("zoomMap");
	            this.spzoommapsensitivity = this.serializedObject.FindProperty("zoomSensitivity");
	            this.spdragmap = this.serializedObject.FindProperty("dragMap");
	            this.spdragbutton = this.serializedObject.FindProperty("dragbutton");
	            this.spdragspeed = this.serializedObject.FindProperty("dragspeed");
	            this.spplanedistance = this.serializedObject.FindProperty("planedistance");
	            this.spmapmarkers = this.serializedObject.FindProperty("mapMarkers");
	            this.spshowmarkers = this.serializedObject.FindProperty("showMarkers");
	            this.spmarkersize = this.serializedObject.FindProperty("markerSize");
	            this.spmapcenter = this.serializedObject.FindProperty("mapCenter");    
	            this.spcullingMask = this.serializedObject.FindProperty("cullingMask");
	            this.spocclusionCulling = this.serializedObject.FindProperty("occlusionCulling");
            }


        protected override void OnDisableEditorChild()
            {
            	this.spmapmanager = null;
	            this.spmappanel = null;
            	this.sprawimage = null;
	            this.spmapcanvas = null;
	            this.spcamerasize = null;
	            this.spcameradistance = null;
	            this.splockmap = null;
	            this.spcentermap = null;
	            this.spoverlay = null;
	            this.spoverlaymap = null;
	            this.spzoommap = null;
	            this.spzoommapsensitivity = null;
	            this.spdragmap = null;
	            this.spdragbutton = null;
	            this.spdragspeed = null;
	            this.spplanedistance = null;
	            this.spshowmarkers = null;
	            this.spmapmarkers = null;
	            this.spmarkersize = null;
	            this.spmapcenter = null;
	            this.spcullingMask = null;
	            this.spocclusionCulling = null;


        }

        public override void OnInspectorGUI()
            {
                this.serializedObject.Update();
	            EditorGUILayout.PropertyField(this.spmapmanager, new GUIContent("Map Manager"));
	            EditorGUILayout.PropertyField(this.spmappanel, new GUIContent("Panel - MiniMap"));
        		EditorGUILayout.PropertyField(this.sprawimage, new GUIContent("RawImage texturemap"));
	            EditorGUILayout.Space();
	            EditorGUILayout.PropertyField(this.spoverlay, new GUIContent("Use Overlay Canvas"));
	            if (overlay)
	            {
		            EditorGUI.indentLevel++;
		            EditorGUILayout.PropertyField(this.spmapcanvas, new GUIContent("Overlay Canvas"));
		            EditorGUILayout.PropertyField(this.spplanedistance, new GUIContent("Plane Distance"));
		            EditorGUI.indentLevel--;
	            }
	            EditorGUILayout.Space();
	            EditorGUILayout.PropertyField(this.spmapmarkers, new GUIContent("Update Map Markers"));
	            if (mapMarkers)
	            {
		            EditorGUI.indentLevel++;
		            EditorGUILayout.PropertyField(this.spshowmarkers, new GUIContent("Show/Hide Markers"));
		            EditorGUILayout.PropertyField(this.spmarkersize, new GUIContent("Resize Markers"));

		            EditorGUI.indentLevel--;
	            }
	            EditorGUILayout.Space();
	            EditorGUILayout.Space();
	            EditorGUILayout.LabelField("Properties");
	            EditorGUI.indentLevel++;
	            EditorGUILayout.PropertyField(this.spcullingMask, new GUIContent("Culling Mask"));
	            EditorGUILayout.PropertyField(this.spocclusionCulling, new GUIContent("Occlusion Culling"));
	            EditorGUILayout.Space();

	            EditorGUILayout.PropertyField(this.spcamerasize, new GUIContent("Field of View"));
	            EditorGUILayout.PropertyField(this.spcameradistance, new GUIContent("Camera Height"));
	            EditorGUILayout.Space();
	            EditorGUILayout.PropertyField(this.splockmap, new GUIContent("Freeze Map"));
	            if (lockMap)
	            {
		            EditorGUI.indentLevel++;
		            EditorGUILayout.PropertyField(this.spcentermap, new GUIContent("Center Map"));
		            EditorGUILayout.PropertyField(this.spmapcenter, new GUIContent("Object to center on"));
		            
		            EditorGUI.indentLevel--;
		         }
	            EditorGUILayout.Space();
	            if (overlay)
	            {
	            	EditorGUILayout.PropertyField(this.spoverlaymap, new GUIContent("Show Overlay"));
	            }
	            EditorGUILayout.PropertyField(this.spzoommap, new GUIContent("Zoom Map on Scrollwheel"));
	            if (zoomMap)
	            {
		            EditorGUI.indentLevel++;
		            EditorGUILayout.PropertyField(this.spzoommapsensitivity, new GUIContent("Scrollwheel Sensitivity"));
		            EditorGUI.indentLevel--;
	            }
	            
	            EditorGUILayout.Space();
	            EditorGUILayout.PropertyField(this.spdragmap, new GUIContent("Drag Map with Mouse"));
	            if (dragMap)
	            {
		            EditorGUI.indentLevel++;
		            EditorGUILayout.PropertyField(this.spdragspeed, new GUIContent("Drag Speed"));
		            EditorGUILayout.PropertyField(this.spdragbutton, new GUIContent("Drag Button"));
		            Rect position = EditorGUILayout.GetControlRect(false, 2 * EditorGUIUtility.singleLineHeight); 
		            position.height *= 0.5f;
           
		            position.y += position.height - 20;
		            position.x += EditorGUIUtility.labelWidth -30;
		            position.width -= EditorGUIUtility.labelWidth + 26; 
		            GUIStyle style = GUI.skin.label;
		            style.fontSize = 10;
		            style.alignment = TextAnchor.UpperLeft; EditorGUI.LabelField(position, "Left", style);
		            style.alignment = TextAnchor.UpperCenter; EditorGUI.LabelField(position, "Right", style);
		            style.alignment = TextAnchor.UpperRight; EditorGUI.LabelField(position, "Middle", style);
		            EditorGUI.indentLevel--;
 
	            }

            this.serializedObject.ApplyModifiedProperties();
            }
	 
#endif

        }
    }
