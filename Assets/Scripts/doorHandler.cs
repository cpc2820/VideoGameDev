using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class doorHandler : MonoBehaviour
{

    public bool location = false; //false = office, true = living room
    private int count;
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
        if (count >= 3 && other.gameObject.tag == "Player")
        {
            if (!location) SceneManager.LoadScene("Scenes/kitchen");
			else SceneManager.LoadScene("Scenes/living_room");         
        }
    }
}
