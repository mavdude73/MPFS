using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponMananger))]
public class PlayerShoot : NetworkBehaviour {

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;

	private const string PLAYER_TAG = "Player";

	private PlayerWeapon currentWeapon;
	private WeaponMananger weaponManager;

	void Start ()
	{
		if(cam == null)
		{
			Debug.LogError("Playershoot: No camera referenced");
			this.enabled = false;
		}

		weaponManager = GetComponent<WeaponMananger>();
	}

	void Update()
	{
		currentWeapon = weaponManager.GetCurrentWeapon();

		if(currentWeapon.fireRate <= 0f)
		{
			if(Input.GetButtonDown("Fire1"))
				{
					Shoot();
				}
		}
		else
		{
			if(Input.GetButtonDown("Fire1"))
			{
				InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
			}
			else if (Input.GetButtonUp("Fire1"))
			{
				CancelInvoke("Shoot");
			}
		}


	}



	[Command]
	void CmdOnHit(Vector3 pos, Vector3 normal)
	{
		RpcDoHitEffect(pos, normal);
	}

	[ClientRpc]
	void RpcDoHitEffect(Vector3 pos, Vector3 normal)
	{
		GameObject hitEffect = (GameObject) Instantiate (weaponManager.GetCurrentGraphics().hitEffectPrefab, pos, Quaternion.LookRotation(normal));
		Destroy(hitEffect, 2f);
	}

	[Command]
	void CmdOnShoot()
	{
		RpcDoShootEffect();
	}

	[ClientRpc]
	void RpcDoShootEffect()
	{
		weaponManager.GetCurrentGraphics().muzzleFlash.Play();
	}


	[Client]
	void Shoot()
	{
		if(!isLocalPlayer)
		{
			return;
		}

		CmdOnShoot();

		RaycastHit hit;
		if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask))
		{
			if(hit.collider.tag == PLAYER_TAG)
			{
				CmdPlayerShot(hit.collider.name, currentWeapon.damage);
			}

			CmdOnHit(hit.point, hit.normal);

		}
	}

	[Command]
	void CmdPlayerShot(string playerID, int damage)
	{
		Player player = GameManager.GetPlayer(playerID);
		player.RpcTakeDamage(damage);
	}


}
