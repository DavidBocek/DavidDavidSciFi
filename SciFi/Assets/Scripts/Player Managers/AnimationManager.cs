using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour {

	private Animator anim;

	private int ShootOneTriggerId;
	private int MeleeTriggerId;
	private int CurSpeedScaledId;
	private int IsSprintingId;
	private int IsSMGNotSniperId;
	private int GrenadeTriggerId;
	private int ShootBurstTriggerId;
	private int IsAimingId;
	private int ReloadTriggerId;

	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator>();
		ShootOneTriggerId = Animator.StringToHash("ShootOneTrigger");
		MeleeTriggerId = Animator.StringToHash("MeleeTrigger");
		CurSpeedScaledId = Animator.StringToHash("CurSpeedScaled");
		IsSprintingId = Animator.StringToHash("IsSprinting");
		IsSMGNotSniperId = Animator.StringToHash("IsSMGNotSniper");
		GrenadeTriggerId = Animator.StringToHash("GrenadeTrigger");
		ShootBurstTriggerId = Animator.StringToHash("ShootBurstTrigger");
		IsAimingId = Animator.StringToHash("IsAiming");
		ReloadTriggerId = Animator.StringToHash("ReloadTrigger");
	}
	
	public void TriggerOneShot(){
		anim.SetTrigger(ShootOneTriggerId);
	}
	public void TriggerBurstShot(){
		anim.SetTrigger(ShootBurstTriggerId);
	}

	public void TriggerMelee(){
		anim.SetTrigger(MeleeTriggerId);
	}

	public void TriggerGrenade(){
		anim.SetTrigger(GrenadeTriggerId);
	}

	public void TriggerReload(){
		anim.SetTrigger(ReloadTriggerId);
	}

	public bool GetSprintingBool(){
		return anim.GetBool(IsSprintingId);
	}
	public void SetSprintingBool(bool isSprinting){
		anim.SetBool(IsSprintingId, isSprinting);
	}

	public bool GetAimingBool(){
		return anim.GetBool(IsAimingId);
	}
	public void SetAimingBool(bool isAiming){
		anim.SetBool(IsAimingId, isAiming);
	}

	public bool GetSMGNotSniperBool(){
		return anim.GetBool(IsSMGNotSniperId);
	}
	public void SetSMGNotSniperBool(bool isSMGNotSniper){
		anim.SetBool(IsSMGNotSniperId, isSMGNotSniper);
	}

	public float GetCurSpeedScaledFloat(){
		return anim.GetFloat(CurSpeedScaledId);
	}
	public void SetCurSpeedScaledFloat(float curSpeedScaled){
		anim.SetFloat(CurSpeedScaledId, curSpeedScaled);
	}
}
