using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {

	[SyncVar]
	private bool _isDead = false;

	public bool isDead
	{
		get { return _isDead; }
		protected set { _isDead = value; }

	}

	[SerializeField]
	private int maxHealth = 100;

	[SyncVar]
	private int currentHealth;

	[SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled; 

	public void Setup ()
	{
		wasEnabled = new bool[disableOnDeath.Length];
		for (int i = 0; i < wasEnabled.Length; i++)
		{
			wasEnabled[i] = disableOnDeath[i].enabled;
		}

		SetDefaultValues();
	}

	[ClientRpc]
	public void RpcTakeDamage(int damage)
	{
		if(isDead)
		{
			return;
		}
		currentHealth -= damage;
		Debug.Log(transform.name + ": " + currentHealth + " hp");

		if(currentHealth <= 0)
		{
			Die();
			StartCoroutine(Respawn());
		}
	}

	private void Die ()
	{
		isDead = true;

		for (int i = 0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = false;
		}

		Collider col = GetComponent<Collider>();
		if(col != null)
		{
			col.enabled = false;
		}

	}

	private IEnumerator Respawn()
	{
		yield return new WaitForSeconds (GameManager.instance.gameSettings.respawnTime);
		SetDefaultValues();
		Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
		transform.position = spawnPoint.position;
		transform.rotation = spawnPoint.rotation;
	}

	public void SetDefaultValues()
	{
		isDead = false;
		currentHealth = maxHealth;

		for (int i = 0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = wasEnabled[i];
		}

		Collider col = GetComponent<Collider>();
		if(col != null)
		{
			col.enabled = true;
		}
	}

}
