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
    }

    // Update is called once per frame
    void Update()
    {
        switch(state) {

            case StoveState.First:
            one.Play();
            two.Play();
            three.Play();
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            } else {
                nextState();
            }
            
            break;

            case StoveState.Middle:
            
            four.Play();
            five.Play();
            break;

            case StoveState.Last:
            one.Play();
            two.Play();
        
            break;
        }
    }

    private void nextState() {
        //switch states 
        if (state == StoveState.First) {
            state = StoveState.Middle;
        } if (state == StoveState.Middle) {
            state = StoveState.Last;
        } if (state == StoveState.Last) {
            state = StoveState.First;
        }
        timeRemaining = 5;    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position = hole2.transform.position - offset;
        }
    }
}
