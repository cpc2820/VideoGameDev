﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinState : MonoBehaviour
{
    private float delayTime = 1f;
    public AudioClip winsound;
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
            AudioSource.PlayClipAtPoint(winsound, transform.position);
            Invoke("DelayedAction", delayTime);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void DelayedAction()
    {
        SceneManager.LoadScene("Scenes/win_screen");
    }
}
