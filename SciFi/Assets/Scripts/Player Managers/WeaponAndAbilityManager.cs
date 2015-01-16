using UnityEngine;
using System.Collections;

public class WeaponAndAbilityManager : MonoBehaviour {

	//animation delay and shooting speed vars
	public float gunModeSwitchDelay;
	public float aimAnimationDelay;
	public float leaveAimAnimationDelay;
	public float reloadAnimationDelay;
	public float meleeAnimationDelay;
	public float SMGShotCooldown;
	public float RifleShotCooldown;
	//innacuracy vars
	public float SMGShotConeRadiusMin;			//smallest cone can be standing no aim
	public float SMGShotConeRadiusMax;			//biggest cone can be standing no aim shooting
	public float SMGShotConeRadiusAbsoluteMax;	//biggest cone reachable, never can be larger than this
	public float SMGShotConeIncreasePerShot;	//increase cone radius per shot
	public float SMGShotConeIncreaseRate;		//increase rate of cone size
	public float SMGShotConeDecreaseRate;		//decrease rate of cone size
	public float SMGShotConeAimingBonus;		//flat reduction of cone size from being aimed
	public float SMGShotConeMovementPenalty;	//float increase of cone size from moving
	public float RifleShotConeRadiusMin;
	public float RifleShotConeRadiusMax;
	public float RifleShotConeRadiusAbsoluteMax;
	public float RifleShotConeIncreasePerShot;
	public float RifleShotConeIncreaseRate;
	public float RifleShotConeDecreaseRate;
	public float RifleShotConeAimingBonus;
	public float RifleShotConeMovementPenalty;
	//recoil vars
	public float SMGRecoilBoundHorizontal, SMGRecoilBoundVertical;
	public float RifleRecoilBoundHorizontal, RifleRecoilBoundVertical;

	public bool isInSMGMode {get; set;}
	public bool isAiming {get; set;}

	private AnimationManager animManager;
	private HUDManager hudManager;
	private MovementManager movementManager;

	private bool canShoot = true;
	private bool justShot = false;
	private bool isSMGInBurstMode = false;
	private int burstCount = 3;
	//innacuracy vars
	private float curShotConeRadius;
	private float curShotConeRadiusMin;
	private float curShotConeRadiusMax;

	//reload vars
	public int SMGBulletsPerClip = 30;
	public int rifleBulletsPerClip = 10;
	public int maxClips = 4;
	private int curClips;
	private int curSMGBullets;
	private int curRifleBullets;

