using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

	[SerializeField]
	private GameObject visor;

	Camera sceneCamera;

	void Start ()
	{
		if(!isLocalPlayer)
		{
			for(int i = 0; i < componentsToDisable.Length; i++)
			{
				componentsToDisable[i].enabled = false;
			}
		}
		else
		{
			sceneCamera = Camera.main;
			sceneCamera.gameObject.SetActive(false);
			visor.SetActive(false);
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
	}


}
