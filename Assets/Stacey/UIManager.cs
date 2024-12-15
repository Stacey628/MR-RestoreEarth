using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
   
    
    public GameObject startMenu;         // UI panel with "Start" and "Tutorial" buttons
    public GameObject tutorialMenu;      // UI panel for Tutorial content
    public GameObject gameplayElements;  // Parent GameObject for the main 3D gameplay elements

    public Button backButton;            // The "back" button in the tutorial panel

    void Start()
    {
        ShowStartMenu();
    }

    public void OnStartButtonClicked()
    {
        // Hide all UI elements and start the main 3D gameplay
        startMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        gameplayElements.SetActive(true);
        backButton.gameObject.SetActive(false); // Ensure "Next" button is hidden
    }

    public void OnTutorialButtonClicked()
    {
        // Show the Tutorial Menu and enable the Next button
        startMenu.SetActive(false);
        tutorialMenu.SetActive(true);
        backButton.gameObject.SetActive(true); // Enable "Next" button when in tutorial
    }

    public void OnBackButtonClicked()
    {
        // Return to the Start Menu and disable the Next button
        tutorialMenu.SetActive(false);
        startMenu.SetActive(true);
        backButton.gameObject.SetActive(false); // Disable "Next" button
    }

    public void ShowStartMenu()
    {
        // Show start menu and initially disable tutorial and gameplay UI
        startMenu.SetActive(true);
        tutorialMenu.SetActive(false);
        gameplayElements.SetActive(false);

        // Ensure "Next" button is disabled at start
        if (backButton != null)
        {
            backButton.gameObject.SetActive(false);
        }
    }
}



