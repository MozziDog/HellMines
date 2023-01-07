using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PauseMenuUI : MonoBehaviour
{
    bool isPaused;
    [SerializeField] GameObject UI;
    [SerializeField] StarterAssetsInputs input;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPause()
    {
        if (GameManager.instance.isPlayMode)
        {
            if (!isPaused)
            {
                isPaused = true;
                Time.timeScale = 0f;
                UI.SetActive(true);
                AudioListener.volume = 0f;
                input.look = Vector2.zero;
                input.cursorInputForLook = false;
                input.cursorLocked = false;
                input.SetCursorState(false);
            }
            else
            {
                isPaused = false;
                Time.timeScale = 1f;
                UI.SetActive(false);
                AudioListener.volume = 1f;
                input.cursorInputForLook = true;
                input.cursorLocked = true;
                input.SetCursorState(true);
            }
        }
    }
}
