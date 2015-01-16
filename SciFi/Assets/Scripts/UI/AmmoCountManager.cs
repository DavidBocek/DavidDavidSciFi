using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AmmoCountManager : MonoBehaviour {
	
	public Text clipCounter;
	public GameObject SMGBulletsObj;
	public GameObject rifleBulletsObj;

	public Transform SMGBulletsStartTransform;
	public float degreesPerSMGBullet;
	public Transform rifleBulletsStartTransform;
	public float degreesPerRifleBullet;

	private int curClipCount, curSMGBullets, curRifleBullets;
	private List<GameObject> SMGBulletObjs;
	private List<GameObject> rifleBulletObjs;

	//initialization
	void Start(){
		SMGBulletObjs = new List<GameObject>();
		rifleBulletObjs = new List<GameObject>();
		for (int i=0; i<GameObject.FindWithTag("Player").GetComponent<WeaponAndAbilityManager>().SMGBulletsPerClip; i++){
			GameObject newBulletObj = (GameObject) Instantiate(SMGBulletsObj, SMGBulletsStartTransform.position, SMGBulletsStartTransform.rotation);
			newBulletObj.transform.localEulerAngles = new Vector3(0f,0f,45f);
			newBulletObj.transform.Rotate(Vector3.forward, degreesPerSMGBullet*i);
			newBulletObj.transform.SetParent(gameObject.transform);
			newBulletObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-407f,-200f);
			newBulletObj.SetActive(false);
			SMGBulletObjs.Add(newBulletObj);
		}
		for (int i=0; i<GameObject.FindWithTag("Player").GetComponent<WeaponAndAbilityManager>().rifleBulletsPerClip; i++){
			GameObject newBulletObj = (GameObject) Instantiate(rifleBulletsObj, rifleBulletsStartTransform.position, rifleBulletsStartTransform.rotation);
			newBulletObj.transform.localEulerAngles = new Vector3(0f,0f,-40f);
			newBulletObj.transform.Rotate(Vector3.forward, degreesPerRifleBullet*i);
			newBulletObj.transform.SetParent(gameObject.transform);
			newBulletObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-407f,-200f);
			newBulletObj.SetActive(false);
			rifleBulletObjs.Add(newBulletObj);
		}
	}

	void OnGUI(){
		clipCounter.text = "x"+curClipCount;

		for (int i=0; i<SMGBulletObjs.Count; i++){
			if (i >= curSMGBullets){
				SMGBulletObjs[i].SetActive(false);
			} else {
				SMGBulletObjs[i].SetActive(true);
			}
		}
		for (int i=0; i<rifleBulletObjs.Count; i++){
			if (i >= curRifleBullets){
				rifleBulletObjs[i].SetActive(false);
			} else {
				rifleBulletObjs[i].SetActive(true);
			}
		}
	}

	public void UpdateClipCount(int clipCount){
		curClipCount = clipCount;
	}
	public void UpdateSMGAmmoCount(int ammo){
		curSMGBullets = ammo;
	}
	public void UpdateRifleAmmoCount(int ammo){
		curRifleBullets = ammo;
	}
}
