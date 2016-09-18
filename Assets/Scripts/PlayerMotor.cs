using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private float cameraRotationLimit = 85f;

	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private float camRotation = 0f;
	private float currentCamRotation = 0f;

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

	public void CamRotate(float rot)
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
			rb.MovePosition(rb.position + velocity * Time.deltaTime);
		}
	}

	void PerformRotation()
	{
		rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
		if(cam != null)
		{
			currentCamRotation -= camRotation;
			currentCamRotation = Mathf.Clamp(currentCamRotation, -cameraRotationLimit, cameraRotationLimit);
			cam.transform.localEulerAngles = new Vector3 (currentCamRotation, 0f, 0f);
		}
	}

}
