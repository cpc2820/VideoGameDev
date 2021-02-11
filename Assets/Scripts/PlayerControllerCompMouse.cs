using UnityEngine;

// Include the namespace required to use Unity UI and Input System
using UnityEngine.InputSystem;
using TMPro;

public class PlayerControllerCompMouse : MonoBehaviour
{

	// Create public variables for player speed, and for the Text UI game objects
	public float speed;

	private float movementX;
	private float movementY;
	private float movementZ;

	private Rigidbody rb;
	public GameObject cable;
	public bool pickup = false;


	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		Vector3 movement = new Vector3(movementX, movementZ, movementY);

		rb.AddForce((new Vector3(movementX, 0.0f, movementY)) * speed);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("PickUp"))
		{
			other.gameObject.SetActive(false);
			pickup = true;
			Object.Destroy(cable);
		}
	}

	void OnMove(InputValue value)
	{
		Vector2 v = value.Get<Vector2>();

		movementX = v.x;
		movementY = v.y;
		movementZ = 0.0f;
	}
}