using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {

	public GameObject rifleCrosshairsObj;
	public GameObject SMGCrosshairsObj;
	public GameObject ammoCounterObj;
	public float MAGIC_WORLD_UNITS_TO_PIXELS_RATIO;
	private const float RIFLE_SIZE_TO_TARGETING_AREA_RATIO = 12.5f;
	private const float SMG_SIZE_TO_TARGETING_AREA_RATIO = 3.63f;

	private WeaponAndAbilityManager weaponManager;

	private GameObject currentActiveCrosshairsObj;
	private AmmoCountManager ammoCountManager;

	private bool isAiming {get {return weaponManager.isAiming;}}
	private bool isInSMGMode { get {return weaponManager.isInSMGMode;}}

	// Use this for initialization
	void Start () {
		weaponManager = GetComponent<WeaponAndAbilityManager>();
		currentActiveCrosshairsObj = SMGCrosshairsObj;
		ammoCountManager = ammoCounterObj.GetComponent<AmmoCountManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isInSMGMode && currentActiveCrosshairsObj != SMGCrosshairsObj){
			currentActiveCrosshairsObj = SMGCrosshairsObj;
		} else if (!isInSMGMode && currentActiveCrosshairsObj != rifleCrosshairsObj){
			currentActiveCrosshairsObj = rifleCrosshairsObj;
		}

		if (isAiming){
			HideCrosshairs();
		} else {
			ShowActiveCrosshairs();
		}
	}

	public void UpdateCrosshairs(float shotRadius){
		RectTransform SMGRectTransform = SMGCrosshairsObj.GetComponent<RectTransform>();
		RectTransform rifleRectTransform = rifleCrosshairsObj.GetComponent<RectTransform>();
		float rPixSMG, rPixRifle;
		rPixSMG = shotRadius * SMG_SIZE_TO_TARGETING_AREA_RATIO * MAGIC_WORLD_UNITS_TO_PIXELS_RATIO;
		rPixRifle = shotRadius * RIFLE_SIZE_TO_TARGETING_AREA_RATIO * MAGIC_WORLD_UNITS_TO_PIXELS_RATIO;

		SMGRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rPixSMG);
		SMGRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rPixSMG);
		rifleRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rPixRifle);
		rifleRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rPixRifle);
	}

	public void UpdateAmmoCount(int clipCount, int SMGAmmoCount, int rifleAmmoCount){
		ammoCountManager.UpdateClipCount(clipCount);
		ammoCountManager.UpdateSMGAmmoCount(SMGAmmoCount);
		ammoCountManager.UpdateRifleAmmoCount(rifleAmmoCount);
	}

	public void UpdateHealthEffects(float curHealth){
		//TODO
	}

	private void HideCrosshairs(){
		rifleCrosshairsObj.SetActive(false);
		SMGCrosshairsObj.SetActive(false);
	}

	private void ShowActiveCrosshairs(){
		if (currentActiveCrosshairsObj == SMGCrosshairsObj){
			rifleCrosshairsObj.SetActive(false);
			SMGCrosshairsObj.SetActive(true);
		} else if (currentActiveCrosshairsObj == rifleCrosshairsObj){
			rifleCrosshairsObj.SetActive(true);
			SMGCrosshairsObj.SetActive(false);
		} else {
			rifleCrosshairsObj.SetActive(false);
			SMGCrosshairsObj.SetActive(false);
		}
	}
}
