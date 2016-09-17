using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

	[SerializeField]
	private GameObject visor;

	[SerializeField]
	private string remoteLayerName = "RemotePlayer";

	Camera sceneCamera;

	void Start ()
	{
		if(!isLocalPlayer)
		{
			DisableComponents();
			AssignPlayerLayer();
		}
		else
		{
			sceneCamera = Camera.main;
			sceneCamera.gameObject.SetActive(false);
			visor.SetActive(false);
		}

		GetComponent<Player>().Setup();

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
		if(sceneCamera != null)
		{
			sceneCamera.gameObject.SetActive(true);

		}
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		GameManager.UnregisterPlayer(transform.name);
	}


}
