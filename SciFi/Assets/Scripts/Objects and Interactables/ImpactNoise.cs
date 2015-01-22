using UnityEngine;
using System.Collections;

public class ImpactNoise : MonoBehaviour {

	public AudioClip[] rifleNoiseFX;
	public AudioClip[] SMGNoiseFX;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void makeNoise(Vector3 point, bool SMGMode)
	{
		if(SMGMode)
			AudioSource.PlayClipAtPoint(SMGNoiseFX[Random.Range(0, SMGNoiseFX.Length)], point);
		else
			AudioSource.PlayClipAtPoint(rifleNoiseFX[Random.Range(0, rifleNoiseFX.Length)], point);

	}
}
