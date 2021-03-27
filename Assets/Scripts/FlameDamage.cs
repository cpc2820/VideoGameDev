using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDamage : MonoBehaviour
{
	public ParticleSystem flame;
	public GameObject hole2;
	Vector3 offset = new Vector3(0, 0, 10);
    // Start is called before the first frame update
    void Start()
    {
        flame  = GetComponent<ParticleSystem>();
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
