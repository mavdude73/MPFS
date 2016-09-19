using UnityEngine;
using UnityEngine.Networking;

public class WeaponMananger : NetworkBehaviour {

	[SerializeField]
	private string weaponLayerName = "Weapon";

	[SerializeField]
	private Transform weaponHolder;
	
	[SerializeField]
	private PlayerWeapon primaryWeapon;

	private PlayerWeapon currentWeapon;
	private WeaponGraphics currentGraphics;

	void Start ()
	{
		EquipWeapon(primaryWeapon);
	}

	void EquipWeapon (PlayerWeapon wep)
	{
		currentWeapon = wep;
		GameObject weaponInstance = (GameObject)Instantiate(wep.graphics, weaponHolder.position, weaponHolder.rotation);
		weaponInstance.transform.SetParent(weaponHolder);

		currentGraphics = weaponInstance.GetComponent<WeaponGraphics>();
		if(currentGraphics == null)
		{
			Debug.LogError("No weapon graphics componet on the weapon object");
		}

		if(isLocalPlayer)
		{
			Util.SetLayerRecursively(weaponInstance, LayerMask.NameToLayer(weaponLayerName));
		}
	}

	public PlayerWeapon GetCurrentWeapon ()
	{
		return currentWeapon;
	}

	public WeaponGraphics GetCurrentGraphics ()
	{
		return currentGraphics;
	}


}
