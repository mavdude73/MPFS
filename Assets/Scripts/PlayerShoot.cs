using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;

	private const string PLAYER_TAG = "Player";

	public PlayerWeapon weapon;

	void Start ()
	{
		if(cam == null)
		{
			Debug.LogError("Playershoot: No camera referenced");
			this.enabled = false;
		}

	}

	void Update()
	{
		if(Input.GetButtonDown("Fire1"))
		{
			FireWeapon();
		}
	}

	[Client]
	void FireWeapon()
	{
		RaycastHit hit;
		if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
		{
			if(hit.collider.tag == PLAYER_TAG)
			{
				CmdPlayerShot(hit.collider.name, weapon.damage);
			}
		}
	}

	[Command]
	void CmdPlayerShot(string playerID, int damage)
	{
		Player player = GameManager.GetPlayer(playerID);
		player.RpcTakeDamage(damage);
	}


}
