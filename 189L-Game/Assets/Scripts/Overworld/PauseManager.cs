using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overworld;

public class PauseManager : MonoBehaviour
{
    private SceneGameManager sceneGameManager;
    [SerializeField] private GameObject pauseMenu;
    private bool isPaused;

    private void Start()
    {
        sceneGameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<SceneGameManager>();
        isPaused = false;
        Debug.Log(sceneGameManager);
    }

    private void Update()
    {
        if(Input.GetButtonDown("Pause") && !isPaused)
        {
            PauseMenu();
        }
        else if (Input.GetButtonDown("Pause") && isPaused)
        {
            UnpauseMenu();
        }
    }

    private void PauseMenu()
    {
        Time.timeScale = 0.0f;
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void UnpauseMenu()
    {
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    public void ShowRules()
    {
        Debug.Log("show rules");
    }

    public void QuitGame()
    {
        StartCoroutine(sceneGameManager.LoadTitleScene());
        Debug.Log("go home");
    }
}
