using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overworld;

public class PauseManager : MonoBehaviour
{
    private SceneGameManager sceneGameManager;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject rulesMenu;
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

    public void PauseMenu()
    {
        Time.timeScale = 0.0f;
        canvas.SetActive(true);
        rulesMenu.SetActive(false);
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void UnpauseMenu()
    {
        Time.timeScale = 1.0f;
        canvas.SetActive(false);
        pauseMenu.SetActive(false);
        rulesMenu.SetActive(false);
        isPaused = false;
    }

    public void ShowRules()
    {
        rulesMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void QuitGame()
    {
        StartCoroutine(sceneGameManager.LoadTitleScene());
        Debug.Log("go home");
    }
}
