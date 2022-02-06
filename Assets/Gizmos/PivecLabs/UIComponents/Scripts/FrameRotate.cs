namespace GameCreator.UIComponents
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System.Linq;
	
public class FrameRotate : MonoBehaviour
{
	private Transform target;
	public bool rotating;

	void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        
	    if (rotating == true) 
	    {
		    Vector3 frameAngle = new Vector3();
		    frameAngle.z = target.transform.eulerAngles.y;
		    this.transform.eulerAngles = frameAngle;
        

	    	
	    }
	  

    }
	}
}

