﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera cam;

	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private Vector3 camRotation = Vector3.zero;

	private Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}

	public void Move(Vector3 vel)
	{
		velocity = vel;
	}

	public void Rotate(Vector3 rot)
	{
		rotation = rot;
	}

	public void CamRotate(Vector3 rot)
	{
		camRotation = rot;
	}

	void FixedUpdate ()
	{
		PerformMovement();
		PerformRotation();
	}

	void PerformMovement()
	{
		if(velocity != Vector3.zero)
		{
			rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
		}
	}

	void PerformRotation()
	{
		rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
		if(cam != null)
		{
			cam.transform.Rotate(-camRotation);
		}
	}

}
