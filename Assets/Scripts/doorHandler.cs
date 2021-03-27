using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class doorHandler : MonoBehaviour
{

    public bool location = false; //false = office, true = living room

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
            if (!location) SceneManager.LoadScene("Scenes/kitchen");
			else SceneManager.LoadScene("Scenes/living_room");         
        }
    }
}
