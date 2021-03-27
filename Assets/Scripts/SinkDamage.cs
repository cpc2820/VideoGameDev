using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkDamage : MonoBehaviour
{
	public GameObject sink;
	public GameObject hole2;
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
        	
            other.gameObject.transform.position = hole2.transform.position - offset;
        }
    }
}