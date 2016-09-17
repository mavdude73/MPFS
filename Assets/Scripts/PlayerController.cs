using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;

	[SerializeField]
	private float lookSensitivity = 3f;

	private PlayerMotor motor;

	void Start ()
	{
		motor = GetComponent<PlayerMotor>();
	}

	void PlayerMovement()
	{
		float xMove = Input.GetAxisRaw("Horizontal");
		float zMove = Input.GetAxisRaw("Vertical");
		Vector3 moveHorizontal = transform.right * xMove;
		Vector3 moveVertical = transform.forward * zMove;
		Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;
		motor.Move(velocity);

		float yRot = Input.GetAxisRaw("Mouse X");
		Vector3 rotation = new Vector3 (0f, yRot, 0f) * lookSensitivity;
		motor.Rotate(rotation);

		float xRot = Input.GetAxisRaw("Mouse Y");
		Vector3 camRotation = new Vector3 (xRot, 0f, 0f) * lookSensitivity;
		motor.CamRotate(camRotation);




	}

	void Update()
	{
		PlayerMovement();
	}

}
