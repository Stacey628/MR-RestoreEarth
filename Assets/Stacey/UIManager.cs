using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
   public GameObject startMenu;  // The original start menu UI
    public GameObject tutorialMenu;  // The tutorial UI panel

    void Start()
    {
        ShowStartMenu();
    }

    public void OnStartButtonClicked()
    {
        // Hide the start menu and transition to main 3D scene logic
        startMenu.SetActive(false);
        // If separate scenes: SceneManager.LoadScene("Main3DScene");
        // If in the same scene: Enable main scene objects

        // Optionally, enable main gameplay elements in the same scene
        // For example: mainGameObjects.SetActive(true);
    }

    public void OnTutorialButtonClicked()
    {
        // Hide start menu and show tutorial menu
        startMenu.SetActive(false);
        tutorialMenu.SetActive(true);
    }

    public void OnBackButtonClicked()
    {
        // Go back to start menu from tutorial
        tutorialMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    public void ShowStartMenu()
    {
        // Ensure starting state is correct
        startMenu.SetActive(true);
        tutorialMenu.SetActive(false);
    }
}

