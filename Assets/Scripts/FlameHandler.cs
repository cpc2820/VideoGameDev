//Author: Joshua Suber
//Scene: kitchen

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameHandler : MonoBehaviour
{
    //gas stove flames
	public ParticleSystem flame;
    public ParticleSystem flame2;
    public ParticleSystem flame3;
    public ParticleSystem flame4;
    public ParticleSystem flame5;
    public ParticleSystem flame6;

    //colliders for particle systems 
    private Collider flameC;
    private Collider flameC2;
    private Collider flameC3;
    private Collider flameC4;
    private Collider flameC5;
    private Collider flameC6;

    //last checkpoint
    public GameObject hole2;

    //offset from last checkpoint location
	Vector3 offset = new Vector3(0, 0, 10);

    //time remaining from state countdown
    public float timeRemaining = 5;

    //state enums
    public enum StoveState {
        First, Second, Third, Four
    }

    public StoveState state;

    public int maxHealth = 3;
    public int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        flameC = flame.GetComponent<Collider>();
        flameC2 = flame2.GetComponent<Collider>();
        flameC3 = flame3.GetComponent<Collider>();
        flameC4 = flame4.GetComponent<Collider>();
        flameC5 = flame5.GetComponent<Collider>();
        flameC6 = flame6.GetComponent<Collider>();

        currentHealth = maxHealth;
    }

    void Update()
    {
        switch(state) {

            case StoveState.First:
            //enabling emission for particle systems 
            flame.enableEmission = false;
            flame2.enableEmission = true;
            flame3.enableEmission = false;
            flame4.enableEmission = true;
            flame5.enableEmission = true;
            flame6.enableEmission = false;
      

            //colliders for particle systems
            flameC.enabled = false;
            flameC2.enabled = true;
            flameC3.enabled = false;
            flameC4.enabled = true;
            flameC5.enabled = true;
            flameC6.enabled = false;

            //state change countdown
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            } else {
                nextState();
            }
            
            break;

            case StoveState.Second:
            //enabling emission for particle systems 
            flame.enableEmission = true;
            flame2.enableEmission = false;
            flame3.enableEmission = false;
            flame4.enableEmission = false;
            flame5.enableEmission = true;
            flame6.enableEmission = true;

            //enable collides
            flameC.enabled = true;
            flameC2.enabled = false;
            flameC3.enabled = false;
            flameC4.enabled = false;
            flameC5.enabled = true;
            flameC6.enabled = true;

            //state change countdown
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            } else {
                nextState();
            }
            break;

            case StoveState.Third:
            //enabling emission for particle systems 
            flame.enableEmission = false;
            flame2.enableEmission = true;
            flame3.enableEmission = true;
            flame4.enableEmission = false;
            flame5.enableEmission = true;
            flame6.enableEmission = false;

            //enable collides
            flameC.enabled = false;
            flameC2.enabled = true;
            flameC3.enabled = true;
            flameC4.enabled = false;
            flameC5.enabled = true;
            flameC6.enabled = false;

            //state change countdown
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            } else {
                nextState();
            }
            break;

            case StoveState.Four:
            //enabling emission for particle systems 
            flame.enableEmission = true;
            flame2.enableEmission = true;
            flame3.enableEmission = false;
            flame4.enableEmission = true;
            flame5.enableEmission = false;
            flame6.enableEmission = false;

            //enable collides
            flameC.enabled = true;
            flameC2.enabled = true;
            flameC3.enabled = false;
            flameC4.enabled = true;
            flameC5.enabled = false;
            flameC6.enabled = false;

            //state change countdown
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
            state = StoveState.Second;
        } else if (state == StoveState.Second) {
            state = StoveState.Third;
        } else if (state == StoveState.Third) {
            state = StoveState.Four;
        } else if (state == StoveState.Four) {
            state = StoveState.First;
        }   
        timeRemaining = 5;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (currentHealth == 0) {
                other.gameObject.transform.position = hole2.transform.position - offset;
                currentHealth = maxHealth;
            } else {
                TakeDamage(1);
                other.gameObject.transform.position = hole2.transform.position - offset;
            }
        }
    }

    void TakeDamage(int damage) {
        currentHealth -= damage;
    }
}
