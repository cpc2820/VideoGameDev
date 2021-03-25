using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHole : MonoBehaviour
{
	public GameObject hole1;
	public GameObject hole2;
	bool location = false;
	Vector3 offset = new Vector3(0, 0, 10);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!location) {
            	other.gameObject.transform.position = hole2.transform.position - offset;
            } else {
            	other.gameObject.transform.position = hole1.transform.position - offset;
            }
        }
    }
}