	// Use this for initialization
	void Start () {
		isInSMGMode = true;
		curShotConeRadius = SMGShotConeRadiusMin;
		curShotConeRadiusMin = SMGShotConeRadiusMin;
		curShotConeRadiusMax = SMGShotConeRadiusMax;

		animManager = GetComponent<AnimationManager>();
		hudManager = GetComponent<HUDManager>();
		movementManager = GetComponent<MovementManager>();

		curClips = maxClips;
		curSMGBullets = SMGBulletsPerClip;
		curRifleBullets = rifleBulletsPerClip;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("SMG: " + curSMGBullets + " Rifle: " + curRifleBullets + " Clips: " + curClips);
		if (Input.GetButtonDown("SwitchGunMode")){
			isInSMGMode = !isInSMGMode;
			if (isAiming){
				isAiming = false;
				movementManager.StartCoroutine("StopAimingAnimation");
			}
			StartCoroutine("DelayShooting", gunModeSwitchDelay);
		}
		if (Input.GetButtonDown("AimDownSights")){
			if (!isAiming){
				StartCoroutine("Aim");
			} else {
				StartCoroutine("StopAiming");
			}
		}
		if (Input.GetButtonDown ("SwitchFireMode") && isInSMGMode){
			isSMGInBurstMode = !isSMGInBurstMode;
		}
		
		if (canShoot){
			if (Input.GetButtonDown ("Melee")){
				if (isAiming){
					isAiming = false;
					movementManager.StartCoroutine("StopAimingAnimation");
				}
				animManager.TriggerMelee();
				StartCoroutine("DelayShooting",meleeAnimationDelay);
			}
			if (Input.GetButtonDown("Reload")){
				if (isAiming){
					isAiming = false;
					movementManager.StartCoroutine("StopAimingAnimation");
				}
				if(curClips > 0){
					animManager.TriggerReload();
					StartCoroutine("DelayShooting",reloadAnimationDelay);
					StartCoroutine("Reload", reloadAnimationDelay);
				}
				else{
					//TODO: Play click sound
				}
			}
			if (isInSMGMode && isSMGInBurstMode){
				if (Input.GetButtonDown("Fire1") && curSMGBullets > 0){
					StartCoroutine("FireBurst", SMGShotCooldown);
				} else if(curSMGBullets <= 0){
					//TODO: Play click sound
				}
			} else if (isInSMGMode && !isSMGInBurstMode && curSMGBullets > 0){
				if (Input.GetButton("Fire1")){
					StartCoroutine("FireOneShot", SMGShotCooldown);
					curSMGBullets--;
				} else if(curSMGBullets <= 0){
					//TODO: Play click sound
				}
			} else if (!isInSMGMode){
				if (Input.GetButtonDown("Fire1") && curRifleBullets > 0){
					StartCoroutine("FireOneShot", RifleShotCooldown);
					curRifleBullets--;
				} else if(curRifleBullets <= 0){
					//TODO: Play click sound
				}
			}
			
		}

		//update innaccuracy values
		curShotConeRadiusMin = isInSMGMode ? SMGShotConeRadiusMin : RifleShotConeRadiusMin;
		curShotConeRadiusMax = isInSMGMode ? SMGShotConeRadiusMax : RifleShotConeRadiusMax;
		if (animManager.GetCurSpeedScaledFloat() >= .1f){
			curShotConeRadiusMin += isInSMGMode ? SMGShotConeMovementPenalty/2f : RifleShotConeMovementPenalty/2f;
			curShotConeRadiusMax += isInSMGMode ? SMGShotConeMovementPenalty : RifleShotConeMovementPenalty;
		}
		if (isAiming){
			curShotConeRadiusMin -= isInSMGMode ? SMGShotConeAimingBonus : RifleShotConeAimingBonus;
			curShotConeRadiusMax -= isInSMGMode ? SMGShotConeAimingBonus : RifleShotConeAimingBonus;
		}
		//make sure the shot cone radius isn't negative
		curShotConeRadiusMin = Mathf.Max(curShotConeRadiusMin, 0f);
		curShotConeRadiusMax = Mathf.Max(curShotConeRadiusMax, 0f);

		if (justShot){
			curShotConeRadius += isInSMGMode ? SMGShotConeIncreasePerShot : RifleShotConeIncreasePerShot;
			curShotConeRadius = isInSMGMode ? 	Mathf.Min (curShotConeRadius, SMGShotConeRadiusAbsoluteMax) : 
												Mathf.Min (curShotConeRadius, RifleShotConeRadiusAbsoluteMax);
			if (curShotConeRadius > curShotConeRadiusMax){
				curShotConeRadius -= isInSMGMode ? SMGShotConeDecreaseRate*Time.deltaTime : RifleShotConeDecreaseRate*Time.deltaTime;
				curShotConeRadius = Mathf.Max (curShotConeRadius, curShotConeRadiusMin);
			} else if (curShotConeRadius < curShotConeRadiusMin){
				curShotConeRadius += isInSMGMode ? SMGShotConeIncreaseRate*Time.deltaTime : RifleShotConeIncreaseRate*Time.deltaTime;
				curShotConeRadius = Mathf.Min (curShotConeRadius, curShotConeRadiusMax);
			}
			justShot = false;
		} else {
			if (curShotConeRadius > curShotConeRadiusMin){
				curShotConeRadius -= isInSMGMode ? SMGShotConeDecreaseRate*Time.deltaTime : RifleShotConeDecreaseRate*Time.deltaTime;
				curShotConeRadius = Mathf.Max (curShotConeRadius, curShotConeRadiusMin);
			} else if (curShotConeRadius < curShotConeRadiusMin){
				curShotConeRadius += isInSMGMode ? SMGShotConeIncreaseRate*Time.deltaTime : RifleShotConeIncreaseRate*Time.deltaTime;
				curShotConeRadius = Mathf.Min (curShotConeRadius, curShotConeRadiusMax);
			}
		}

		//update HUD values
		hudManager.UpdateCrosshairs(curShotConeRadius);
		hudManager.UpdateAmmoCount(curClips, curSMGBullets, curRifleBullets);

		//update animation values for shooting
		animManager.SetSMGNotSniperBool(isInSMGMode);
		animManager.SetAimingBool(isAiming);
		if (animManager.GetSMGNotSniperBool() && !isInSMGMode){
			animManager.SetSMGNotSniperBool(false);
		} else if (!animManager.GetSMGNotSniperBool() && isInSMGMode){
			animManager.SetSMGNotSniperBool(true);
		}
	}

