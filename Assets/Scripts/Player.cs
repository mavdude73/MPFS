using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
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
	private Behaviour[] disableComponentsOnDeath;
	private bool[] wasEnabled; 

	[SerializeField]
	private GameObject[] disableObjectsOnDeath;

	[SerializeField]
	private GameObject deathEffect;

	[SerializeField]
	private GameObject spawnEffect;

	private bool firstSetup;

	public void SetupPlayer ()
	{
		if(isLocalPlayer)
		{
			GameManager.instance.SetSceneCameraActive(false);
			GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
		}

		CmdBroadcastNewPlayerSetup();

	}

	[Command]
	private void CmdBroadcastNewPlayerSetup()
	{
		RpcSetupPlayerToAll();
	}

	[ClientRpc]
	private void RpcSetupPlayerToAll()
	{
		if(firstSetup)
		{
			wasEnabled = new bool[disableComponentsOnDeath.Length];
			for (int i = 0; i < wasEnabled.Length; i++)
			{
				wasEnabled[i] = disableComponentsOnDeath[i].enabled;
			}
			firstSetup = false;
		}

		SetDefaultValues();
	}

	void Update ()
	{
		Suicide();
	}

	void Suicide ()
	{
		if(!isLocalPlayer)
		{
			return;
		}
		if(Input.GetKeyDown(KeyCode.K))
		{
			RpcTakeDamage(99999);
		}
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
		}
	}

	private void Die ()
	{
		isDead = true;

		for (int i = 0; i < disableComponentsOnDeath.Length; i++)
		{
			disableComponentsOnDeath[i].enabled = false;
		}

		for (int i = 0; i < disableObjectsOnDeath.Length; i++)
		{
			disableObjectsOnDeath[i].SetActive(false);
		}

		Collider col = GetComponent<Collider>();
		if(col != null)
		{
			col.enabled = false;
		}

		GameObject _deathEffectInstance = (GameObject) Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(_deathEffectInstance, 3f);

		if(isLocalPlayer)
		{
			GameManager.instance.SetSceneCameraActive(true);
			GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
		}

		StartCoroutine(Respawn());
	}

	private IEnumerator Respawn()
	{
		yield return new WaitForSeconds (GameManager.instance.gameSettings.respawnTime);

		Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
		transform.position = spawnPoint.position;
		transform.rotation = spawnPoint.rotation;

		yield return new WaitForSeconds (0.1f);

		SetupPlayer();

	}

	public void SetDefaultValues()
	{
		isDead = false;
		currentHealth = maxHealth;

		for (int i = 0; i < disableComponentsOnDeath.Length; i++)
		{
			disableComponentsOnDeath[i].enabled = wasEnabled[i];
		}

		for (int i = 0; i < disableObjectsOnDeath.Length; i++)
		{
			disableObjectsOnDeath[i].SetActive(true);
		}

		Collider col = GetComponent<Collider>();
		if(col != null)
		{
			col.enabled = true;
		}

		GameObject _spawnEffectInstance = (GameObject) Instantiate(spawnEffect, transform.position, Quaternion.identity);
		Destroy(_spawnEffectInstance, 3f);

	}

}
