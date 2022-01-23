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
    public class ActionDisplayMiniMap : IAction
    {
        public GameObject mapPanel;
	    public GameObject mapManager;
	    public GameObject rawImage;
	    public GameObject mapcanvas;
	    public bool overlay;
	    public bool rotating;

	    [Range(1,100)]
	    public float cameraSize = 20f;
	    [Range(1,100)]
	    public float cameraDistance = 30f;

	    public LayerMask cullingMask = ~0;
	    public bool occlusionCulling = true;

	    public bool mapMarkers;
	    public bool showMarkers;
	    [Range(0,5)]
	    public float markerSize = 0.2f;
	    
	   
	  
	    
        private PlayerCharacter player;
	    public bool usePlayer = true;
	    public GameObject objectTransform;
	    private Transform objecttransform;
	    
	    
        public enum MAPPOSITION
        {
            TopRight,
            TopLeft,
            BottomLeft,
            BottomRight
            
        }


        public MAPPOSITION mapPosition = MAPPOSITION.TopRight;

        private float mmwidth;
	    public float mmoffsetx;
	    public float mmoffsety;
        private RectTransform m_RectTransform;

     

        RenderTexture renderTexture;
        RectTransform rt;
        RawImage img;
        private Camera targetCamera;
    
	    private MapManager fullscreen;

	    private GameObject rotatingFrame;
	    
	    [Range(1,30)]
	    public int renderFrequency = 1;
	    
	    public NumberProperty resizeamount = new NumberProperty(0.0f);
	    public bool resized = false;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

	        GameObject go = GameObject.Find ("MiniMapCamera");
	        
	      if (mapcanvas != null)
	        
		        mapcanvas.SetActive(false);
	        
	         
	        
	        if (go){
	        	
			    img = null;
		        targetCamera = null;
		        renderTexture = null;
		        Destroy (go.gameObject);

	        }
	        
	        if (usePlayer)

	    		 player = HookPlayer.Instance.Get<PlayerCharacter>();
            
	        else 
		        	objecttransform = objectTransform.GetComponent<Transform>();
	       
	       
	        
	        fullscreen = mapManager.GetComponent<MapManager>();
	        fullscreen.miniMapshowing = true;
	        fullscreen.fullMapshowing = false;
	        fullscreen.switchCams();
	        
	        if (renderTexture == null)
	        {
                rt = (RectTransform)rawImage.transform;
                renderTexture = new RenderTexture((int)rt.rect.width, (int)rt.rect.height, 32);
                renderTexture.Create();

	        }

            if (img == null)
            {

                img = rawImage.gameObject.GetComponent<RawImage>();
                img.texture = renderTexture;

            }

            if (targetCamera == null)
            {
           
	            GameObject cameraMinimap = new GameObject();
                


	            targetCamera = cameraMinimap.AddComponent<Camera>();
	            targetCamera.enabled = false;
	            targetCamera.cullingMask = this.cullingMask;
	            targetCamera.useOcclusionCulling = occlusionCulling;
	            targetCamera.allowHDR = true;
	            targetCamera.allowMSAA = true;
	            targetCamera.allowDynamicResolution = true;
	            targetCamera.targetTexture = renderTexture;
                targetCamera.orthographic = true;
	            targetCamera.orthographicSize = cameraSize;
	            targetCamera.name = "MiniMapCamera";
	            if (usePlayer)
	            {
		            cameraMinimap.transform.parent = player.transform;
		            targetCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + cameraDistance, player.transform.position.z);
		            targetCamera.transform.LookAt(player.transform);

	            }
	            
	            else 
	            {
		            cameraMinimap.transform.parent = objecttransform;
		            targetCamera.transform.position = new Vector3(objecttransform.position.x, objecttransform.position.y + cameraDistance, objecttransform.position.z);
		            targetCamera.transform.LookAt(objecttransform);

	            }

	            targetCamera.transform.localRotation = Quaternion.Euler(90.0f,0.0f,0.0f);
	            targetCamera.Render();
            }

            
	        RectTransform parentCanvas = mapPanel.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

            m_RectTransform = mapPanel.GetComponent<RectTransform>();
            m_RectTransform.localScale += new Vector3(0, 0, 0);
            mmwidth = m_RectTransform.rect.width;

	        if (resized == true)
	        {
		        m_RectTransform.localScale += new Vector3(resizeamount.GetValue(target), resizeamount.GetValue(target), 0);
		        mmwidth = (m_RectTransform.rect.width * (resizeamount.GetValue(target) + 1));
       
	        }
	        else
	        {
		        m_RectTransform.localScale += new Vector3(0, 0, 0);
		        mmwidth = (m_RectTransform.rect.width * (0 + 1));
	        }

           
                        switch (mapPosition)
                        {
                            case MAPPOSITION.BottomLeft:
	                            m_RectTransform.anchoredPosition = new Vector3(mmoffsetx, mmoffsety);
	                            fullscreen.minimapPosition = MapManager.MINIMAPPOSITION.BottomLeft;
                                break;
                            case MAPPOSITION.TopLeft:
                                m_RectTransform.anchoredPosition = new Vector3(mmoffsetx, parentCanvas.rect.height - (mmwidth + mmoffsety));
	                            fullscreen.minimapPosition = MapManager.MINIMAPPOSITION.TopLeft;
	                            break;
                            case MAPPOSITION.TopRight:
	                            m_RectTransform.anchoredPosition = new Vector3(parentCanvas.rect.width - (mmwidth + mmoffsetx), parentCanvas.rect.height - (mmwidth + mmoffsety));
	                            fullscreen.minimapPosition = MapManager.MINIMAPPOSITION.TopRight;
	                            break;
                            case MAPPOSITION.BottomRight:
                                m_RectTransform.anchoredPosition = new Vector3(parentCanvas.rect.width - (mmwidth + mmoffsetx), mmoffsety);
	                            fullscreen.minimapPosition = MapManager.MINIMAPPOSITION.BottomRight;
	                            break;

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
        
	        if (rotating == false)
	 
	        {
	        	
		        mapPanel.GetComponentInChildren<FrameRotate>().rotating = false;
	        
	        }
	        
	        else 
	        
	        {
		        mapPanel.GetComponentInChildren<FrameRotate>().rotating = true;
	        	
	        }



	        mapPanel.SetActive(true);


            return true;

        }

	    void Update () {
	    	
	    	
		    if (targetCamera != null)
		    {
			   
			    if (Time.frameCount % renderFrequency == 0)
			    {   
				   	    targetCamera.Render();
			    }
			   	 
			   
		    }
     }
	  
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

        public static new string NAME = "UI/MiniMap/Display MiniMap";
            private const string NODE_TITLE = "Display MiniMap";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/UIComponents/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

	    private SerializedProperty spmapmanager;
	    private SerializedProperty spmapposition;
        private SerializedProperty spmappanel;
        private SerializedProperty sprawimage;
	    private SerializedProperty spcamerasize;
	    private SerializedProperty spcameradistance;
	    private SerializedProperty spmapcanvas;
	    private SerializedProperty spoverlay;
	    private SerializedProperty sprotate;
	    private SerializedProperty spmapmarkers;
	    private SerializedProperty spshowmarkers;
	    private SerializedProperty spmarkersize;
	    private SerializedProperty spmmoffsetx;
	    private SerializedProperty spmmoffsety;
	    private SerializedProperty sprenderFrequency;
	    private SerializedProperty spcullingMask;
	    private SerializedProperty spocclusionCulling;
	    private SerializedProperty spusePlayer;
	    private SerializedProperty spobjectTransform;
	    private SerializedProperty spmapresized;
	    private SerializedProperty spmapResize;

	    
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
	            this.spmapposition = this.serializedObject.FindProperty("mapPosition");
            	this.spmappanel = this.serializedObject.FindProperty("mapPanel");
            	this.sprawimage = this.serializedObject.FindProperty("rawImage");
	        	this.spcamerasize = this.serializedObject.FindProperty("cameraSize");
	            this.spcameradistance = this.serializedObject.FindProperty("cameraDistance");
	            this.spmapcanvas = this.serializedObject.FindProperty("mapcanvas");
	            this.spoverlay = this.serializedObject.FindProperty("overlay");
	            this.sprotate = this.serializedObject.FindProperty("rotating");
	            this.spmapmarkers = this.serializedObject.FindProperty("mapMarkers");
	            this.spshowmarkers = this.serializedObject.FindProperty("showMarkers");
	            this.spmarkersize = this.serializedObject.FindProperty("markerSize");
	            this.spmmoffsetx = this.serializedObject.FindProperty("mmoffsetx");
	            this.spmmoffsety = this.serializedObject.FindProperty("mmoffsety");
	            this.sprenderFrequency = this.serializedObject.FindProperty("renderFrequency");
	            this.spcullingMask = this.serializedObject.FindProperty("cullingMask");
	            this.spocclusionCulling = this.serializedObject.FindProperty("occlusionCulling");
	            this.spusePlayer = this.serializedObject.FindProperty("usePlayer");
	            this.spobjectTransform = this.serializedObject.FindProperty("objectTransform");
	            this.spmapresized = this.serializedObject.FindProperty("resized");
	            this.spmapResize = this.serializedObject.FindProperty("resizeamount");

        }


        protected override void OnDisableEditorChild()
            {
        		this.spmapmanager = null;
	            this.spmapposition = null;
            	this.spmappanel = null;
            	this.sprawimage = null;
	            this.spmapcanvas = null;
	            this.spoverlay = null;
	            this.sprotate = null;
	            this.spshowmarkers = null;
	            this.spmapmarkers = null;
	            this.spmarkersize = null;
	            this.spmmoffsetx = null;
	            this.spmmoffsety = null;
	            this.sprenderFrequency = null;
	            this.spcullingMask = null;
	            this.spocclusionCulling = null;
	            this.spusePlayer = null;
	            this.spobjectTransform = null;
	            this.spcamerasize = null;
	            this.spcameradistance = null;
	            this.spmapresized = null;
	            this.spmapResize = null;


        }

        public override void OnInspectorGUI()
            {
                this.serializedObject.Update();
	            EditorGUILayout.PropertyField(this.spmapmanager, new GUIContent("Map Manager"));
	            EditorGUILayout.PropertyField(this.spmappanel, new GUIContent("Panel - MiniMap"));
            	EditorGUILayout.PropertyField(this.sprawimage, new GUIContent("RawImage texturemap"));
	            EditorGUILayout.PropertyField(this.spoverlay, new GUIContent("Full Map has Overlay"));
	            if (overlay)
	            {
		            EditorGUILayout.PropertyField(this.spmapcanvas, new GUIContent("Overlay Canvas"));
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
	            EditorGUILayout.LabelField("Properties");
	            EditorGUI.indentLevel++;
	            EditorGUILayout.PropertyField(this.spmapposition, new GUIContent("MiniMap Position"));
            	EditorGUILayout.Space();
	            EditorGUILayout.PropertyField(this.spcullingMask, new GUIContent("Culling Mask"));
	            EditorGUILayout.PropertyField(this.spocclusionCulling, new GUIContent("Occlusion Culling"));
            	EditorGUILayout.Space();
	            EditorGUILayout.PropertyField(this.spusePlayer, new GUIContent("Center on Player"));
	            if (usePlayer == false)
	            {
		            EditorGUILayout.PropertyField(this.spobjectTransform, new GUIContent("Center on GameObject"));

	            }
	            EditorGUILayout.PropertyField(this.spcamerasize, new GUIContent("Field of View"));
	            EditorGUILayout.PropertyField(this.spcameradistance, new GUIContent("Camera Height"));
	            EditorGUILayout.PropertyField(this.sprotate, new GUIContent("Rotate Frame"));
	            EditorGUILayout.Space();
	            EditorGUILayout.PropertyField(this.spmapresized, new GUIContent("Minimap resized?"));

	            if (resized == true)
	            {
		            EditorGUILayout.PropertyField(this.spmapResize, new GUIContent("Resize Minimap by"));

	            }

	            EditorGUILayout.PropertyField(this.spmmoffsetx, new GUIContent("Position Offset x"));
	            EditorGUILayout.PropertyField(this.spmmoffsety, new GUIContent("Position Offset y"));
	            EditorGUILayout.Space();
	            EditorGUILayout.PropertyField(this.sprenderFrequency, new GUIContent("Render Frequency"));
	            EditorGUILayout.Space();
	            EditorGUI.indentLevel--;

            this.serializedObject.ApplyModifiedProperties();
            }
	    
	   

	 
#endif

        }
    }