	//business logic of actually firing the weapon in the world
	private void Shoot(){
		//for innacuracy
		justShot = true;
		//project cone circle cap to the visible target
		float distToTarget = 1000f;
		RaycastHit rayHitInfo = new RaycastHit();
		if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(.5f,.5f,0f)), out rayHitInfo, 1000f)){
			distToTarget = rayHitInfo.distance;
		}
		//apply innacuracy to find actual shot trajectory
		Vector2 shotConePoint = Random.insideUnitCircle * curShotConeRadius;
		Vector3 shotConeWorldPoint = Camera.main.transform.position + 
			Camera.main.transform.TransformDirection(Vector3.forward*distToTarget + Vector3.right*shotConePoint.x + Vector3.up * shotConePoint.y);
		Ray shotTrajectory = new Ray(Camera.main.transform.position, (shotConeWorldPoint - Camera.main.transform.position).normalized);
		//shoot along that trajectory
		rayHitInfo = new RaycastHit();
		if (Physics.Raycast(shotTrajectory, out rayHitInfo)){
			rayHitInfo.collider.gameObject.SendMessage("OnBulletHit", isInSMGMode, SendMessageOptions.DontRequireReceiver);
		}

		//recoil
		Vector2 recoilVector = new Vector2();
		recoilVector.x = isInSMGMode ? 	Random.Range(-SMGRecoilBoundHorizontal, SMGRecoilBoundHorizontal) : 
										Random.Range(-RifleRecoilBoundHorizontal, RifleRecoilBoundHorizontal);
		recoilVector.y = isInSMGMode ? 	Random.Range(SMGRecoilBoundVertical/5f, SMGRecoilBoundVertical) : 
										Random.Range(RifleRecoilBoundVertical/5f, RifleRecoilBoundVertical);
		movementManager.StartCoroutine("AdjustAimFromRecoil",recoilVector);

		//deal with shooting effects
		ActivateShootEffects(shotTrajectory);

		//debug
		Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward*5f, Color.grey);
		Debug.DrawRay(shotTrajectory.origin, shotTrajectory.direction*10f, Color.red);
	}

	private void ActivateShootEffects(Ray trajectory){
		//play sounds and shoot particles and such
	}

	private IEnumerator Reload(float reloadDelay){
		yield return new WaitForSeconds(reloadDelay);
		curSMGBullets = SMGBulletsPerClip;
		curRifleBullets = rifleBulletsPerClip;
		curClips--;
	}


	private IEnumerator Aim(){
		isAiming = true;
		movementManager.StartCoroutine("AimAnimation");
		StartCoroutine("DelayShooting",aimAnimationDelay);
		//deal with rifle sight picture later
		yield return null;
	}

	private IEnumerator StopAiming(){
		isAiming = false;
		//end sight picture if necessary
		movementManager.StartCoroutine("StopAimingAnimation");
		StartCoroutine("DelayShooting",leaveAimAnimationDelay);
		yield return null;
	}

	private IEnumerator FireBurst(float shotCooldown){
		canShoot = false;
		//we won't trigger the animation while aiming down sights, because then it would leave the aiming animation
		if (!isAiming){
			animManager.TriggerBurstShot();
		}
		for (int i=0; i<burstCount; i++){
			if(curSMGBullets > 0){
				Shoot();
				curSMGBullets--;
			} else {
				//TODO: play click noise
			}
			yield return new WaitForSeconds(shotCooldown);
		}
		canShoot = true;
	}

	private IEnumerator FireOneShot(float shotCooldown){
		canShoot = false;
		//we won't trigger the animation while aiming down sights, because then it would leave the aiming animation
		if (!isAiming){
			animManager.TriggerOneShot();
		}
		Shoot();
		yield return new WaitForSeconds(shotCooldown);
		canShoot = true;
	}

	private IEnumerator DelayShooting(float delay){
		canShoot = false;
		yield return new WaitForSeconds(delay);
		canShoot = true;
	}
}