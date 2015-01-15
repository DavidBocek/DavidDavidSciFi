using UnityEngine;
using System.Collections;

public class transparency : MonoBehaviour
{
	private float a=1f; //alpha control
	void Awake()
	{
		a= sliders.opacity;
		this.renderer.material.color = new Color(this.renderer.material.color.r,this.renderer.material.color.b,this.renderer.material.color.g,.65f*a);
	}

	void Start ()
	{
		
	}
	
	
	

	void Update ()
	{
	
		if (a!=sliders.opacity){
			a= sliders.opacity;
this.renderer.material.color = new Color(this.renderer.material.color.r,this.renderer.material.color.b,this.renderer.material.color.g,.65f*a);
		}
		
	}

  

}