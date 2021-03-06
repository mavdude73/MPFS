﻿using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 3f;

	[SerializeField]
	private float lookSensitivityY = 3f;

	[SerializeField]
	private float lookSensitivityX = 1f;

	private Animator anim;
	private PlayerMotor motor;

	void Start ()
	{
		motor = GetComponent<PlayerMotor>();
		anim = GetComponent<Animator>();
	}

	void PlayerMovement()
	{
		float xMove = Input.GetAxisRaw("Horizontal");
		float zMove = Input.GetAxisRaw("Vertical");
		Vector3 moveHorizontal = transform.right * xMove;
		Vector3 moveVertical = transform.forward * zMove;
		Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

		anim.SetFloat("Velocity", zMove);

		motor.Move(velocity);

		float yRot = Input.GetAxisRaw("Mouse X");
		Vector3 rotation = new Vector3 (0f, yRot, 0f) * lookSensitivityY;
		motor.Rotate(rotation);

		float xRot = Input.GetAxisRaw("Mouse Y");
		float camRotation = xRot * lookSensitivityX;
		motor.CamRotate(camRotation);
	}

	void CursorLock(bool isPaused)
	{
		if(isPaused)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}


	void Update()
	{
		if(PauseMenu.GamePaused)
		{
			motor.Move(Vector3.zero);
			motor.Rotate(Vector3.zero);
			motor.CamRotate(0f);

			CursorLock(true);
			return;
		}

		CursorLock(false);
		PlayerMovement();

	}




}
