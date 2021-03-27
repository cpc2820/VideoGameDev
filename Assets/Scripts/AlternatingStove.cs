using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingStove : MonoBehaviour
{
	public ParticleSystem one;
	public ParticleSystem two;
	public ParticleSystem three;
	public ParticleSystem four;
	public ParticleSystem five;
    private SphereCollider oneCollider;
    private SphereCollider twoCollider;
    private SphereCollider threeCollider;
    private SphereCollider fourCollider;
    private SphereCollider fiveCollider;

	public GameObject hole2;
    public float timeRemaining = 5;
	Vector3 offset = new Vector3(0, 0, 10);

    //state enums
    public enum StoveState {
        First, Middle, Last
    }

    public StoveState state;
    // Start is called before the first frame update
    void Start()
    {
        state = StoveState.First;
        oneCollider = one.GetComponent<SphereCollider>();
        twoCollider = two.GetComponent<SphereCollider>();
        threeCollider = one.GetComponent<SphereCollider>();
        fourCollider = two.GetComponent<SphereCollider>();
        fiveCollider = one.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(state) {

            case StoveState.First:
            //setting emissions for particle systems
            one.enableEmission = true;
            two.enableEmission = false;
            three.enableEmission = true;
            four.enableEmission = false;
            five.enableEmission = true;

            //colliders for particle systems
            oneCollider.enabled = true;
            twoCollider.enabled = false;
            threeCollider.enabled = true;
            fourCollider.enabled = false;
            fiveCollider.enabled = true;
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            } else {
                nextState();
            }
            
            break;

            case StoveState.Middle:
            one.enableEmission = true;
            two.enableEmission = false;
            three.enableEmission = false;
            four.enableEmission = true;
            five.enableEmission = true;

            //colliders for particle systems
            oneCollider.enabled = true;
            twoCollider.enabled = false;
            threeCollider.enabled = false;
            fourCollider.enabled = true;
            fiveCollider.enabled = true;
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            } else {
                nextState();
            }
            break;

            case StoveState.Last:
            one.enableEmission = true;
            two.enableEmission = true;
            three.enableEmission = false;
            four.enableEmission = true;
            five.enableEmission = false;

            //colliders for particle systems
            oneCollider.enabled = true;
            twoCollider.enabled = true;
            threeCollider.enabled = false;
            fourCollider.enabled = true;
            fiveCollider.enabled = false;
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            } else {
                nextState();
            }
        
            break;
        }
    }

    private void nextState() {
        //switch states 
        if (state == StoveState.First) {
            state = StoveState.Middle;
        } else if (state == StoveState.Middle) {
            state = StoveState.Last;
        } else if (state == StoveState.Last) {
            state = StoveState.First;
        }
        timeRemaining = 5;   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            other.gameObject.transform.position = hole2.transform.position - offset;
        }
    }


}
