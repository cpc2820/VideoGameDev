using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PauseMenuToggle : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Awake()
    {
        if (GetComponent<CanvasGroup>() == null) Debug.LogError("Canvas Group not found!");
        else canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (canvasGroup.interactable)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.alpha = 0f;
                Time.timeScale = 1f;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.alpha = 1f;
                Time.timeScale = 0f;
            }
        }
    }
}
