using UnityEngine;
using System.Collections;

public class CompanionAnimationManager : MonoBehaviour {

	private Animator anim;

	private int IsSprintingId;
	private int MeleeTriggerId;
	private int ReloadTriggerId;
	private int CurVelScaledXId;
	private int CurVelScaledZId;
	private int CurSpeedScaledId;
	private int DeathTriggerId;

	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator>();

		IsSprintingId = Animator.StringToHash("IsSprinting");
		MeleeTriggerId = Animator.StringToHash("MeleeTrigger");
		ReloadTriggerId = Animator.StringToHash("ReloadTrigger");
		CurVelScaledXId = Animator.StringToHash("CurVelScaledX");
		CurVelScaledZId = Animator.StringToHash("CurVelScaledZ");
		CurSpeedScaledId = Animator.StringToHash("CurSpeedScaled");
		DeathTriggerId = Animator.StringToHash("DeathTrigger");
	}
	
	public void TriggerMelee(){
		anim.SetTrigger(MeleeTriggerId);
	}

	public void TriggerReload(){
		anim.SetTrigger(ReloadTriggerId);
	}

	public void TriggerDeath(){
		anim.SetTrigger(DeathTriggerId);
	}
	
	public bool GetSprintingBool(){
		return anim.GetBool(IsSprintingId);
	}
	public void SetSprintingBool(bool isSprinting){
		anim.SetBool(IsSprintingId, isSprinting);
	}

	public float GetCurVelScaledXFloat(){
		return anim.GetFloat(CurVelScaledXId);
	}
	public void SetCurVelScaledXFloat(float curVelScaledX){
		anim.SetFloat(CurVelScaledXId, curVelScaledX);
	}

	public float GetCurVelScaledZFloat(){
		return anim.GetFloat(CurVelScaledZId);
	}
	public void SetCurVelScaledZFloat(float curVelScaledZ){
		anim.SetFloat(CurVelScaledZId, curVelScaledZ);
	}
	
	public float GetCurSpeedScaledFloat(){
		return anim.GetFloat(CurSpeedScaledId);
	}
	public void SetCurSpeedScaledFloat(float curSpeedScaled){
		anim.SetFloat(CurSpeedScaledId, curSpeedScaled);
	}
}
