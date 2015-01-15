using UnityEngine;
using System.Collections;

public class roboGui : MonoBehaviour {
	private float a=1f; //alpha control
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if (sliders.roboGui && this.renderer.material.color.a==0f){
		this.renderer.material.color= new Color(1,1,1,1f*a);	
		}
		
	if (!sliders.roboGui && this.renderer.material.color.a==1f*a){
		this.renderer.material.color= new Color(1,1,1,0f);	
		}
		if (a!=sliders.opacity &&sliders.roboGui){
			a= sliders.opacity;
this.renderer.material.color = new Color(this.renderer.material.color.r,this.renderer.material.color.b,this.renderer.material.color.g,1f*a);
		}
	}
}
