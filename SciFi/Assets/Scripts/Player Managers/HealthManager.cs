using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {

	public float maxHealth;
	public float healthRegenRate;
	public bool isDead = false;
	private float curHealth;

	private HUDManager hudManager;
	private PlayerNetworkManager networkManager;

	// Use this for initialization
	void Start () {
		curHealth = maxHealth;
		hudManager = GetComponent<HUDManager>();
		networkManager = GetComponentInParent<PlayerNetworkManager>();
	}

	void FixedUpdate () {
		if (isDead){
			return;
		}
		hudManager.UpdateHealthEffects(curHealth);
		curHealth += healthRegenRate;
		curHealth = Mathf.Min(curHealth, maxHealth);
	}

	public void Damage(float damage){
		curHealth -= damage;
		if (curHealth <= 0){
			StartCoroutine("Death");
		}
		//play sounds and shit
	}

	private IEnumerator Death(){
		isDead = true;
		yield return null;
	}
}
