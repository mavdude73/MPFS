using UnityEngine;
using System.Collections;

public class PlayerUI : MonoBehaviour {


	[SerializeField]
	private GameObject pauseMenu;

	void Start ()
	{
		PauseMenu.GamePaused = false;
	}

	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			TogglePauseMenu();
		}
	}

	public void TogglePauseMenu()
	{
		pauseMenu.SetActive(!pauseMenu.activeSelf);
		PauseMenu.GamePaused = pauseMenu.activeSelf;
	}

}
