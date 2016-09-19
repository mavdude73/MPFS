using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

	[SerializeField]
	private string remoteLayerName = "RemotePlayer";

	[SerializeField]
	private string dontDrawLayerName = "DontDraw";

	[SerializeField]
	private GameObject playerGraphics;

	[SerializeField]
	private GameObject playerUIPrefab;

	[HideInInspector]
	public GameObject playerUIInstance;



	void Start ()
	{
		if(!isLocalPlayer)
		{
			DisableComponents();
			AssignPlayerLayer();
		}
		else
		{
			
			SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

			playerUIInstance= Instantiate(playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;
			GetComponent<Player>().SetupPlayer();

		}

	}

	void SetLayerRecursively(GameObject obj, int newLayer)
	{
		obj.layer = newLayer;

		foreach (Transform child in obj.transform)
		{
			SetLayerRecursively(child.gameObject, newLayer);
		}
	}

	public override void OnStartClient ()
	{
		string netID = GetComponent<NetworkIdentity>().netId.ToString();
		Player player = GetComponent<Player>();
		GameManager.RegisterPlayer(netID, player);
	}



	void AssignPlayerLayer()
	{
		gameObject.layer = LayerMask.NameToLayer (remoteLayerName);
	}

	void DisableComponents()
	{
		for(int i = 0; i < componentsToDisable.Length; i++)
			{
				componentsToDisable[i].enabled = false;
			}

	}

	void OnDisable()
	{
//		Cursor.lockState = CursorLockMode.None;
//		Cursor.visible = true;

		if(isLocalPlayer)
		{
			GameManager.instance.SetSceneCameraActive(true);
		}

		Destroy(playerUIInstance);

		GameManager.UnregisterPlayer(transform.name);
	}


}
