using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ventHandler : MonoBehaviour
{
    public AudioClip winsound;
    public bool location = false; //false = office, true = living room
    public GameObject player;
    public int indicator_count;
    private float delayTime = 1f;

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
        if (other.gameObject.tag == "Player" && player.GetComponent<PlayerController>().count == indicator_count)
        {
            AudioSource.PlayClipAtPoint(winsound, transform.position);
            Invoke("DelayedAction", delayTime);

        }
    }

    void DelayedAction()
    {
        if (!location) SceneManager.LoadScene("Scenes/office");
        else SceneManager.LoadScene("Scenes/living_room");
    }
}
