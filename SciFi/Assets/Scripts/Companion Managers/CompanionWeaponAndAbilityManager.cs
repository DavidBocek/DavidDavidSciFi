using UnityEngine;
using System.Collections;

public class CompanionWeaponAndAbilityManager : MonoBehaviour {

	//noisy vars
	public Transform sourcePoint;
	public AudioClip[] SMGNoises;
	public AudioClip[] rifleNoises;
	public AudioClip reloadAudio;

	//visual effect vars
	public GameObject rifleTrail;
	public GameObject SMGTrail;
	public GameObject rifleMuzz;
	public GameObject SMGMuzz;
	public GameObject[] rifleImpacts;
	public GameObject[] SMGImpacts;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ActivateProxyShootEffects(Ray traj, bool isInSMGMode){
		RaycastHit hitInfo = new RaycastHit();
		float dist = 100f;
		if (Physics.Raycast(traj, out hitInfo, 100f)){
			dist = hitInfo.distance;
		}

		Quaternion bullRot;
		
		if(hitInfo.collider != null){
			bullRot = Quaternion.LookRotation(hitInfo.point-sourcePoint.position);
		} else {
			bullRot = Quaternion.LookRotation(traj.direction);
		}
		
		if(isInSMGMode){
			//visual effects
			GameObject tempBullet = (GameObject)GameObject.Instantiate(SMGTrail, sourcePoint.position + 1.25f * sourcePoint.TransformDirection(Vector3.forward), bullRot);
			Destroy (tempBullet, dist/tempBullet.GetComponent<BulletMove>().speed);
			
			GameObject flash = (GameObject)GameObject.Instantiate(SMGMuzz, sourcePoint.position - .05f * sourcePoint.TransformDirection(Vector3.forward), sourcePoint.rotation);
			flash.transform.Rotate(Vector3.forward, Random.Range(0,91));
			Destroy (flash, .05f);
			
			if(dist < 100f && hitInfo.collider != null)
			{
				GameObject impact = (GameObject)GameObject.Instantiate(SMGImpacts[Random.Range(0,SMGImpacts.Length)], hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
				impact.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
				Destroy(impact,.95f);
				
				ImpactNoise impactNoise = hitInfo.collider.gameObject.GetComponent<ImpactNoise>();
				if(impactNoise != null){
					impactNoise.makeNoise(hitInfo.point, isInSMGMode);
				}
			}
			
			//sound effects
			sourcePoint.audio.clip = SMGNoises[Random.Range (0,SMGNoises.Length)];
			sourcePoint.audio.Play();
		} else {
			//visual effects
			GameObject tempBullet = (GameObject)GameObject.Instantiate(rifleTrail, sourcePoint.position + 1.25f * sourcePoint.TransformDirection(Vector3.forward), bullRot);
			Destroy (tempBullet, dist/tempBullet.GetComponent<BulletMove>().speed);
			
			GameObject flash = (GameObject)GameObject.Instantiate(rifleMuzz, sourcePoint.position, sourcePoint.rotation);
			flash.transform.Rotate(Vector3.forward, Random.Range(0,91));
			Destroy (flash, .05f);
			
			if(dist < 100f && hitInfo.collider != null)
			{
				GameObject impact = (GameObject)GameObject.Instantiate(rifleImpacts[Random.Range(0,rifleImpacts.Length)], hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
				impact.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
				Destroy(impact,.95f);
				
				ImpactNoise impactNoise = hitInfo.collider.gameObject.GetComponent<ImpactNoise>();
				if(impactNoise != null){
					impactNoise.makeNoise(hitInfo.point, isInSMGMode);
				}
			}
			
			//sound effects
			sourcePoint.audio.clip = rifleNoises[Random.Range (0,rifleNoises.Length)];
			sourcePoint.audio.Play();
		}
	}

	public void ProxyReloadEffects(){
		sourcePoint.audio.clip = reloadAudio;
		sourcePoint.audio.Play();
	}
}
