using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{

    public GameObject startMenu;         // Start Menu panel
    public GameObject tutorialMenu;      // Tutorial panel
    public GameObject gameplayElements;  // Parent GameObject for all main 3D gameplay elements

    public Button startButton;           // Start button reference
    public Button tutorialButton;        // Tutorial button reference
    public Button backButton;            // Back button reference
    public AudioSource audioSource;      // Audio source for button clicks

    private void Start()
    {
        ShowStartMenu(); // Initialize the game with the start menu visible
    }

    private void PlayButtonClickSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void OnStartButtonClicked()
    {
        PlayButtonClickSound();
        // Hide all UI panels and show all 3D gameplay elements
        SetMenuVisibility(false, false, true);

        // Ensure UI buttons are turned off when switching to gameplay
        SetButtonVisibility(startButton, false);
        SetButtonVisibility(tutorialButton, false);
        SetButtonVisibility(backButton, false);
    }

    public void OnTutorialButtonClicked()
    {
        PlayButtonClickSound();
        SetMenuVisibility(false, true, false); // Show tutorial panel only

        // Adjust button visibility for tutorial
        SetButtonVisibility(startButton, false);
        SetButtonVisibility(tutorialButton, false);
        SetButtonVisibility(backButton, true); // Enable back button for navigation
    }

    public void OnBackButtonClicked()
    {
        PlayButtonClickSound();
        ShowStartMenu(); // Return to start and reset UI for navigation
    }

    private void ShowStartMenu()
    {
        // Start menu is active, others including 3D elements are not
        SetMenuVisibility(true, false, false);

        // Initialize buttons for the start menu
        SetButtonVisibility(startButton, true);
        SetButtonVisibility(tutorialButton, true);
        SetButtonVisibility(backButton, false); // Ensure back button is hidden initially
    }

    private void SetMenuVisibility(bool showStart, bool showTutorial, bool showGameplay)
    {
        // Set visibility for each menu
        startMenu.SetActive(showStart);
        tutorialMenu.SetActive(showTutorial);
        gameplayElements.SetActive(showGameplay); // Ensure 3D gameplay elements are shown/hidden correctly
    }

    private void SetButtonVisibility(Button button, bool isVisible)
    {
        if (button != null)
        {
            button.gameObject.SetActive(isVisible);
        }
    }
}







