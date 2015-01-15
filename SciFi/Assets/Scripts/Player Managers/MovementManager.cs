using UnityEngine;
using System.Collections;

public class MovementManager : MonoBehaviour {

	public float movementSpeed;
	public float sprintMultiplier;
	public float aimingMultiplier;
	public float mouseXSensitivity;
	public float mouseYSensitivity;
	public float aimingMouseMultiplier;
	//aiming gun animation vars
	public GameObject armsAndGunModelRoot;
	public Transform aimingGunTransform;
	private Vector3 aimingGunPositionChange;

	private AnimationManager animManager;
	private WeaponAndAbilityManager weapManager;
	private Rigidbody rb;
	private GameObject playerCameraObj;

	private Vector3 inputDir = Vector3.zero;
	private Vector3 curVel = Vector3.zero;
	private bool isSprinting = false;
	private bool isAiming { get{ return weapManager.isAiming;}}

	// Use this for initialization
	void Start () {
		aimingGunPositionChange = aimingGunTransform.localPosition - armsAndGunModelRoot.transform.localPosition;;

		animManager = GetComponent<AnimationManager>();
		weapManager = GetComponent<WeaponAndAbilityManager>();
		rb = GetComponent<Rigidbody>();
		playerCameraObj = GetComponentInChildren<Camera>().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (isAiming){
			isSprinting = false;
		} else {
			isSprinting = Input.GetButton("Sprint");
		}
		UpdateAiming();
	}

	void FixedUpdate(){
		UpdateRigidbodyPosition();
		animManager.SetCurSpeedScaledFloat(curVel.magnitude/(movementSpeed+.00000001f));	//avoid dividing by 0 errors
		animManager.SetSprintingBool(isSprinting);
	}

	void UpdateRigidbodyPosition(){
		inputDir.x = Input.GetAxisRaw("Horizontal");
		inputDir.z = Input.GetAxisRaw("Vertical");
		inputDir.Normalize();
		inputDir = transform.TransformDirection(inputDir);
		if (inputDir.sqrMagnitude > 0){
			//slow accel for moving, but not slow to stop (it feels weird)
			if (isAiming){
				inputDir = Vector3.Lerp(curVel, inputDir*movementSpeed*aimingMultiplier, .1f);
			} else if (isSprinting) {
				inputDir = Vector3.Lerp(curVel, inputDir*movementSpeed*sprintMultiplier, .1f);
			} else {
				inputDir = Vector3.Lerp(curVel, inputDir*movementSpeed, .1f);
			}
		}
		curVel = inputDir;
		rb.AddForce(inputDir - rb.velocity,ForceMode.VelocityChange);
	}

	void UpdateAiming(){
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");
		if (!isAiming){
			transform.Rotate(Vector3.up, mouseX*mouseXSensitivity*SPlayerSettings.mouseSensitivity);
		} else {
			transform.Rotate(Vector3.up, mouseX*mouseXSensitivity*aimingMouseMultiplier*SPlayerSettings.mouseSensitivity);
		}
		Vector3 goalCamRot = playerCameraObj.transform.localEulerAngles;
		float amountToMoveY = SPlayerSettings.invertMouse ? -mouseY*mouseYSensitivity : mouseY*mouseYSensitivity;
		amountToMoveY *= isAiming ? SPlayerSettings.mouseSensitivity*aimingMouseMultiplier : SPlayerSettings.mouseSensitivity;
		goalCamRot.x += amountToMoveY;
		if (goalCamRot.x < 275f && goalCamRot.x > 180f){goalCamRot.x = 275.0f;}
		else if (goalCamRot.x > 80f && goalCamRot.x< 180f){goalCamRot.x = 80.0f;}
		playerCameraObj.transform.localEulerAngles = goalCamRot;
	}

	public IEnumerator AdjustAimFromRecoil(Vector2 recoilVector){
		for (float i=0f; i<1f; i+=Time.deltaTime/.15f){
			transform.Rotate(Vector3.up, recoilVector.x*.15f);
			playerCameraObj.transform.Rotate(Vector3.right, -recoilVector.y*.15f);
			yield return null;
		}
	}

	public IEnumerator AimAnimation(){
		//lerp to aim position
		for (int i=0; i<5; i++){
			armsAndGunModelRoot.transform.localPosition += aimingGunPositionChange/5f;
			yield return null;
		}
	}

	public IEnumerator StopAimingAnimation(){
		//lerp out of aim position
		for (int i=0; i<5; i++){
			armsAndGunModelRoot.transform.localPosition -= aimingGunPositionChange/5f;
			yield return null;
		}
	}
}





























