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

    [SerializeField] private GameObject descMenu;
    private bool isInDescMenu;
    [SerializeField] private GameObject tankDesc;
    [SerializeField] private GameObject supportDesc;
    [SerializeField] private GameObject healerDesc;
    [SerializeField] private GameObject rangerDesc;

    private bool menuOpen = false;

    private void Start()
    {
        sceneGameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<SceneGameManager>();
        isPaused = false;
        isInDescMenu = false;
        menuOpen = false;
    }

    private void Update()
    {
        if(isPaused || isInDescMenu)
        {
            menuOpen = true;
        }
        else
        {
            menuOpen = false;
        }

        if(Input.GetButtonDown("Pause") && !menuOpen)
        {
            PauseMenu();
        }
        else if(Input.GetButtonDown("Pause") && isPaused)
        {
            UnpauseMenu();
        }

        if(Input.GetButtonDown("Description") && !menuOpen)
        {
            OpenBios();
        }
        else if(Input.GetButtonDown("Description") && isInDescMenu)
        {
            CloseBios();
        }

    }

    public void PauseMenu()
    {
        Time.timeScale = 0.0f;
        this.canvas.SetActive(true);
        this.rulesMenu.SetActive(false);
        this.pauseMenu.SetActive(true);
        this.isPaused = true;
    }

    public void UnpauseMenu()
    {
        Time.timeScale = 1.0f;
        this.canvas.SetActive(false);
        this.pauseMenu.SetActive(false);
        this.rulesMenu.SetActive(false);
        this.isPaused = false;
    }

    public void ShowRules()
    {
        this.rulesMenu.SetActive(true);
        this.pauseMenu.SetActive(false);
    }

    public void QuitGame()
    {
        StartCoroutine(this.sceneGameManager.LoadTitleScene());
    }

    private void OpenBios()
    {
        Time.timeScale = 0.0f;
        this.EnableBios(1);
        this.isInDescMenu = true;
        this.descMenu.SetActive(true);
    }

    private void CloseBios()
    {
        Time.timeScale = 1.0f;
        this.EnableBios(0);
        this.isInDescMenu = false;
        this.descMenu.SetActive(false);
    }

    public void EnableBios(int numDesc)
    {
        switch(numDesc)
        {
            case 1:
                tankDesc.SetActive(true);
                supportDesc.SetActive(false);
                healerDesc.SetActive(false);
                rangerDesc.SetActive(false);
                break;
            case 2:
                tankDesc.SetActive(false);
                supportDesc.SetActive(true);
                healerDesc.SetActive(false);
                rangerDesc.SetActive(false);
                break;
            case 3:
                tankDesc.SetActive(false);
                supportDesc.SetActive(false);
                healerDesc.SetActive(true);
                rangerDesc.SetActive(false);
                break;
            case 4:
                tankDesc.SetActive(false);
                supportDesc.SetActive(false);
                healerDesc.SetActive(false);
                rangerDesc.SetActive(true);
                break;
            default:
                tankDesc.SetActive(false);
                supportDesc.SetActive(false);
                healerDesc.SetActive(false);
                rangerDesc.SetActive(false);
                break;
        }
    }
}
