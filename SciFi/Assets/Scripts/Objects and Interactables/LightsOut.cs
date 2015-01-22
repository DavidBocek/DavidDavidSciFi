using UnityEngine;
using System.Collections;

public class LightsOut : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("KillLight");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator KillLight(){
		yield return new WaitForSeconds(.1f);
		gameObject.GetComponent<Light>().enabled = false;
	}


}
