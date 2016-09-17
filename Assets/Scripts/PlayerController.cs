using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;

	[SerializeField]
	private float lookSensitivityY = 3f;

	[SerializeField]
	private float lookSensitivityX = 1f;


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
		Vector3 rotation = new Vector3 (0f, yRot, 0f) * lookSensitivityY;
		motor.Rotate(rotation);

		float xRot = Input.GetAxisRaw("Mouse Y");
		float camRotation = xRot * lookSensitivityX;
		motor.CamRotate(camRotation);
	}

	void CursorLock()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		if(Cursor.visible)
		{
			if(Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}

	}

	void Update()
	{
		PlayerMovement();
		CursorLock();
	}




}
