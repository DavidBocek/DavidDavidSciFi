using UnityEngine;
using System.Collections;

public class BulletMove : MonoBehaviour {


	public float speed;

	// Use this for initialization
	void Start () 
	{
		transform.Rotate(0,90,0);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void FixedUpdate()
	{
		transform.Translate(speed * Time.fixedDeltaTime * Vector3.left);
	}
}
