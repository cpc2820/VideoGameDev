using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchCharacterScript : MonoBehaviour {

	// referenses to controlled game objects
	public GameObject avatar1, avatar2, cable;
	public Camera camera;
	private CameraController cont;
	private bool pickup = false;

	// variable contains which avatar is on and active
	int whichAvatarIsOn = 1;

	// Use this for initialization
	void Start () {

		// anable first avatar and disable another one
		avatar1.gameObject.SetActive (true);
		avatar2.gameObject.SetActive (false);
		cont = camera.GetComponent<CameraController>();
	}

    private void Update()
    {
		pickup = avatar1.GetComponent<PlayerControllerCompMouse>().pickup;

        if (pickup && Keyboard.current.tKey.wasPressedThisFrame)
        {
			SwitchAvatar();
		}
    }

    // public method to switch avatars by pressing UI button
    public void SwitchAvatar()
	{
		// processing whichAvatarIsOn variable
		switch (whichAvatarIsOn) {

			// if the first avatar is on
			case 1:

				// then the second avatar is on now
				whichAvatarIsOn = 2;
				cont.player = avatar2.gameObject;


				// disable the first one and enable the second one
				avatar1.gameObject.SetActive (false);
				avatar2.gameObject.SetActive (true);

                avatar2.transform.rotation = avatar1.transform.rotation;
                avatar2.transform.position = avatar1.transform.position;

				break;
			// if the second avatar is on
			case 2:

				// then the first avatar is on now
				whichAvatarIsOn = 1;
				cont.player = avatar1.gameObject;

				// disable the second one and enable the first one
				avatar1.gameObject.SetActive (true);
				avatar2.gameObject.SetActive (false);

                avatar1.transform.rotation = avatar2.transform.rotation;
                avatar1.transform.position
					= new Vector3(avatar2.transform.position.x, avatar2.transform.position.y+1, avatar2.transform.position.z);

				break;
			}
			
	}
}
